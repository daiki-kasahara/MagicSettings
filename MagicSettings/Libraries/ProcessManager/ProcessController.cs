using System.Diagnostics;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ProcessController : IProcessController
{
    private static readonly string DirPath = AppContext.BaseDirectory;

    private readonly ClientPipe _pipe = new();

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
