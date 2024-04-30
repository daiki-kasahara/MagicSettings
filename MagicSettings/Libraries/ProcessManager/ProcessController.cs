using System.Diagnostics;
using ProcessManager.Contracts;
using ProcessManager.Internal;
using ProcessManager.PipeMessage;

namespace ProcessManager;

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

    public bool IsExistsProcess(MyProcesses process) => Process.GetProcessesByName($"MagicSettings.{process}").Length > 0;

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

    public async Task<bool> SendMessageAsync(MyProcesses process, RequestMessage requestMessage) => await _pipe.SendRequestMessageAsync(process, requestMessage);

    public async Task<bool> TerminateAsync(MyProcesses process) => await _pipe.SendTerminateMessageAsync(process);
}
