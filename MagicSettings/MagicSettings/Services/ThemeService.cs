using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;

namespace MagicSettings.Services;

internal class ThemeService(IThemeRepository repository) : IThemeService
{
    public async Task<AppTheme> GetCurrentThemeAsync() => await repository.GetAsync();

    public async Task SetCurrentThemeAsync(AppTheme theme) => await repository.SaveAsync(theme);
}
