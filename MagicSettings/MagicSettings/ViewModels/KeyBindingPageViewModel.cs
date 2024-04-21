using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;

namespace MagicSettings.ViewModels;

internal partial class KeyBindingPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabledBlueLightBlocking;

    [ObservableProperty]
    private int _reductionRate;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _canExecute = false;

    private readonly IKeyBindingService _screenService;

    public KeyBindingPageViewModel(IKeyBindingService service)
    {
        _screenService = service;
    }

    public async Task InitializeAsync()
    {
        CanExecute = false;

        //var settings = await _screenService.GetScreenSettingsAsync();

        //IsEnabledBlueLightBlocking = settings.IsEnabledBlueLightBlocking;
        //ReductionRate = (int)settings.BlueLightBlocking;

        CanExecute = true;
    }

    public async Task<bool> SetEnabledKeyBindingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledBlueLightBlocking == value)
            return true;

        CanExecute = false;

        if (!await _screenService.SetEnabledKeyBindingAsync(value))
        {
            HasError = true;
            CanExecute = true;
            OnPropertyChanged(nameof(IsEnabledBlueLightBlocking));
            return false;
        }

        HasError = false;
        CanExecute = true;
        IsEnabledBlueLightBlocking = value;
        return true;
    }
}
