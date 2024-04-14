using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;

namespace MagicSettings.Services;

internal class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;

    public ScreenService(IScreenRepository repository)
    {
        _screenRepository = repository;
    }

    public async Task<ScreenSettings> GetScreenSettingsAsync() => await _screenRepository.GetAsync();

    public async Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value)
    {
        // Todo: ウィンドウメッセージ送信
        await _screenRepository.SaveAsync(value);
        return true;
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool value)
    {
        // プロセス起動
        await _screenRepository.SaveAsync(value);
        return true;
    }
}
