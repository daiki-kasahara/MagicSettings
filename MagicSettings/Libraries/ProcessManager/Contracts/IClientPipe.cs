using ProcessManager.PipeMessage;

namespace ProcessManager.Contracts;

/// <summary>
/// クライアントパイプインタフェース
/// </summary>
internal interface IClientPipe
{
    /// <summary>
    /// プロセスが応答するかどうかを確認する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public Task<bool> CheckExistedMessageAsync(MyProcesses process);

    /// <summary>
    /// プロセスに終了メッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public Task<bool> SendTerminateMessageAsync(MyProcesses process);

    /// <summary>
    /// プロセスにリクエストメッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
    public Task<bool> SendRequestMessageAsync(MyProcesses process, RequestMessage requestMessage);
}
