using System.IO.Pipes;
using System.Security.Principal;
using System.Text.Json;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager;

internal class ClientPipe
{
    public async Task<bool> CheckExistedMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Check"), 10000);

    public async Task<bool> SendTerminateMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Terminate"), 5000);

    public async Task<bool> SendRequestMessageAsync(MyProcesses process, RequestMessage requestMessage) =>
        await SendMessageToServerAsync(process, requestMessage, 5000);

    private async Task<bool> SendMessageToServerAsync(MyProcesses process, RequestMessage message, int timeout)
    {
        try
        {
            using var pipeClient = new NamedPipeClientStream(".", $"MagicSettings-{process}", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            await pipeClient.ConnectAsync(timeout);
            var ss = new StreamString(pipeClient);

            // 入力された文字列を送信する
            var write = ss.WriteString(message.Serialize());

            // 応答待ち
            var read = ss.ReadString();

            try
            {
                return JsonSerializer.Deserialize<ResponseMessage>(read)?.Result ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}
