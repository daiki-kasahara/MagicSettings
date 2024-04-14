namespace ProcessManager.Contracts;

public interface IProcessLauncher
{
    public Task<bool> LaunchAsync(MyProcesses process);
}
