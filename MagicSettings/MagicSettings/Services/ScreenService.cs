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
    private readonly IProcessLauncher _launcher;
    // Todo: DIする
    private readonly ClientPipe _pipe = new();

    public ScreenService(IScreenRepository repository, IProcessLauncher launcher)
    {
        _screenRepository = repository;
        _launcher = launcher;
    }

    public async Task<ScreenSettings> GetScreenSettingsAsync() => await _screenRepository.GetAsync();

    public async Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value)
    {
        await _screenRepository.SaveAsync(value);
        await _pipe.SendRequestMessageAsync(MyProcesses.ScreenController, new RequestMessage("Update"));
        return true;
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool value)
    {
        // プロセス起動
        if (value)
        {
            if (!await _launcher.LaunchAsync(MyProcesses.ScreenController))
                return false;
        }
        else
        {
            await _pipe.SendTerminateMessageAsync(MyProcesses.ScreenController);
        }

        await _screenRepository.SaveAsync(value);
        return true;
    }
}
