using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

public interface IKeyboardBindingRepository
{
    public Task<KeyboardBindingSettings> GetAsync();

    public Task SaveAsync(int key, KeyboardAction action);

    public Task SaveAsync(bool isEnabled);
}
