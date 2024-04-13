using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Models;

namespace MagicSettings.ViewModels;

internal partial class SettingsPageViewModel(IThemeService themeService) : ObservableObject
{
    [ObservableProperty]
    private AppTheme _theme;

    public async Task InitializeAsync()
    {
        Theme = await themeService.GetCurrentThemeAsync();
    }
}
