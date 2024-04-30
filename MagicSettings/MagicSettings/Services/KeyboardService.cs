using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models.SettingsFile;
using ProcessManager;
using ProcessManager.Contracts;

namespace MagicSettings.Services;

/// <summary>
/// キーボード設定をするサービス
/// </summary>
/// <param name="keyboardRepository"></param>
/// <param name="controller"></param>
internal class KeyboardService(IKeyboardBindingRepository keyboardRepository, IProcessController controller) : IKeyboardService
{
    private readonly IKeyboardBindingRepository _keyboardRepository = keyboardRepository;
    private readonly IProcessController _controller = controller;

    /// <summary>
    /// キーバインディングの設定を取得する
    /// </summary>
    /// <returns></returns>
    public async Task<KeyboardBindingSettings> GetKeyBindingSettingsAsync() => await _keyboardRepository.GetAsync();

    /// <summary>
    /// キーバインディングを設定する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<bool> SetKeyBindingActionAsync(int key, KeyboardAction action)
    {
        await _keyboardRepository.SaveAsync(key, action);
        return true;
    }

    /// <summary>
    /// キーバインディングの有効無効を設定する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public async Task<bool> SetEnabledKeyBindingAsync(bool isEnabled)
    {
        await _keyboardRepository.SaveAsync(isEnabled);
        return isEnabled ? await _controller.LaunchAsync(MyProcesses.KeyBindingListener) :
            await _controller.TerminateAsync(MyProcesses.KeyBindingListener);
    }

    /// <summary>
    /// 登録されているキーバインディングの削除する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<bool> DeleteKeyBindingActionAsync(int key)
    {
        await _keyboardRepository.DeleteAsync(key);
        return true;
    }

    /// <summary>
    /// 設定の更新する
    /// </summary>
    /// <returns></returns>
    public async Task UpdateSettingAsync() => await _keyboardRepository.SaveAsync(_controller.IsExistsProcess(MyProcesses.KeyBindingListener));
}
