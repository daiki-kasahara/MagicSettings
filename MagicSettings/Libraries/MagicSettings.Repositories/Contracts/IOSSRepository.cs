using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

/// <summary>
/// OSSのリポジトリ
/// </summary>
public interface IOSSRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public Task<OSSFile?> GetAsync();
}
