using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MagicSettings.Models;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.Views;

public sealed partial class KeyboardPage : Page
{
    public ICommand AddCommand => new RelayCommand(AddAsync);

    public ICommand UpdateCommand => new RelayCommand(Update);

    private readonly KeyboardPageViewModel _viewModel;

    public KeyboardPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<KeyboardPageViewModel>();
    }

    private async void AddAsync()
    {
        if (KeyBindDialog.DataContext is not KeyBindAction newAction)
            return;

        await _viewModel.AddNewAction(newAction);
        KeyBindDialog.Hide();
    }

    private async void Update()
    {
        if (KeyBindDialog.DataContext is not KeyBindAction newAction)
            return;

        if (KeyBindDialog.Tag is not KeyBindAction oldAction)
            return;

        await _viewModel.UpdateAction(oldAction, newAction);
        KeyBindDialog.Hide();
    }

    private async void PageLoadedAsync(object _, RoutedEventArgs __) => await _viewModel.InitializeAsync();

    private async void KeyBindingToggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch)
            return;

        await _viewModel.SetEnabledKeyBindingAsync(toggleSwitch.IsOn);
    }

    private async void NewKeyBindButton_Click(object sender, RoutedEventArgs e)
    {
        var resourceLoader = new ResourceLoader();
        KeyBindDialog.Title = resourceLoader.GetString("KeyBindDialog_Add_Title");
        KeyBindDialog.DataContext = new KeyBindAction();
        KeyBindDialog.Tag = string.Empty;

        KeyBindDialog.PrimaryButtonText = resourceLoader.GetString("KeyBindDialog_Add");
        KeyBindDialog.PrimaryButtonCommand = AddCommand;
        await KeyBindDialog.ShowAsync();
    }

    private void KeyBindDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {

    }
}
