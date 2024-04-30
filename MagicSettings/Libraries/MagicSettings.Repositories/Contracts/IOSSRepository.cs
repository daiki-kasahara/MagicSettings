using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories.Contracts;

public interface IOSSRepository
{
    public Task<OSSFile?> GetAsync();
}
