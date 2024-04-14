using MagicSettings.Domains;

namespace MagicSettings.Repositories.Contracts;

public interface IScreenRepository
{
    public Task<BlueLightBlocking> GetAsync();

    public Task SaveAsync(BlueLightBlocking value);
}
