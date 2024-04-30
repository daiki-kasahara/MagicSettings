using MagicSettings.Domains;

namespace MagicSettings.Repositories.Contracts;

/// <summary>
/// テーマのリポジトリ
/// </summary>
public interface IThemeRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public Task<AppTheme> GetAsync();

    /// <summary>
    /// テーマを保存する
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    public Task SaveAsync(AppTheme theme);
}
