using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models.SettingsFile;
using MagicSettings.Services;
using Moq;
using ProcessManager;
using ProcessManager.Contracts;

namespace MagicSettingsTest.Services;

public class KeyboardServiceTest
{
    [Fact]
    public async Task GetKeyBindingSettingsAsyncTest()
    {
        // Arrange
        var expected = new KeyboardBindingSettings();
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(expected);

        // Act
        var service = new KeyboardService(repositoryStub.Object, new ProcessController());
        var actual = await service.GetKeyBindingSettingsAsync();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task SetKeyBindingActionAsyncTest()
    {
        // Arrange
        var saveKey = 1;
        var saveAction = new KeyboardAction();
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.SaveAsync(saveKey, saveAction));

        // Act
        var service = new KeyboardService(repositoryStub.Object, new ProcessController());
        var actual = await service.SetKeyBindingActionAsync(saveKey, saveAction);

        // Assert
        Assert.True(actual);
        repositoryStub.Verify(x => x.SaveAsync(saveKey, saveAction), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledKeyBindingAsyncTest(bool value)
    {
        // Arrange
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.LaunchAsync(MyProcesses.KeyBindingListener))
            .ReturnsAsync(true);
        controllerStub.Setup(x => x.TerminateAsync(MyProcesses.KeyBindingListener))
            .ReturnsAsync(true);

        // Act
        var service = new KeyboardService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetEnabledKeyBindingAsync(value);

        // Assert
        Assert.True(ret);
        repositoryStub.Verify(x => x.SaveAsync(value), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledBlueLightBlockingAsyncTest_Failed(bool value)
    {
        // Arrange
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.LaunchAsync(MyProcesses.ScreenController))
            .ReturnsAsync(false);
        controllerStub.Setup(x => x.TerminateAsync(MyProcesses.ScreenController))
            .ReturnsAsync(false);

        // Act
        var service = new KeyboardService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetEnabledKeyBindingAsync(value);

        // Assert
        Assert.False(ret);
        repositoryStub.Verify(x => x.SaveAsync(value), Times.Once);
    }

    [Fact]
    public async Task DeleteKeyBindingActionAsyncTest()
    {
        // Arrange
        var saveKey = 1;
        var repositoryStub = new Mock<IKeyboardBindingRepository>();
        repositoryStub.Setup(x => x.DeleteAsync(saveKey));

        // Act
        var service = new KeyboardService(repositoryStub.Object, new ProcessController());
        var actual = await service.DeleteKeyBindingActionAsync(saveKey);

        // Assert
        Assert.True(actual);
        repositoryStub.Verify(x => x.DeleteAsync(saveKey), Times.Once);
    }
}
