using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using ProcessManager;
using ProcessManager.Contracts;

namespace MagicSettings.Services;

internal class KeyBindingService(IProcessController controller) : IKeyBindingService
{
    private readonly IProcessController _controller = controller;

    public async Task<bool> SetEnabledKeyBindingAsync(bool isEnabled)
    {
        // プロセス起動
        if (isEnabled)
        {
            if (!await _controller.LaunchAsync(MyProcesses.KeyBindingListener))
                return false;
        }
        else
        {
            await _controller.TerminateAsync(MyProcesses.KeyBindingListener);
        }
        return true;
    }
}
