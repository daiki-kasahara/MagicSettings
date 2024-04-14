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
    private ProcedureThrottle _procedureThrottle = new();

    public ScreenPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<ScreenPageViewModel>();
    }

    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    private async void BlueLightBlockingToggled(object sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledBlueLightBlockingAsync(toggleSwitch.IsOn);
    }

    private void BlueLightBlockingValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is not Slider slider)
            return;

        _procedureThrottle.PostAsyncAction(async () => await _viewModel.SetBlueLightBlockingAsync((int)slider.Value), TimeSpan.FromMilliseconds(500));
    }
}
