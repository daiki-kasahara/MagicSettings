using System.Threading.Tasks;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;

namespace MagicSettings.Contracts.Services;

/// <summary>
/// スクリーンの設定をするサービス
/// </summary>
internal interface IScreenService
{
    /// <summary>
    /// スクリーンの設定情報を取得する
    /// </summary>
    /// <returns></returns>
    public Task<ScreenSettings> GetScreenSettingsAsync();

    /// <summary>
    /// ブルーライトカットの有効無効を設定する
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task<bool> SetEnabledBlueLightBlockingAsync(bool value);

    /// <summary>
    /// 軽減率の設定をする
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value);

    /// <summary>
    /// 設定を更新する
    /// </summary>
    /// <returns></returns>
    public Task UpdateSettingAsync();
}
