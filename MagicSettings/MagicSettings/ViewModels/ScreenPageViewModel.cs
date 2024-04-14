using CommunityToolkit.Mvvm.ComponentModel;

namespace MagicSettings.ViewModels;

internal partial class ScreenPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _IsEnabledBlueLightBlocking;

    [ObservableProperty]
    private int _reductionRate;

    public ScreenPageViewModel()
    {

    }
}
