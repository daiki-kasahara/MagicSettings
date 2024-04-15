using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using ProcessManager;
using ProcessManager.Contracts;

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
        // Todo: プロセス通信
        await _screenRepository.SaveAsync(value);
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
            await _pipe.SendTerminateMessage(MyProcesses.ScreenController);
        }

        await _screenRepository.SaveAsync(value);
        return true;
    }
}
