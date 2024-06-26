﻿#include "PipeServer.h"
#include <vector>
#include <nlohmann/json.hpp>
#include "ScreenController.h"

auto PipeServer::OpenPipe() noexcept -> bool
{
    // すでにパイプオープンしていたら何もせずに true を返す
    if (_pipeThreadHandle.has_value())
        return true;

    // パイプ作成
    _pipeHandle = CreateNamedPipeW(
        PipeName.c_str(),
        PIPE_ACCESS_DUPLEX,
        PIPE_TYPE_BYTE | PIPE_WAIT,
        1,
        0,
        0,
        0,
        nullptr);

    if (_pipeHandle == INVALID_HANDLE_VALUE)
    {
        _pipeHandle = nullptr;
        return false;
    }

    // 別スレッドでパイプ処理を実行
    _pipeThreadHandle = std::thread(&PipeServer::PipeThread, this);

    return true;
}

auto PipeServer::ClosePipe() noexcept -> bool
{
    // 名前付きパイプを開く
    auto hPipe = CreateFileW(PipeName.c_str(),
        GENERIC_READ | GENERIC_WRITE,
        0,
        nullptr,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        nullptr);

    if (hPipe == INVALID_HANDLE_VALUE)
        return false;

    // パイプ終了メッセージを送信
    nlohmann::json json;
    json["Cmd"] = "Close";
    json["Args"] = "";
    auto jsonString = json.dump();

    auto ret = WriteMessage(hPipe, jsonString);

    // パイプ送信に成功した時のみ、スレッドの終了を待機する
    if (ret && _pipeThreadHandle.has_value() && _pipeThreadHandle.value().joinable())
        _pipeThreadHandle.value().join();

    CloseHandle(hPipe);
    hPipe = nullptr;

    return true;
}

auto PipeServer::PipeThread() noexcept -> void
{
    while (1)
    {
        if (!ConnectNamedPipe(_pipeHandle, nullptr))
        {
            if (GetLastError() == ERROR_PIPE_CONNECTED)
            {
                DisconnectNamedPipe(_pipeHandle);
            }
            continue;
        }

        const auto request = ReadMessage();

        if (!request.has_value())
        {
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }

        nlohmann::json json;
        try
        {
            json = nlohmann::json::parse(request.value());
        }
        catch (const std::exception&)
        {
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }

        if (json.is_null())
        {
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }

        auto& cmdJson = json["Cmd"];
        auto& argsJson = json["Args"];

        // argsは空のパターンもあるのでチェックしない
        if (cmdJson.is_null())
        {
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }

        const auto cmd = cmdJson.get<std::string>();
        const auto args = argsJson.get<std::string>();

        if (cmd == UpdateCmd)
        {
            // 設定情報を更新する
            // メインメスレッドでしかフィルター更新できないため自分自身にSendMessageする
            auto ret = SendMessage(_hWnd, WM_CUSTOM_UPDATE_MESSAGE, 0, 0);

            nlohmann::json json;
            json["ReturnCode"] = ret;
            json["ReturnParameters"] = "";
            std::ignore = WriteMessage(_pipeHandle, json.dump());

            FlushFileBuffers(_pipeHandle);
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }
        else if (cmd == TerminateCmd)
        {
            // アプリを終了する
            nlohmann::json json;
            json["ReturnCode"] = 0;
            json["ReturnParameters"] = "";
            std::ignore = WriteMessage(_pipeHandle, json.dump());

            FlushFileBuffers(_pipeHandle);
            DisconnectNamedPipe(_pipeHandle);
            PostMessage(_hWnd, WM_CUSTOM_EXIT_MESSAGE, 0, 0);
            continue;
        }
        else if (cmd == CloseCmd)
        {
            // パイプを終了する
            FlushFileBuffers(_pipeHandle);
            DisconnectNamedPipe(_pipeHandle);
            CloseHandle(_pipeHandle);
            _pipeHandle = nullptr;
            return;
        }
        else if (cmd == CheckCmd)
        {
            // 何もせず成功を返す
            nlohmann::json json;
            json["ReturnCode"] = 0;
            json["ReturnParameters"] = "";
            std::ignore = WriteMessage(_pipeHandle, json.dump());

            FlushFileBuffers(_pipeHandle);
            DisconnectNamedPipe(_pipeHandle);
            continue;
        }
        else
        {
            // 何もしない
        }

        // 処理が終わったら Disconnect する
        DisconnectNamedPipe(_pipeHandle);
    }
}

