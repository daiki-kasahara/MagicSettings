using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;

namespace MagicSettings.ViewModels;

internal partial class SettingsPageViewModel(IThemeService themeService) : ObservableObject
{
    [ObservableProperty]
    private AppTheme _theme;

    public async Task InitializeAsync()
    {
        Theme = await themeService.GetCurrentThemeAsync();
    }

    public async Task SetCurrentThemeAsync(AppTheme theme)
    {
        Theme = theme;
        await themeService.SetCurrentThemeAsync(theme);
    }

}
