using MagicSettings.Repositories.Models;

namespace MagicSettings.Repositories.Contracts;

/// <summary>
/// アセンブリ情報を取得するリポジトリ
/// </summary>
public interface IAssemblyInfoRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public Task<About> GetAsync();
}
