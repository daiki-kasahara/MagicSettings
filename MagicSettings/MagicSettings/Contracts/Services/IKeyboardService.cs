using System.Threading.Tasks;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Contracts.Services;

internal interface IKeyboardService
{
    public Task<KeyboardBindingSettings> GetKeyBindingSettingsAsync();

    public Task<bool> SetEnabledKeyBindingAsync(bool isEnabled);

    public Task<bool> SetKeyBindingActionAsync(int key, KeyboardAction action);

    public Task<bool> DeleteKeyBindingActionAsync(int key);
}
