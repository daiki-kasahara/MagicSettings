using System.Diagnostics;
using ProcessManager.Contracts;

namespace ProcessManager;

public class ProcessLauncher : IProcessLauncher
{
    private static readonly string DirPath = AppContext.BaseDirectory;

    public async Task<bool> LaunchAsync(MyProcesses process)
    {
        try
        {
            using var proc = new Process();
            proc.StartInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(DirPath, $"{process}.exe"),
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = DirPath
            };
            proc.Start();

            // Todo: 接続確認できるまでwaitする

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> TerminateAsync(MyProcesses processes)
    {
        // Todo: パイプ通信で終了処理をおくる
        return true;
    }
}
