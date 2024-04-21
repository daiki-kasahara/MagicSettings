using System.IO.Pipes;
using System.Text.Json;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ServerPipe(MyProcesses process)
{
    private static readonly int PipeNumber = 1;
    private static readonly string CheckCmd = "Check";
    private static readonly string CloseCmd = "Close";
    private static readonly string TerminateCmd = "Terminate";

    private readonly string _pipeName = $"MagicSettings-{process}";
    private readonly MyProcesses _process = process;
    private Task? _pipeTask;
    private readonly ClientPipe _clientPipe = new();

    public Action<RequestMessage>? OnAction;

    public bool OpenPipe()
    {
        if (_pipeTask is not null)
            return true;

        _pipeTask = Task.Run(PipeThread);

        return true;
    }

    public async Task<bool> ClosePipe()
    {
        if (_pipeTask is null)
            return true;

        return await _clientPipe.SendRequestMessageAsync(_process, new RequestMessage(CloseCmd));
    }

    private async Task PipeThread()
    {
        NamedPipeServerStream? pipeServer = null;

        while (true)
        {
            try
            {
                // 同じパイプに対しての接続は1件まで
                using (pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, PipeNumber))
                {

                    await pipeServer.WaitForConnectionAsync();

                    var stream = new StreamString(pipeServer);

                    var readString = stream.ReadString();

                    try
                    {
                        var rowRequest = JsonSerializer.Deserialize<RowRequestMessage>(readString);

                        if (rowRequest is null || OnAction is null)
                            continue;

                        var request = new RequestMessage(rowRequest.Cmd, rowRequest.Args);

                        if (request.Cmd == CloseCmd)
                            return;

                        if (request.Cmd == CheckCmd)
                        {
                            stream.WriteString(JsonSerializer.Serialize(new ResponseMessage() { ReturnCode = 0 }));
                            continue;
                        }

                        if (request.Cmd == TerminateCmd)
                        {
                            stream.WriteString(JsonSerializer.Serialize(new ResponseMessage() { ReturnCode = 0 }));
                        }

                        OnAction(request);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (IOException ofex)
            {
                // クライアントが切断
                Console.WriteLine("受信：クライアント側が切断しました");
                Console.WriteLine(ofex.Message);
            }
            finally
            {
                Console.WriteLine("受信：パイプ終了");
            }
        }
    }
}
