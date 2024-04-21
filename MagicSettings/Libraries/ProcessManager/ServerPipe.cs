using System.IO.Pipes;
using System.Text.Json;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ServerPipe(MyProcesses process)
{
    private static readonly int PipeNumber = 1;

    private readonly string _pipeName = $"MagicSettings-{process}";
    private readonly MyProcesses _process = process;
    private Task? _pipeTask;
    private readonly ClientPipe _clientPipe = new();

    public Action<RequestMessage>? OnAction;

    public bool OpenPipe()
    {
        if (_pipeTask is not null)
            return true;

        _pipeTask = PipeThread();

        return true;
    }

    public async Task<bool> ClosePipe()
    {
        if (_pipeTask is null)
            return true;

        return await _clientPipe.SendTerminateMessageAsync(_process);
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

                    var reader = new StreamString(pipeServer);

                    var readString = reader.ReadString();

                    try
                    {
                        var request = JsonSerializer.Deserialize<RequestMessage>(readString);

                        if (request is null || OnAction is null)
                            continue;

                        if (request.Cmd is "Terminate")
                            return;

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
