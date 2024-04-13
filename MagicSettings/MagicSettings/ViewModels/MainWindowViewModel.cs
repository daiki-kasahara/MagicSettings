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
            new(loader.GetString($"MainMenu_{Tag.Home}"), "\xE80F", Tag.Home),
            new(loader.GetString($"MainMenu_{Tag.KeyBinding}"), "\xE765", Tag.KeyBinding),
            new(loader.GetString($"MainMenu_{Tag.Display}"), "\xE770", Tag.Display),
        ];
    }
}
