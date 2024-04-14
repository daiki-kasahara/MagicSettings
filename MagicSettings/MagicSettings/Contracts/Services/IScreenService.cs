using System.Threading.Tasks;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;

namespace MagicSettings.Contracts.Services;

internal interface IScreenService
{
    public Task<bool> SetEnabledBlueLightBlockingAsync(bool value);

    public Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value);

    public Task<ScreenSettings> GetScreenSettingsAsync();
}
