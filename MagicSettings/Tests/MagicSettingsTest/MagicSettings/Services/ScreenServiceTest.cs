using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Services;
using Moq;
using ProcessManager;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace MagicSettingsTest.MagicSettings.Services;

public class ScreenServiceTest
{
    [Fact]
    public async Task GetScreenSettingsAsyncTest()
    {
        // Arrange
        var expected = new ScreenSettings();
        var repositoryStub = new Mock<IScreenRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(expected);

        // Act
        var service = new ScreenService(repositoryStub.Object, new ProcessController());
        var actual = await service.GetScreenSettingsAsync();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(BlueLightBlocking.Ten)]
    [InlineData(BlueLightBlocking.Twenty)]
    [InlineData(BlueLightBlocking.Thirty)]
    [InlineData(BlueLightBlocking.Forty)]
    [InlineData(BlueLightBlocking.Fifty)]
    [InlineData(BlueLightBlocking.Sixty)]
    [InlineData(BlueLightBlocking.Seventy)]
    [InlineData(BlueLightBlocking.Eighty)]
    [InlineData(BlueLightBlocking.Ninety)]
    [InlineData(BlueLightBlocking.OneHundred)]
    public async Task SetBlueLightBlockingAsyncTest(BlueLightBlocking value)
    {
        // Arrange
        var repositoryStub = new Mock<IScreenRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.SendMessageAsync(MyProcesses.ScreenController, It.IsAny<RequestMessage>()))
            .ReturnsAsync(true);

        // Act
        var service = new ScreenService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetBlueLightBlockingAsync(value);

        // Assert
        Assert.True(ret);
        repositoryStub.Verify(x => x.SaveAsync(value), Times.Once);
    }

    [Fact]
    public async Task SetBlueLightBlockingAsyncTest_Failed()
    {
        // Arrange
        var repositoryStub = new Mock<IScreenRepository>();
        repositoryStub.Setup(x => x.SaveAsync(BlueLightBlocking.None));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.SendMessageAsync(MyProcesses.ScreenController, It.IsAny<RequestMessage>()))
            .ReturnsAsync(false);

        // Act
        var service = new ScreenService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetBlueLightBlockingAsync(BlueLightBlocking.None);

        // Assert
        Assert.False(ret);
        repositoryStub.Verify(x => x.SaveAsync(BlueLightBlocking.None), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledBlueLightBlockingAsyncTest(bool value)
    {
        // Arrange
        var repositoryStub = new Mock<IScreenRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.LaunchAsync(MyProcesses.ScreenController))
            .ReturnsAsync(true);
        controllerStub.Setup(x => x.TerminateAsync(MyProcesses.ScreenController))
            .ReturnsAsync(true);

        // Act
        var service = new ScreenService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetEnabledBlueLightBlockingAsync(value);

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
        var repositoryStub = new Mock<IScreenRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));
        var controllerStub = new Mock<IProcessController>();
        controllerStub.Setup(x => x.LaunchAsync(MyProcesses.ScreenController))
            .ReturnsAsync(false);
        controllerStub.Setup(x => x.TerminateAsync(MyProcesses.ScreenController))
            .ReturnsAsync(false);

        // Act
        var service = new ScreenService(repositoryStub.Object, controllerStub.Object);
        var ret = await service.SetEnabledBlueLightBlockingAsync(value);

        // Assert
        Assert.False(ret);
        repositoryStub.Verify(x => x.SaveAsync(value), Times.Once);
    }
}
