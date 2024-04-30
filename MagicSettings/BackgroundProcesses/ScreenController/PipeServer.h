#pragma once

#include <windows.h>
#include <string>
#include <optional>
#include <thread>
#include <functional>

/// <summary>
/// パイプサーバー
/// </summary>
class PipeServer
{
private:
    inline static const std::wstring PipeName = L"\\\\.\\pipe\\MagicSettings-ScreenController";

    inline static const std::string CloseCmd = "Close";
    inline static const std::string UpdateCmd = "Update";
    inline static const std::string TerminateCmd = "Terminate";
    inline static const std::string CheckCmd = "Check";

    static const DWORD ReadElapsed = 100;
    static const DWORD WriteElapsed = 100;

public:
    PipeServer(HWND hWnd) : _hWnd(hWnd) {}

public:
    /// <summary>
    /// パイプを開く
    /// </summary>
    /// <returns></returns>
    auto OpenPipe() noexcept -> bool;

    /// <summary>
    /// パイプを閉じる
    /// </summary>
    /// <returns></returns>
    auto ClosePipe() noexcept -> bool;

private:
    auto PipeThread() noexcept -> void;
    auto ReadMessage() const noexcept -> std::optional<std::string>;
    auto WriteMessage(HANDLE pipeHandle, std::string returnMessage) const noexcept -> bool;

private:
    HANDLE _pipeHandle = nullptr;
    std::optional<std::thread> _pipeThreadHandle = std::nullopt;
    HWND _hWnd;
};
