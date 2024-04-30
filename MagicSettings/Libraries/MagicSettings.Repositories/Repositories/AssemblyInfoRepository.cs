using System.Diagnostics;
using System.Reflection;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models;

namespace MagicSettings.Repositories.Repositories;

/// <summary>
/// アセンブリ情報を取得するリポジトリ
/// </summary>
public class AssemblyInfoRepository : IAssemblyInfoRepository
{
    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public async Task<About> GetAsync()
    {
        return await Task.Run(() =>
        {
            // ファイルアセンブリから情報取得する
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return new About(fileVersionInfo.ProductName!, fileVersionInfo.FileVersion!, fileVersionInfo.LegalCopyright!);
        });
    }
}
