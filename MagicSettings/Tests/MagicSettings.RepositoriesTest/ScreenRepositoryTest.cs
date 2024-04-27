using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories;

namespace MagicSettings.RepositoriesTest;

public class ScreenRepositoryTest
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "screen.json");
    private class TestJsonObject
    {
        public int TestInt { get; set; }
    }

    [Fact]
    public async Task GetAsyncTest()
    {
        // Arrange
        var settings = new ScreenSettings();
        settings.IsEnabledBlueLightBlocking = true;
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new ScreenRepository();
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
        var repository = new ScreenRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.False(result.IsEnabledBlueLightBlocking);
        Assert.Equal(BlueLightBlocking.Twenty, result.BlueLightBlocking);
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
        var repository = new ScreenRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.False(result.IsEnabledBlueLightBlocking);
        Assert.Equal(BlueLightBlocking.Twenty, result.BlueLightBlocking);
    }

    [Fact]
    public async Task SaveAsyncTest()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        var setValue = BlueLightBlocking.Ten;

        // Action
        var repository = new ScreenRepository();
        await repository.SaveAsync(setValue);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<ScreenSettings>(openStream);

        Assert.NotNull(actual);
        Assert.Equal(setValue, actual.BlueLightBlocking);
    }

    [Fact]
    public async Task SaveAsyncTest_IsEnabled()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        // Action
        var repository = new ScreenRepository();
        await repository.SaveAsync(true);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<ScreenSettings>(openStream);

        Assert.NotNull(actual);
        Assert.True(actual.IsEnabledBlueLightBlocking);
    }
}
