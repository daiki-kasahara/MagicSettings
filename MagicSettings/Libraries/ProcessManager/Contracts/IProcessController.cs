using ProcessManager.PipeMessage;

namespace ProcessManager.Contracts;

public interface IProcessController
{
    public Task<bool> LaunchAsync(MyProcesses process);

    public Task<bool> SendMessageAsync(MyProcesses process, RequestMessage requestMessage);

    public Task<bool> TerminateAsync(MyProcesses process);
}
