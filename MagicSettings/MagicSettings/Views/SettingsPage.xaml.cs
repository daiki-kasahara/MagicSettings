using MagicSettings.Models;
using MagicSettings.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MagicSettings.Views;

public sealed partial class SettingsPage : Page
{
    private SettingsPageViewModel _viewModel;

    public SettingsPage()
    {
        this.InitializeComponent();
        _viewModel = App.Provider.GetRequiredService<SettingsPageViewModel>();
    }

    private void ThemeButton_Checked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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

        var childElement = FindChildElementInXamlRoot(radioButton.XamlRoot, "RootGrid");
        childElement.RequestedTheme = requestedTheme;
    }

    // XamlRootから子のFrameworkElementを取得するメソッド
    public FrameworkElement FindChildElementInXamlRoot(XamlRoot root, string childName)
    {
        // XamlRootからルート要素を取得
        var rootElement = root.Content as FrameworkElement;

        // VisualTreeHelperを使用して子要素を検索
        return FindChildElement(rootElement, childName);
    }

    // VisualTreeHelperを使用して子要素を検索する再帰メソッド
    private FrameworkElement FindChildElement(FrameworkElement parent, string childName)
    {
        if (parent == null)
            return null;

        if (parent.Name == childName)
            return parent;

        int childCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
            var result = FindChildElement(child, childName);
            if (result != null)
                return result;
        }

        return null;
    }

    internal bool CurrentThemeToCheckedConverter(AppTheme currentTheme, AppTheme theme) => currentTheme == theme;
}
