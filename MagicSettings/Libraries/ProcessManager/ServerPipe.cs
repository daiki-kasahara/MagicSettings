using System.IO.Pipes;
using System.Text.Json;
using ProcessManager.Contracts;
using ProcessManager.Internal;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ServerPipe
{
    private static readonly int PipeNumber = 1;
    private static readonly string CheckCmd = "Check";
    private static readonly string CloseCmd = "Close";
    private static readonly string TerminateCmd = "Terminate";

    public Action<RequestMessage>? OnAction;

    private readonly string _pipeName;
    private readonly MyProcesses _process;
    private Task? _pipeTask;
    private readonly IClientPipe _clientPipe;

    public ServerPipe(MyProcesses process)
    {
        _process = process;
        _pipeName = $"MagicSettings-{process}";
        _clientPipe = new ClientPipe();
    }

    internal ServerPipe(MyProcesses process, IClientPipe clientPipe)
    {
        _process = process;
        _pipeName = $"MagicSettings-{process}";
        _clientPipe = clientPipe;
    }

    /// <summary>
    /// サーバーパイプを開く
    /// </summary>
    /// <returns></returns>
    public bool OpenPipe()
    {
        if (_pipeTask is not null)
            return true;

        _pipeTask = Task.Run(PipeThread);

        return true;
    }

    /// <summary>
    /// サーバーパイプを閉じる
    /// </summary>
    /// <returns></returns>
    public bool ClosePipe()
    {
        if (_pipeTask is null)
            return true;

        return Task.Run(async () => await _clientPipe.SendRequestMessageAsync(_process, new RequestMessage(CloseCmd))).Result;
    }

    private async Task PipeThread()
    {
        while (true)
        {
            try
            {
                using var serverPipe = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, PipeNumber);
                await serverPipe.WaitForConnectionAsync();

                var stream = new StreamString(serverPipe);
                var readString = stream.ReadString();

                try
                {
                    var rowRequest = JsonSerializer.Deserialize<RowRequestMessage>(readString);

                    if (rowRequest is null || OnAction is null)
                        continue;

                    var request = new RequestMessage(rowRequest.Cmd, rowRequest.Args);

                    // Closeコマンドを受信したらパイプスレッドを終了する
                    if (request.Cmd == CloseCmd)
                        return;

                    // Checkコマンドを受信したら応答だけする
                    if (request.Cmd == CheckCmd)
                    {
                        stream.WriteString(new ResponseMessage() { ReturnCode = 0 }.Serialize());
                        continue;
                    }

                    // Terminateコマンドを受信したら応答だけして、終了処理はコールバック関数で処理する
                    if (request.Cmd == TerminateCmd)
                    {
                        stream.WriteString(new ResponseMessage() { ReturnCode = 0 }.Serialize());
                    }

                    OnAction(request);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            catch (IOException ofex)
            {
                // クライアントが切断
                Console.WriteLine(ofex.Message);
            }
        }
    }
}
