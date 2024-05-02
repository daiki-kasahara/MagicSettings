using System;
using MagicSettings.Helper;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace MagicSettings.Views;

public sealed partial class ScreenPage : Page
{
    private readonly ScreenPageViewModel _viewModel;
    private readonly ProcedureThrottle _procedureThrottle = new();

    public ScreenPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<ScreenPageViewModel>();
    }

    /// <summary>
    /// ページを表示した時に実行する処理
    /// </summary>
    /// <param name="_"></param>
    /// <param name="__"></param>
    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    /// <summary>
    /// ブルーライトカットの有効無効設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private async void BlueLightBlockingToggled(object sender, RoutedEventArgs _)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledBlueLightBlockingAsync(toggleSwitch.IsOn);
    }

    /// <summary>
    /// ブルーライトの軽減率の設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="_"></param>
    private void BlueLightBlockingValueChanged(object sender, RangeBaseValueChangedEventArgs _)
    {
        if (sender is not Slider slider)
            return;

        // スロットルで処理をする
        _procedureThrottle.PostAsyncAction(async () => await _viewModel.SetBlueLightBlockingAsync((int)slider.Value), TimeSpan.FromMilliseconds(500));
    }

    #region Converter

    private bool MultiBoolConverter(bool b1, bool b2) => b1 && b2;

    #endregion
}
