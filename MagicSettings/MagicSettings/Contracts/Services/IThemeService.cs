using System.Threading.Tasks;
using MagicSettings.Models;

namespace MagicSettings.Contracts.Services;

internal interface IThemeService
{
    public Task<AppTheme> GetCurrentThemeAsync();

    public Task SetCurrentThemeAsync(AppTheme theme);
}
