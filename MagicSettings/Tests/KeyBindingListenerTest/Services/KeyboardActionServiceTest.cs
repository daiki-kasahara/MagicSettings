using KeyBindingListener.Contracts;
using KeyBindingListener.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models.SettingsFile;
using Moq;

namespace KeyBindingListenerTest.Services;

public class KeyboardActionServiceTest
{
    [Theory]
    [InlineData(VKeys.A)]
    [InlineData(VKeys.B)]
    [InlineData(VKeys.C)]
    public async Task ActionAsyncTest(VKeys key)
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" } },
                { 0x42, new KeyboardAction(){ ActionType = KeyboardActionType.OpenUrl, IsEnabled = true, UrlPath = "https:\\test.test" } },
                { 0x43, new KeyboardAction(){ ActionType = KeyboardActionType.MicrosoftStore, IsEnabled = true } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var actionStub = new Mock<IAction>();
        actionStub.Setup(x => x.Action());

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>())).Returns(actionStub.Object);

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(key));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Once);
        actionStub.Verify(x => x.Action(), Times.Once);
    }

    [Fact]
    public async Task ActionAsyncTest_DoNothing_Disable()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = false,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()));

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(VKeys.A));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ActionAsyncTest_DoNothing_NotSetting()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = "C:\\TestPath" } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()));

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(VKeys.D));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ActionAsyncTest_DoNothing_FileNameIsNull()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = KeyboardActionType.StartProgram, IsEnabled = true, ProgramPath = null } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()));

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(VKeys.A));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ActionAsyncTest_DoNothing_ActionDisabled()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = KeyboardActionType.StartProgram, IsEnabled = false, ProgramPath = "C:\\TestPath" } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()));

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(VKeys.A));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ActionAsyncTest_DoNothing_ActionIsNull()
    {
        // Arrange
        var settings = new KeyboardBindingSettings
        {
            IsEnabledKeyboardBinding = true,
            KeyboardActions = new Dictionary<int, KeyboardAction>()
            {
                { 0x41, new KeyboardAction(){ ActionType = null, IsEnabled = true, ProgramPath = "C:\\TestPath" } },
            }
        };
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(settings);

        var factoryStub = new Mock<IActionFactory>();
        factoryStub.Setup(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()));

        // Act
        var service = new KeyboardActionService(repositoryStub.Object, factoryStub.Object);
        var exception = await Record.ExceptionAsync(async () => await service.ActionAsync(VKeys.A));

        // Assert
        Assert.Null(exception);
        factoryStub.Verify(x => x.Create(It.IsAny<KeyboardActionType>(), It.IsAny<string>()), Times.Never);
    }
}
