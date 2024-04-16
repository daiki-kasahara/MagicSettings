#pragma once

#include <windows.h>
#include <string>
#include <optional>
#include <thread>
#include <functional>

class PipeServer
{
private:
    inline static const std::wstring PipeName = L"\\\\.\\pipe\\MagicSettings-ScreenController";

    inline static const std::string CloseCmd = "Close";
    inline static const std::string UpdateCmd = "Update";
    inline static const std::string TerminateCmd = "Terminate";

    static const DWORD ReadElapsed = 100;
    static const DWORD WriteElapsed = 100;

public:
    PipeServer(HWND hWnd) : _hWnd(hWnd) {}

public:
    auto OpenPipe() noexcept -> bool;
    auto ClosePipe() noexcept -> bool;
    auto SetUpdateProcedure(std::function<std::string()> updateFunc) -> void;

private:
    auto PipeThread() noexcept -> void;
    auto ReadMessage() const noexcept -> std::optional<std::string>;
    auto WriteMessage(HANDLE pipeHandle, std::string returnMessage) const noexcept -> bool;

private:
    HANDLE _pipeHandle = nullptr;
    std::optional<std::thread> _pipeThreadHandle = std::nullopt;
    std::optional<std::function<std::string()>> _updateFunc;
    HWND _hWnd;
};
