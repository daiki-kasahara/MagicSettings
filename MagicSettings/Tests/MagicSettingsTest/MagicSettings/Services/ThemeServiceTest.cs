using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Services;
using Moq;

namespace MagicSettingsTest.MagicSettings.Services;

public class ThemeServiceTest
{
    [Theory]
    [InlineData(AppTheme.Dark)]
    [InlineData(AppTheme.System)]
    [InlineData(AppTheme.Light)]
    public async Task GetCurrentThemeAsyncTest(AppTheme expected)
    {
        // Arrange
        var repositoryStub = new Mock<IThemeRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(expected);

        // Act
        var service = new ThemeService(repositoryStub.Object);
        var actual = await service.GetCurrentThemeAsync();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(AppTheme.Dark)]
    [InlineData(AppTheme.System)]
    [InlineData(AppTheme.Light)]
    public async Task SetCurrentThemeAsyncTest(AppTheme value)
    {
        // Arrange
        var repositoryStub = new Mock<IThemeRepository>();
        repositoryStub.Setup(x => x.SaveAsync(value));

        // Act
        var service = new ThemeService(repositoryStub.Object);
        await service.SetCurrentThemeAsync(value);

        // Assert
        repositoryStub.Verify(x => x.SaveAsync(value), Times.Once);
    }
}