using System.IO.Pipes;
using System.Security.Principal;
using System.Text.Json;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager.Internal;

/// <summary>
/// クライアントパイプ
/// </summary>
internal class ClientPipe : IClientPipe
{
    /// <summary>
    /// プロセスが応答するかどうかを確認する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<bool> CheckExistedMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Check"), 10000);

    /// <summary>
    /// プロセスに終了メッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<bool> SendTerminateMessageAsync(MyProcesses process) =>
        await SendMessageToServerAsync(process, new("Terminate"), 5000);

    /// <summary>
    /// プロセスにリクエストメッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
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
