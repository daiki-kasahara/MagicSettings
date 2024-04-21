using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using ProcessManager;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace MagicSettings.Services;

internal class ScreenService(IScreenRepository repository, IProcessController controller) : IScreenService
{
    private readonly IScreenRepository _screenRepository = repository;
    private readonly IProcessController _controller = controller;

    public async Task<ScreenSettings> GetScreenSettingsAsync() => await _screenRepository.GetAsync();

    public async Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value)
    {
        await _screenRepository.SaveAsync(value);
        return await _controller.SendMessageAsync(MyProcesses.ScreenController, new RequestMessage("Update"));
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool isEnabled)
    {
        await _screenRepository.SaveAsync(isEnabled);
        return isEnabled ? await _controller.LaunchAsync(MyProcesses.ScreenController) :
            await _controller.TerminateAsync(MyProcesses.ScreenController);
    }
}
