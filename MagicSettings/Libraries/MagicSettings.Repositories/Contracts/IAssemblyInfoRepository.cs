using MagicSettings.Repositories.Models;

namespace MagicSettings.Repositories.Contracts;

public interface IAssemblyInfoRepository
{
    public Task<About> GetAsync();
}
