#include "framework.h"
#include "ScreenController.h"
#include <nlohmann/json.hpp>

#include "PipeServer.h"
#include "ScreenFilter.h"
#include "ScreenSettingsRepository.h"

using namespace ScreenController::Repositories;
using namespace ScreenController::Services;

#define MAX_LOADSTRING 100

// グローバル変数
HINSTANCE g_hInst;
WCHAR g_szWindowClass[MAX_LOADSTRING];
ScreenFilter g_screenFilter{};
PipeServer g_pipeServer = nullptr;
bool g_isPipeRunning = false;
HANDLE g_hMutex;

// プロトタイプ宣言
auto MyRegisterClass(HINSTANCE hInstance) -> ATOM;
auto InitInstance(HINSTANCE, int) -> bool;
auto GetFilterValue() noexcept -> BlueLightBlockingFilter;

LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR    lpCmdLine,
    _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // グローバル文字列を初期化する
    LoadStringW(hInstance, IDC_SCREENCONTROLLER, g_szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // アプリケーション初期化の実行:
    if (!InitInstance(hInstance, nCmdShow))
        return FALSE;

    auto hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_SCREENCONTROLLER));

    // メイン メッセージ ループ:
    MSG msg;
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int)msg.wParam;
}

/// <summary>
/// ウィンドウ クラスの登録
/// </summary>
/// <param name="hInstance"></param>
/// <returns></returns>
auto MyRegisterClass(HINSTANCE hInstance) -> ATOM
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = nullptr;
    wcex.hCursor = nullptr;
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = nullptr;
    wcex.lpszClassName = g_szWindowClass;
    wcex.hIconSm = nullptr;

    return RegisterClassExW(&wcex);
}

/// <summary>
/// インスタンスの初期化
/// </summary>
/// <param name="hInstance"></param>
/// <param name="nCmdShow"></param>
/// <returns></returns>
auto InitInstance(HINSTANCE hInstance, int nCmdShow) -> bool
{
    g_hMutex = CreateMutexW(nullptr, 0, L"MagicSettings.ScreenController");
    g_hInst = hInstance;

    // 起動していたらすぐに終了させる
    if (::GetLastError() == ERROR_ALREADY_EXISTS && g_hMutex != nullptr)
    {
        ReleaseMutex(g_hMutex);
        CloseHandle(g_hMutex);
        return false;
    }


    auto hWnd = CreateWindowW(g_szWindowClass, nullptr, WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);
    if (!hWnd)
        return false;

    ShowWindow(hWnd, SW_HIDE);

    g_pipeServer = PipeServer(hWnd);
    g_isPipeRunning = g_pipeServer.OpenPipe();

    return true;
}

/// <summary>
/// ウィンドウプロシージャ
/// </summary>
/// <param name="hWnd"></param>
/// <param name="message"></param>
/// <param name="wParam"></param>
/// <param name="lParam"></param>
/// <returns></returns>
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_CREATE:
    {
        // スクリーンフィルターを適用する
        g_screenFilter.Initialize();
        g_screenFilter.Set(GetFilterValue());
        break;
    }
    case WM_COMMAND:
    {
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    case WM_DESTROY:
    {
        // フィルターのリセット
        g_screenFilter.Uninitialize();

        // パイプを閉じる
        if (g_isPipeRunning)
        {
            g_pipeServer.ClosePipe();
        }

        if (g_hMutex != nullptr)
        {
            ReleaseMutex(g_hMutex);
            CloseHandle(g_hMutex);
        }

        // 終了時は設定を無効にする
        ScreenSettingsRepository().Set(false);

        PostQuitMessage(0);
        break;
    }
    case WM_CUSTOM_EXIT_MESSAGE:
    {
        DestroyWindow(hWnd);
        break;
    }
    case WM_CUSTOM_UPDATE_MESSAGE:
    {
        return g_screenFilter.Set(GetFilterValue()) ? 0 : 1;
    }
    case WM_PAINT:
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }

    return 0;
}

auto GetFilterValue() noexcept -> BlueLightBlockingFilter
{
    return ScreenSettingsRepository().Get();
}