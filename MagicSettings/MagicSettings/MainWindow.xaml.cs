using Microsoft.UI.Xaml;

namespace MagicSettings;

/// <summary>
/// ���C���E�B���h�E
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        myButton.Content = "Clicked";
    }
}
