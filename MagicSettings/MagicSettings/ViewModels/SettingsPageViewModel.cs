using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models;

namespace MagicSettings.ViewModels;

internal partial class SettingsPageViewModel(IThemeService themeService, IAssemblyInfoRepository assemblyInfoRepository) : ObservableObject
{
    [ObservableProperty]
    private AppTheme _theme;

    [ObservableProperty]
    private About? _about;

    private readonly IThemeService _themeService = themeService;
    private readonly IAssemblyInfoRepository _assemblyInfoRepository = assemblyInfoRepository;

    public async Task InitializeAsync()
    {
        Theme = await _themeService.GetCurrentThemeAsync();
        About = await _assemblyInfoRepository.GetAsync();
    }

    public async Task SetCurrentThemeAsync(AppTheme theme)
    {
        Theme = theme;
        await _themeService.SetCurrentThemeAsync(theme);
    }

}
