using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace MagicSettings.Views;

public sealed partial class KeyBindingPage : Page
{
    private readonly KeyBindingPageViewModel _viewModel;

    public KeyBindingPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<KeyBindingPageViewModel>();
    }

    private async void KeyBindingToggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledKeyBindingAsync(toggleSwitch.IsOn);
    }
}
