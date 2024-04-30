using ProcessManager.PipeMessage;

namespace ProcessManager.Contracts;

/// <summary>
/// プロセスの起動や終了をするインタフェース
/// </summary>
public interface IProcessController
{
    /// <summary>
    /// 起動中のプロセスが存在するか
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public bool IsExistsProcess(MyProcesses process);

    /// <summary>
    /// プロセスを起動する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public Task<bool> LaunchAsync(MyProcesses process);

    /// <summary>
    /// プロセスにメッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
    public Task<bool> SendMessageAsync(MyProcesses process, RequestMessage requestMessage);

    /// <summary>
    /// プロセスを終了させる
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public Task<bool> TerminateAsync(MyProcesses process);
}
