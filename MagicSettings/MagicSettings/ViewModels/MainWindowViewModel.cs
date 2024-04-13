using System.Collections.Generic;
using MagicSettings.Models;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.ViewModels;

internal class MainWindowViewModel
{
    public List<NavigationMenuItem> NavigationMenuItems { get; }

    public MainWindowViewModel()
    {
        var loader = new ResourceLoader();

        NavigationMenuItems =
        [
            new(loader.GetString("MainMenu_Home"), "\xE80F", "Home"),
            new(loader.GetString("MainMenu_KeyBinding"), "\xE765", "KeyBinding"),
            new(loader.GetString("MainMenu_Display"), "\xE770", "Display"),
        ];
    }
}
