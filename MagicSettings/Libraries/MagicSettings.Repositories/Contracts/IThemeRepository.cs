using MagicSettings.Domains;

namespace MagicSettings.Repositories.Contracts;

public interface IThemeRepository
{
    public Task<AppTheme> GetAsync();

    public Task SaveAsync(AppTheme theme);
}
