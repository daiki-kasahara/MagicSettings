﻿using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models;
using MagicSettings.Repositories.Repositories;
using MagicSettings.ViewModels;
using Moq;

namespace MagicSettingsTest.ViewModels;

public class SettingsViewModelTest
{
    [Fact]
    public async Task InitializeAsyncTest()
    {
        // Arrange
        var expectedTheme = AppTheme.Light;
        var expectedAbout = new About("TestName", "1.0.1000.0", "Copyright");
        var serviceStub = new Mock<IThemeService>();
        serviceStub.Setup(x => x.GetCurrentThemeAsync()).ReturnsAsync(expectedTheme);
        var repositoryStub = new Mock<IAssemblyInfoRepository>();
        repositoryStub.Setup(x => x.GetAsync()).ReturnsAsync(expectedAbout);

        // Act
        var viewModel = new SettingsPageViewModel(serviceStub.Object, repositoryStub.Object);
        await viewModel.InitializeAsync();

        // Assert
        Assert.Equal(expectedTheme, viewModel.Theme);
        Assert.Equal(expectedAbout, viewModel.About);
    }

    [Theory]
    [InlineData(AppTheme.Dark)]
    [InlineData(AppTheme.System)]
    [InlineData(AppTheme.Light)]
    public async Task SetCurrentThemeAsyncTest(AppTheme setValue)
    {
        // Arrange
        var serviceStub = new Mock<IThemeService>();
        serviceStub.Setup(x => x.SetCurrentThemeAsync(setValue));

        // Act
        var viewModel = new SettingsPageViewModel(serviceStub.Object, new AssemblyInfoRepository());
        await viewModel.SetCurrentThemeAsync(setValue);

        // Assert
        Assert.Equal(setValue, viewModel.Theme);
        serviceStub.Verify(x => x.SetCurrentThemeAsync(setValue), Times.Once);
    }
}
