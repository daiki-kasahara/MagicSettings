using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using ProcessManager;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace MagicSettings.Services;

internal class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;
    private readonly IProcessController _controller;

    public ScreenService(IScreenRepository repository, IProcessController controller)
    {
        _screenRepository = repository;
        _controller = controller;
    }

    public async Task<ScreenSettings> GetScreenSettingsAsync() => await _screenRepository.GetAsync();

    public async Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value)
    {
        await _screenRepository.SaveAsync(value);
        return await _controller.SendMessageAsync(MyProcesses.ScreenController, new RequestMessage("Update"));
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool value)
    {
        await _screenRepository.SaveAsync(value);

        // プロセス起動
        if (value)
        {
            if (!await _controller.LaunchAsync(MyProcesses.ScreenController))
                return false;
        }
        else
        {
            await _controller.TerminateAsync(MyProcesses.ScreenController);
        }
        return true;
    }
}
