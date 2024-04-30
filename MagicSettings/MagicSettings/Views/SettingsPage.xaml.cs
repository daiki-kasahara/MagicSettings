using MagicSettings.Domains;
using MagicSettings.Helper;
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

    /// <summary>
    /// �y�[�W��\���������Ɏ��s���鏈��
    /// </summary>
    /// <param name="_"></param>
    /// <param name="__"></param>
    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    /// <summary>
    /// �e�[�}�ݒ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void ThemeButtonCheckedAsync(object sender, RoutedEventArgs _)
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

    private bool CurrentThemeToCheckedConverter(AppTheme currentTheme, AppTheme theme) => currentTheme == theme;

    #endregion
}
