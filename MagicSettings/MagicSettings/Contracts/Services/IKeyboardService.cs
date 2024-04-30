using System.Threading.Tasks;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Contracts.Services;

/// <summary>
/// キーボードの設定をするサービス
/// </summary>
internal interface IKeyboardService
{
    /// <summary>
    /// キーバインディングの設定を取得する
    /// </summary>
    /// <returns></returns>
    public Task<KeyboardBindingSettings> GetKeyBindingSettingsAsync();

    /// <summary>
    /// キーバインディングの有効無効を設定する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public Task<bool> SetEnabledKeyBindingAsync(bool isEnabled);

    /// <summary>
    /// キーバインディングを設定する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task<bool> SetKeyBindingActionAsync(int key, KeyboardAction action);

    /// <summary>
    /// 登録されているキーバインディングの削除する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<bool> DeleteKeyBindingActionAsync(int key);

    /// <summary>
    /// 設定の更新する
    /// </summary>
    /// <returns></returns>
    public Task UpdateSettingAsync();
}
