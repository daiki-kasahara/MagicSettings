using System.Diagnostics;
using System.Reflection;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models;

namespace MagicSettings.Repositories.Repositories;

public class AssemblyInfoRepository : IAssemblyInfoRepository
{
    public async Task<About> GetAsync()
    {
        return await Task.Run(() =>
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return new About(fileVersionInfo.ProductName!, fileVersionInfo.FileVersion!, fileVersionInfo.LegalCopyright!);
        });
    }
}
