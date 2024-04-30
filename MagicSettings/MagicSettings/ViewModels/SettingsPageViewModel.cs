using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.ViewModels;

internal partial class SettingsPageViewModel(IThemeService themeService,
    IAssemblyInfoRepository assemblyInfoRepository, IOSSRepository ossRepository) : ObservableObject
{
    [ObservableProperty]
    private AppTheme _theme;

    [ObservableProperty]
    private About? _about;

    [ObservableProperty]
    private List<OSSProperty>? _oss;

    private readonly IThemeService _themeService = themeService;
    private readonly IAssemblyInfoRepository _assemblyInfoRepository = assemblyInfoRepository;
    private readonly IOSSRepository _ossRepository = ossRepository;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync()
    {
        Theme = await _themeService.GetCurrentThemeAsync();
        About = await _assemblyInfoRepository.GetAsync();
        Oss = (await _ossRepository.GetAsync())?.OSS;
    }

    /// <summary>
    /// テーマを設定する
    /// </summary>
    /// <param name="theme">設定するテーマ</param>
    /// <returns></returns>
    public async Task SetCurrentThemeAsync(AppTheme theme)
    {
        Theme = theme;
        await _themeService.SetCurrentThemeAsync(theme);
    }
}
