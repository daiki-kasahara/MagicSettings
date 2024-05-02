using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;

namespace MagicSettings.Services;

/// <summary>
/// テーマを設定するサービス
/// </summary>
/// <param name="repository"></param>
internal class ThemeService(IThemeRepository repository) : IThemeService
{
    /// <summary>
    /// 現在のテーマを取得する
    /// </summary>
    /// <returns></returns>
    public async Task<AppTheme> GetCurrentThemeAsync() => await repository.GetAsync();

    /// <summary>
    /// テーマを設定する
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    public async Task SetCurrentThemeAsync(AppTheme theme) => await repository.SaveAsync(theme);
}
