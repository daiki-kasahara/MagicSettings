using System.Threading.Tasks;
using MagicSettings.Models;

namespace MagicSettings.Contracts.Repositories;

internal interface IThemeRepository
{
    public Task<AppTheme> GetAsync();

    public Task SaveAsync(AppTheme theme);
}