auto PipeServer::ReadMessage() const noexcept -> std::optional<std::string>
{
    auto ReadMessageFromPipe = [](HANDLE hFile, LPVOID lpBuffer, DWORD nNumberOfBytesToRead, LPDWORD lpNumberOfBytesRead, DWORD timeout, LPDWORD lpLastError)
        {
            auto ov = OVERLAPPED{ 0 };
            auto gle = DWORD{ 0 };

            auto result = ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, &ov);
            if (!result)
            {
                gle = GetLastError();
                if (gle == ERROR_IO_PENDING)
                {
                    for (DWORD passed = 0; passed < timeout; passed += ReadElapsed)
                    {
                        if (HasOverlappedIoCompleted(&ov))
                        {
                            gle = 0;
                            result = TRUE;
                            break;
                        }
                        Sleep(ReadElapsed);
                    }
                }
            }

            if (lpLastError)
            {
                *lpLastError = gle;
            }
            if (lpNumberOfBytesRead)
            {
                *lpNumberOfBytesRead = static_cast<DWORD>(ov.InternalHigh);
            }

            return result;
        };

    auto bufH = BYTE{ 0 };
    auto bufL = BYTE{ 0 };
    auto recvBuffer = DWORD{ 0 };

    // クライアントからのリクエストのバッファ情報を受信
    if (!ReadMessageFromPipe(_pipeHandle, &bufH, sizeof(bufH), &recvBuffer, 1000, nullptr))
        return std::nullopt;

    if (!ReadMessageFromPipe(_pipeHandle, &bufL, sizeof(bufL), &recvBuffer, 1000, nullptr))
        return std::nullopt;

    const auto recvBufferLength = bufH * 256 + bufL;

    if (recvBufferLength == 0)
        return std::nullopt;

    auto buff = std::vector<char>(recvBufferLength);

    // クライアントからのリクエストを受信
    if (!ReadMessageFromPipe(_pipeHandle, buff.data(), recvBufferLength, &recvBuffer, 5000, nullptr))
        return std::nullopt;

    buff.push_back('\0');
    return std::string(buff.data());
}

auto PipeServer::WriteMessage(HANDLE pipeHandle, std::string message) const noexcept -> bool
{
    auto WriteMessageToPipe = [](HANDLE hFile, LPCVOID lpBuffer, DWORD nNumberOfBytesToWrite, LPDWORD lpNumberOfBytesWritten, DWORD timeout, LPDWORD lpLastError)
        {
            auto ov = OVERLAPPED{ 0 };
            auto gle = DWORD{ 0 };

            auto result = WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, &ov);
            if (!result)
            {
                gle = GetLastError();
                if (gle == ERROR_IO_PENDING)
                {
                    for (DWORD passed = 0; passed < timeout; passed += WriteElapsed)
                    {
                        if (HasOverlappedIoCompleted(&ov))
                        {
                            gle = 0;
                            result = TRUE;
                            break;
                        }
                        Sleep(WriteElapsed);
                    }
                }
            }

            if (lpLastError)
            {
                *lpLastError = gle;
            }
            if (lpNumberOfBytesWritten)
            {
                *lpNumberOfBytesWritten = static_cast<DWORD>(ov.InternalHigh);
            }

            return result;
        };

    // レスポンスメッセージのバッファを計算してクライアントに送信
    auto messageLength = DWORD{ 0 };
    if (message.length() <= USHRT_MAX)
    {
        messageLength = static_cast<DWORD>(message.length());
    }
    else
    {
        messageLength = USHRT_MAX;
        message.resize(static_cast<size_t>(messageLength + 1));
        message.at(messageLength) = '\0';
    }

    auto bufSize = DWORD{ messageLength };
    auto bufH = static_cast<BYTE>(bufSize / 256);
    auto bufL = static_cast<BYTE>(bufSize % 256);
    auto bytesWritten = DWORD{ 0 };

    if (!WriteMessageToPipe(pipeHandle, &bufH, sizeof(bufH), &bytesWritten, 1000, nullptr))
        return false;

    if (!WriteMessageToPipe(pipeHandle, &bufL, sizeof(bufL), &bytesWritten, 1000, nullptr))
        return false;

    // レスポンスをクライアントに送信
    if (bufSize != 0)
        return WriteMessageToPipe(pipeHandle, message.c_str(), bufSize, &bytesWritten, 5000, nullptr);

    return false;
}