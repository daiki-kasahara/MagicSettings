using System.Text;
using System.Text.Json;
using MagicSettings.Domains;

namespace MagicSettings.Repositories;

public class ThemeRepositoryTest
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "theme.json");

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
    public async Task GetAsyncTest_InvalidJson()
    {
        // Arrange
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath)) { }

        var enc = Encoding.UTF8;
        using (var writer = new StreamWriter(FilePath, false, enc))
        {
            writer.WriteLine("InvalidJson");
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
