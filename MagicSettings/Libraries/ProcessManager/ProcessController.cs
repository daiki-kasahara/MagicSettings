using System.Diagnostics;
using ProcessManager.Contracts;
using ProcessManager.Internal;
using ProcessManager.PipeMessage;

namespace ProcessManager;

/// <summary>
/// プロセスの起動や終了をするプロセス管理クラス
/// </summary>
public class ProcessController : IProcessController
{
    private static readonly string DirPath = AppContext.BaseDirectory;

    private readonly IClientPipe _pipe;

    public ProcessController()
    {
        _pipe = new ClientPipe();
    }

    internal ProcessController(IClientPipe pipe)
    {
        _pipe = pipe;
    }

    /// <summary>
    /// 起動中のプロセスが存在するか
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public bool IsExistsProcess(MyProcesses process) => Process.GetProcessesByName($"MagicSettings.{process}").Length > 0;

    /// <summary>
    /// プロセスを起動する
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<bool> LaunchAsync(MyProcesses process)
    {
        try
        {
            using var proc = new Process();
            proc.StartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(DirPath, $"MagicSettings.{process}.exe"),
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = DirPath
            };
            proc.Start();

            return await _pipe.CheckExistedMessageAsync(process);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// プロセスにメッセージを送信する
    /// </summary>
    /// <param name="process"></param>
    /// <param name="requestMessage"></param>
    /// <returns></returns>
    public async Task<bool> SendMessageAsync(MyProcesses process, RequestMessage requestMessage) => await _pipe.SendRequestMessageAsync(process, requestMessage);

    /// <summary>
    /// プロセスを終了させる
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public async Task<bool> TerminateAsync(MyProcesses process) => await _pipe.SendTerminateMessageAsync(process);
}
