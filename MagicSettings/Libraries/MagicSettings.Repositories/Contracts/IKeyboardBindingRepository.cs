using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

/// <summary>
/// キーバインディングのリポジトリ
/// </summary>
public interface IKeyboardBindingRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public Task<KeyboardBindingSettings> GetAsync();

    /// <summary>
    /// キーバインディングを保存する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task SaveAsync(int key, KeyboardAction action);

    /// <summary>
    /// キーバインディングの有効無効を保存する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public Task SaveAsync(bool isEnabled);

    /// <summary>
    /// キーバインディングを削除する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task DeleteAsync(int key);
}
