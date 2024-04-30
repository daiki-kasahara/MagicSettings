using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

/// <summary>
/// スクリーンのリポジトリ
/// </summary>
public interface IScreenRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public Task<ScreenSettings> GetAsync();

    /// <summary>
    /// 軽減率を保存する
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task SaveAsync(BlueLightBlocking value);

    /// <summary>
    /// 有効無効を保存する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public Task SaveAsync(bool isEnabled);
}
