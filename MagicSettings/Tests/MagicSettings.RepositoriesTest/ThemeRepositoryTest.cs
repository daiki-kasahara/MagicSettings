﻿using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Repositories;

namespace MagicSettings.RepositoriesTest;

public class ThemeRepositoryTest
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "theme.json");
    private class TestJsonObject
    {
        public int TestInt { get; set; }
    }

    [Fact]
    public async Task GetAsyncTest()
    {
        // Arrange
        var settings = new ThemeSetting
        {
            Theme = AppTheme.Light
        };
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new ThemeRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.NotEqual(default, result);
    }

    [Fact]
    public async Task GetAsyncTest_NotExist()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        // Action
        var repository = new ThemeRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.Equal(AppTheme.System, result);
    }

    [Fact]
    public async Task GetAsyncTest_NotTargetString()
    {
        // Arrange
        var settings = new TestJsonObject();
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new ThemeRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.Equal(AppTheme.System, result);
    }

    [Fact]
    public async Task SaveAsyncTest()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        var setValue = AppTheme.Light;

        // Action
        var repository = new ThemeRepository();
        await repository.SaveAsync(setValue);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<ThemeSetting>(openStream);

        Assert.NotNull(actual);
        Assert.Equal(setValue, actual.Theme);
    }
}
