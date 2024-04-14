using System.Threading.Tasks;
using MagicSettings.Contracts.Repositories;
using MagicSettings.Contracts.Services;
using MagicSettings.Models;

namespace MagicSettings.Services;

internal class ThemeService(IThemeRepository repository) : IThemeService
{
    public async Task<AppTheme> GetCurrentThemeAsync() => await repository.GetAsync();

    public async Task SetCurrentThemeAsync(AppTheme theme) => await repository.SaveAsync(theme);
}
