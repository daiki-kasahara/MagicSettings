using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.ViewModels;
using Moq;

namespace MagicSettingsTest.MagicSettings.ViewModels;

public class ScreenPageViewModelTest
{
    [Fact]
    public async Task InitializeAsyncTest()
    {
        // Arrange
        var settings = new ScreenSettings();
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.GetScreenSettingsAsync()).ReturnsAsync(settings);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        await viewModel.InitializeAsync();

        // Assert
        Assert.True(viewModel.CanExecute);
        Assert.Equal(settings.IsEnabledBlueLightBlocking, viewModel.IsEnabledBlueLightBlocking);
        Assert.Equal((int)settings.BlueLightBlocking, viewModel.ReductionRate);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledBlueLightBlockingAsyncTest(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetEnabledBlueLightBlockingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.IsEnabledBlueLightBlocking = !setValue;
        var actual = await viewModel.SetEnabledBlueLightBlockingAsync(setValue);

        // Assert
        Assert.True(actual);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.HasError);
        Assert.Equal(setValue, viewModel.IsEnabledBlueLightBlocking);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledBlueLightBlockingAsyncTest_SameValue(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetEnabledBlueLightBlockingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.IsEnabledBlueLightBlocking = setValue;
        var actual = await viewModel.SetEnabledBlueLightBlockingAsync(setValue);

        // Assert
        Assert.True(actual);
        Assert.False(viewModel.HasError);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SetEnabledBlueLightBlockingAsyncTest_Failed(bool setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetEnabledBlueLightBlockingAsync(setValue)).ReturnsAsync(false);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.IsEnabledBlueLightBlocking = !setValue;
        var actual = await viewModel.SetEnabledBlueLightBlockingAsync(setValue);

        // Assert
        Assert.False(actual);
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.HasError);
        Assert.Equal(!setValue, viewModel.IsEnabledBlueLightBlocking);
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
    public async Task SetBlueLightBlockingAsyncTest(BlueLightBlocking setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetBlueLightBlockingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.ReductionRate = (int)BlueLightBlocking.None;
        var actual = await viewModel.SetBlueLightBlockingAsync((int)setValue);

        // Assert
        Assert.True(actual);
        Assert.True(viewModel.CanExecute);
        Assert.False(viewModel.HasError);
        Assert.Equal((int)setValue, viewModel.ReductionRate);
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
    public async Task SetBlueLightBlockingAsyncTest_SameValue(BlueLightBlocking setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetBlueLightBlockingAsync(setValue)).ReturnsAsync(true);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.ReductionRate = (int)setValue;
        var actual = await viewModel.SetBlueLightBlockingAsync((int)setValue);

        // Assert
        Assert.True(actual);
        Assert.False(viewModel.HasError);
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
    public async Task SetBlueLightBlockingAsyncTest_Failed(BlueLightBlocking setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetBlueLightBlockingAsync(setValue)).ReturnsAsync(false);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.ReductionRate = (int)BlueLightBlocking.None;
        var actual = await viewModel.SetBlueLightBlockingAsync((int)setValue);

        // Assert
        Assert.False(actual);
        Assert.True(viewModel.CanExecute);
        Assert.True(viewModel.HasError);
        Assert.Equal((int)BlueLightBlocking.None, viewModel.ReductionRate);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public async Task SetBlueLightBlockingAsyncTest_Failed_NotDefined(int setValue)
    {
        // Arrange
        var serviceStub = new Mock<IScreenService>();
        serviceStub.Setup(x => x.SetBlueLightBlockingAsync(It.IsAny<BlueLightBlocking>())).ReturnsAsync(false);

        // Act
        var viewModel = new ScreenPageViewModel(serviceStub.Object);
        viewModel.ReductionRate = (int)BlueLightBlocking.None;
        var actual = await viewModel.SetBlueLightBlockingAsync(setValue);

        // Assert
        Assert.False(actual);
        Assert.True(viewModel.HasError);
        Assert.Equal((int)BlueLightBlocking.None, viewModel.ReductionRate);
    }
}
