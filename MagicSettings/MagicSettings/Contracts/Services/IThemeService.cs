using System.Threading.Tasks;
using MagicSettings.Domains;

namespace MagicSettings.Contracts.Services;

/// <summary>
/// テーマを設定するサービス
/// </summary>
internal interface IThemeService
{
    /// <summary>
    /// 現在のテーマを取得する
    /// </summary>
    /// <returns></returns>
    public Task<AppTheme> GetCurrentThemeAsync();

    /// <summary>
    /// テーマを設定する
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    public Task SetCurrentThemeAsync(AppTheme theme);
}
