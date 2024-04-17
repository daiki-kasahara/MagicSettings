using System.Threading.Tasks;
using MagicSettings.Domains;

namespace MagicSettings.Contracts.Services;

internal interface IThemeService
{
    public Task<AppTheme> GetCurrentThemeAsync();

    public Task SetCurrentThemeAsync(AppTheme theme);
}
