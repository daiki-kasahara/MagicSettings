﻿using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;

namespace MagicSettings.ViewModels;

internal partial class ScreenPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _IsEnabledBlueLightBlocking;

    [ObservableProperty]
    private int _reductionRate;

    private readonly IScreenService _screenService;

    public ScreenPageViewModel(IScreenService service)
    {
        _screenService = service;
    }

    public async Task InitializeAsync()
    {
        var settings = await _screenService.GetScreenSettingsAsync();

        IsEnabledBlueLightBlocking = settings.IsEnabledBlueLightBlocking;
        ReductionRate = (int)settings.BlueLightBlocking;
    }

    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (IsEnabledBlueLightBlocking == value)
            return true;

        if (!await _screenService.SetEnabledBlueLightBlockingAsync(value))
            return false;

        IsEnabledBlueLightBlocking = value;
        return true;
    }

    public async Task<bool> SetBlueLightBlockingAsync(int value)
    {
        // 設定する値が現在と同じ場合何もせず成功を返す
        if (ReductionRate == value)
            return true;

        if (!Enum.IsDefined(typeof(BlueLightBlocking), value))
            return false;

        if (!await _screenService.SetBlueLightBlockingAsync((BlueLightBlocking)value))
            return false;

        ReductionRate = value;
        return true;
    }
}