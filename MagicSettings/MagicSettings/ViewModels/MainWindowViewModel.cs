using System.Collections.Generic;
using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.ViewModels;

internal class MainWindowViewModel
{
    public List<MenuItem> NavigationMenuItems { get; }

    private readonly IThemeService _themeService;

    public MainWindowViewModel(IThemeService themeService,
                      IKeyboardService keyboardService, IScreenService screenService)
    {
        _themeService = themeService;

        var loader = new ResourceLoader();

        // 設定ファイルの更新
        keyboardService.UpdateSettingAsync();
        screenService.UpdateSettingAsync();

        // メニューの構成
        NavigationMenuItems =
        [
            new(loader.GetString($"MainMenu_{Tag.Keyboard}"), "\xE765", Tag.Keyboard),
            new(loader.GetString($"MainMenu_{Tag.Screen}"), "\xE770", Tag.Screen),
        ];
    }

    public async Task<AppTheme> GetCurrentThemeAsync() => await _themeService.GetCurrentThemeAsync();
}
