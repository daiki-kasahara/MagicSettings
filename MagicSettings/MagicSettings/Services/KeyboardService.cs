using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models.SettingsFile;
using ProcessManager;
using ProcessManager.Contracts;

namespace MagicSettings.Services;

internal class KeyboardService(IKeyboardBindingRepository keyboardRepository, IProcessController controller) : IKeyboardService
{
    private readonly IKeyboardBindingRepository _keyboardRepository = keyboardRepository;
    private readonly IProcessController _controller = controller;

    public async Task<KeyboardBindingSettings> GetKeyBindingSettingsAsync() => await _keyboardRepository.GetAsync();

    public async Task<bool> SetKeyBindingActionAsync(int key, KeyboardAction action)
    {
        await _keyboardRepository.SaveAsync(key, action);
        return true;
    }

    public async Task<bool> SetEnabledKeyBindingAsync(bool isEnabled)
    {
        await _keyboardRepository.SaveAsync(isEnabled);
        return isEnabled ? await _controller.LaunchAsync(MyProcesses.KeyBindingListener) :
            await _controller.TerminateAsync(MyProcesses.KeyBindingListener);
    }

    public async Task<bool> DeleteKeyBindingActionAsync(int key)
    {
        await _keyboardRepository.DeleteAsync(key);
        return true;
    }

    public async Task UpdateSettingAsync() => await _keyboardRepository.SaveAsync(_controller.IsExistsProcess(MyProcesses.KeyBindingListener));
}
