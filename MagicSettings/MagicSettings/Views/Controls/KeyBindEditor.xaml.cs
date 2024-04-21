using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MagicSettings.Views.Controls
{
    public sealed partial class KeyBindEditor : UserControl
    {
        public KeyBindEditor()
        {
            this.InitializeComponent();
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
        }
    }
}
