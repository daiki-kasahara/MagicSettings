using System.Collections.ObjectModel;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models;
using MagicSettings.Repositories.Models.SettingsFile;
using MagicSettings.ViewModels;
using Moq;

namespace MagicSettingsTest.ViewModels;

public class KeyboardPageViewModelTest
{
    [Fact]
    public async Task InitializeAsyncTest()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 1, new KeyboardAction(){ ActionType = MagicSettings.Domains.KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" } },
                { 2, new KeyboardAction(){ ActionType = MagicSettings.Domains.KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" } },
                { 3, new KeyboardAction(){ ActionType = MagicSettings.Domains.KeyboardActionType.MicrosoftStore, IsEnabled = false } },
            }
        };
        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.GetKeyBindingSettingsAsync()).ReturnsAsync(settings);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        await viewModel.InitializeAsync();

        // Assert
        Assert.True(viewModel.CanExecute);
        Assert.Equal(settings.IsEnabledKeyboardBinding, viewModel.IsEnabledKeyBinding);
        foreach (var actual in viewModel.KeyActions)
        {
            Assert.True(settings.KeyboardActions.ContainsKey((int)actual.VirtualKey));
            Assert.Equal(settings.KeyboardActions[(int)actual.VirtualKey].ActionType, actual.ActionType);
            Assert.Equal(settings.KeyboardActions[(int)actual.VirtualKey].ProgramPath, actual.ProgramPath);
            Assert.Equal(settings.KeyboardActions[(int)actual.VirtualKey].UrlPath, actual.UrlPath);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledKeyBindingAsyncTest(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetEnabledKeyBindingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.IsEnabledKeyBinding = !setValue;
        var actual = await viewModel.SetEnabledKeyBindingAsync(setValue);

        // Assert
        Assert.True(actual);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.EnabledError);
        Assert.Equal(setValue, viewModel.IsEnabledKeyBinding);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledKeyBindingAsyncTest_SameValue(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetEnabledKeyBindingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.IsEnabledKeyBinding = setValue;
        var actual = await viewModel.SetEnabledKeyBindingAsync(setValue);

        // Assert
        Assert.True(actual);
        serviceStub.Verify(x => x.SetEnabledKeyBindingAsync(setValue), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledKeyBindingAsyncTest_Failed(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetEnabledKeyBindingAsync(setValue)).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.IsEnabledKeyBinding = !setValue;
        var actual = await viewModel.SetEnabledKeyBindingAsync(setValue);

        // Assert
        Assert.False(actual);
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.EnabledError);
        Assert.Equal(!setValue, viewModel.IsEnabledKeyBinding);
    }

    [Fact]
    public async Task AddNewActionAsyncTest()
    {
        // Arrange
        var setKey = VKeys.R;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = true,
            ActionType = KeyboardActionType.StartProgram,
            ProgramPath = "C:\\Test.test",
            UrlPath = "https:\\Test.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = MagicSettings.Domains.KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = MagicSettings.Domains.KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = MagicSettings.Domains.KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.AddNewActionAsync(keyBindAction);

        // Assert
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 4);
        Assert.True(viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey) is not null);
    }

    [Fact]
    public async Task AddNewActionAsyncTest_HadAlreadyExist()
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = true,
            ActionType = KeyboardActionType.StartProgram,
            ProgramPath = "C:\\Test.test",
            UrlPath = "https:\\Test.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = MagicSettings.Domains.KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = MagicSettings.Domains.KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = MagicSettings.Domains.KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.AddNewActionAsync(keyBindAction);

        // Assert
        Assert.True(viewModel.KeyBindError);
        serviceStub.Verify(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>()), Times.Never);
    }

    [Fact]
    public async Task AddNewActionAsyncTest_FailedToAdd()
    {
        // Arrange
        var setKey = VKeys.R;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = true,
            ActionType = KeyboardActionType.StartProgram,
            ProgramPath = "C:\\Test.test",
            UrlPath = "https:\\Test.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.AddNewActionAsync(keyBindAction);

        // Assert
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
        Assert.True(viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey) is null);
    }

    [Fact]
    public async Task UpdateActionAsyncTest()
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = false,
            ActionType = KeyboardActionType.OpenUrl,
            ProgramPath = "C:\\TestUpdate.test",
            UrlPath = "https:\\TestUpdate.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = MagicSettings.Domains.KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = MagicSettings.Domains.KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = MagicSettings.Domains.KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionAsync(keyBindAction);

        // Assert
        var actual = viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
        Assert.True(actual is not null);
        Assert.Equal(keyBindAction.IsEnabled, actual.IsEnabled);
        Assert.Equal(keyBindAction.ActionType, actual.ActionType);
        Assert.Equal(keyBindAction.ProgramPath, actual.ProgramPath);
        Assert.Equal(keyBindAction.UrlPath, actual.UrlPath);
    }

    [Fact]
    public async Task UpdateActionAsyncTest_HasNotAction()
    {
        // Arrange
        var setKey = VKeys.R;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = true,
            ActionType = KeyboardActionType.StartProgram,
            ProgramPath = "C:\\Test.test",
            UrlPath = "https:\\Test.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionAsync(keyBindAction);

        // Assert
        Assert.True(viewModel.KeyBindError);
        serviceStub.Verify(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>()), Times.Never);
    }

    [Fact]
    public async Task UpdateActionAsyncTest_FailedToUpdate()
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyBindAction = new KeyBindAction
        {
            VirtualKey = setKey,
            IsEnabled = false,
            ActionType = KeyboardActionType.OpenUrl,
            ProgramPath = "C:\\TestUpdate.test",
            UrlPath = "https:\\TestUpdate.test"
        };

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionAsync(keyBindAction);

        // Assert
        var actual = viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey);
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
        Assert.True(actual is not null);
        Assert.NotEqual(keyBindAction.IsEnabled, actual.IsEnabled);
        Assert.NotEqual(keyBindAction.ActionType, actual.ActionType);
        Assert.NotEqual(keyBindAction.ProgramPath, actual.ProgramPath);
        Assert.NotEqual(keyBindAction.UrlPath, actual.UrlPath);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateActionEnabledAsyncTest(bool setValue)
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = !setValue, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionEnabledAsync(setKey, setValue);

        // Assert
        var actual = viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
        Assert.True(actual is not null);
        Assert.Equal(setValue, actual.IsEnabled);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateActionEnabledAsyncTest_HasNotAction(bool setValue)
    {
        // Arrange
        var setKey = VKeys.A;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionEnabledAsync(setKey, setValue);

        // Assert
        Assert.True(viewModel.KeyBindError);
        serviceStub.Verify(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>()), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateActionEnabledAsyncTest_SameValue(bool setValue)
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = setValue, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionEnabledAsync(setKey, setValue);

        // Assert
        Assert.False(viewModel.KeyBindError);
        serviceStub.Verify(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>()), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateActionEnabledAsyncTest_FailedToUpdate(bool setValue)
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = !setValue, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.SetKeyBindingActionAsync((int)setKey, It.IsAny<KeyboardAction>())).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.UpdateActionEnabledAsync(setKey, setValue);

        // Assert
        var actual = viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey);
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
        Assert.True(actual is not null);
        Assert.NotEqual(setValue, actual.IsEnabled);
    }

    [Fact]
    public async Task RemoveActionAsyncTest()
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.DeleteKeyBindingActionAsync((int)setKey)).ReturnsAsync(true);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.RemoveActionAsync(setKey);

        // Assert
        var actual = viewModel.KeyActions.FirstOrDefault(x => x.VirtualKey == setKey);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 2);
        Assert.Equal(default, actual);
    }

    [Fact]
    public async Task RemoveActionAsyncTest_HasNotAction()
    {
        // Arrange
        var setKey = VKeys.A;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.DeleteKeyBindingActionAsync((int)setKey)).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.RemoveActionAsync(setKey);

        // Assert
        Assert.True(viewModel.KeyBindError);
        serviceStub.Verify(x => x.DeleteKeyBindingActionAsync((int)setKey), Times.Never);
    }

    [Fact]
    public async Task RemoveActionAsyncTest_FailedToRemove()
    {
        // Arrange
        var setKey = VKeys.Z;

        var keyActions = new ObservableCollection<KeyBindAction>()
            {
                new KeyBindAction(){ VirtualKey = VKeys.Z, ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" },
                new KeyBindAction() { VirtualKey = VKeys.Y, ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" },
                new KeyBindAction(){ VirtualKey = VKeys.X, ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = false },
            };

        var serviceStub = new Mock<IKeyboardService>();
        serviceStub.Setup(x => x.DeleteKeyBindingActionAsync((int)setKey)).ReturnsAsync(false);

        // Act
        var viewModel = new KeyboardPageViewModel(serviceStub.Object);
        viewModel.KeyActions = keyActions;
        await viewModel.RemoveActionAsync(setKey);

        // Assert
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.KeyBindError);
        Assert.True(viewModel.KeyActions.Count == 3);
    }
}
