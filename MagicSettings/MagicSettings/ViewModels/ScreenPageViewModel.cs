using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;

namespace MagicSettings.ViewModels;

internal partial class ScreenPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabledBlueLightBlocking;

    [ObservableProperty]
    private int _reductionRate;

    [ObservableProperty]
    private bool _hasError = false;

    [ObservableProperty]
    private bool _isBusy = false;

    private readonly IScreenService _screenService;

    public ScreenPageViewModel(IScreenService service)
    {
        _screenService = service;
    }

    public async Task InitializeAsync()
    {
        IsBusy = false;

        var settings = await _screenService.GetScreenSettingsAsync();

        IsEnabledBlueLightBlocking = settings.IsEnabledBlueLightBlocking;
        ReductionRate = (int)settings.BlueLightBlocking;

        IsBusy = true;
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledBlueLightBlocking == value)
            return true;

        IsBusy = false;

        if (!await _screenService.SetEnabledBlueLightBlockingAsync(value))
        {
            HasError = true;
            IsBusy = true;
            OnPropertyChanged(nameof(IsEnabledBlueLightBlocking));
            return false;
        }

        HasError = false;
        IsBusy = true;
        IsEnabledBlueLightBlocking = value;
        return true;
    }

    public async Task<bool> SetBlueLightBlockingAsync(int value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (ReductionRate == value)
            return true;

        if (!Enum.IsDefined(typeof(BlueLightBlocking), value))
        {
            HasError = true;
            OnPropertyChanged(nameof(ReductionRate));
            return false;
        }

        IsBusy = false;

        if (!await _screenService.SetBlueLightBlockingAsync((BlueLightBlocking)value))
        {
            HasError = true;
            IsBusy = true;
            OnPropertyChanged(nameof(ReductionRate));
            return false;
        }

        HasError = false;
        IsBusy = true;
        ReductionRate = value;
        return true;
    }
}
