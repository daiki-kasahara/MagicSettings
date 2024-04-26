using System.Collections.Generic;
using MagicSettings.Models.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;

namespace MagicSettings.ViewModels;

internal class MainWindowViewModel
{
    public List<MenuItem> NavigationMenuItems { get; }

    public MainWindowViewModel()
    {
        var loader = new ResourceLoader();

        NavigationMenuItems =
        [
            new(loader.GetString($"MainMenu_{Tag.Keyboard}"), "\xE765", Tag.Keyboard),
            new(loader.GetString($"MainMenu_{Tag.Screen}"), "\xE770", Tag.Screen),
        ];
    }
}
