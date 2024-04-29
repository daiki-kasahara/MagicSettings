using ProcessManager.PipeMessage;

namespace ProcessManager.Contracts;

internal interface IClientPipe
{
    public Task<bool> CheckExistedMessageAsync(MyProcesses process);
    public Task<bool> SendTerminateMessageAsync(MyProcesses process);
    public Task<bool> SendRequestMessageAsync(MyProcesses process, RequestMessage requestMessage);
}
