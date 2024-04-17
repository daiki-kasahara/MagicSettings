using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

public interface IScreenRepository
{
    public Task<ScreenSettings> GetAsync();

    public Task SaveAsync(BlueLightBlocking value);

    public Task SaveAsync(bool isEnabled);
}
