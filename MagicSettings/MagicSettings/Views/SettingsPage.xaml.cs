using MagicSettings.Helper;
using MagicSettings.Models;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MagicSettings.Views;

public sealed partial class SettingsPage : Page
{
    private readonly SettingsPageViewModel _viewModel;

    public SettingsPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<SettingsPageViewModel>();
    }

    private async void PageLoadedAsync(object sender, RoutedEventArgs e) => await _viewModel.InitializeAsync();

    private async void ThemeButtonCheckedAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is not RadioButton radioButton)
            return;

        var theme = (AppTheme)radioButton.Tag;

        if (_viewModel.Theme == theme)
            return;

        var requestedTheme = theme switch
        {
            AppTheme.Dark => ElementTheme.Dark,
            AppTheme.Light => ElementTheme.Light,
            AppTheme.System => ElementTheme.Default,
            _ => ElementTheme.Default,
        };

        WindowHelper.RootTheme = requestedTheme;
        await _viewModel.SetCurrentThemeAsync(theme);
    }

    #region Converter

    internal bool CurrentThemeToCheckedConverter(AppTheme currentTheme, AppTheme theme) => currentTheme == theme;

    #endregion
}
