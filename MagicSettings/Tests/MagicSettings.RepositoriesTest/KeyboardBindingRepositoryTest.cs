using System.Text;
using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Repositories;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.RepositoriesTest;

public class KeyboardBindingRepositoryTest
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "keybinding.json");

    [Fact]
    public async Task GetAsyncTest()
    {
        // Arrange
        var settings = new KeyboardBindingSettings();
        settings.IsEnabledKeyboardBinding = true;
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new KeyboardBindingRepository();
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
        var repository = new KeyboardBindingRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.False(result.IsEnabledKeyboardBinding);
        Assert.Null(result.KeyboardActions);
    }

    [Fact]
    public async Task GetAsyncTest_Failed_InvaliedJson()
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
        var repository = new KeyboardBindingRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.False(result.IsEnabledKeyboardBinding);
        Assert.Null(result.KeyboardActions);
    }

    [Fact]
    public async Task SaveAsyncTest()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        var setKey = 1;
        var setAction = new KeyboardAction();
        setAction.ActionType = KeyboardActionType.StartProgram;
        setAction.IsEnabled = true;
        setAction.ProgramPath = "C:\\TestPath";
        setAction.UrlPath = "https:\\test.test";
        var filePath = Path.Combine(AppContext.BaseDirectory, "Settings", "keybinding.json");

        // Action
        var repository = new KeyboardBindingRepository();
        await repository.SaveAsync(setKey, setAction);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<KeyboardBindingSettings>(openStream);
        var actualAction = actual?.KeyboardActions?.FirstOrDefault(x => x.Key == setKey).Value;

        Assert.NotNull(actualAction);
        Assert.Equal(setAction.ActionType, actualAction.ActionType);
        Assert.Equal(setAction.IsEnabled, actualAction.IsEnabled);
        Assert.Equal(setAction.ProgramPath, actualAction.ProgramPath);
        Assert.Equal(setAction.UrlPath, actualAction.UrlPath);
    }

    [Fact]
    public async Task SaveAsyncTest_HasAlreadyExist()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        var setKey = 1;
        var setAction = new KeyboardAction();
        setAction.ActionType = KeyboardActionType.StartProgram;
        setAction.IsEnabled = true;
        setAction.ProgramPath = "C:\\TestPath";
        setAction.UrlPath = "https:\\test.test";
        var filePath = Path.Combine(AppContext.BaseDirectory, "Settings", "keybinding.json");

        // Action
        var repository = new KeyboardBindingRepository();
        await repository.SaveAsync(setKey, setAction);
        await repository.SaveAsync(setKey, setAction);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<KeyboardBindingSettings>(openStream);
        var actualAction = actual?.KeyboardActions?.FirstOrDefault(x => x.Key == setKey).Value;

        Assert.NotNull(actualAction);
        Assert.Equal(setAction.ActionType, actualAction.ActionType);
        Assert.Equal(setAction.IsEnabled, actualAction.IsEnabled);
        Assert.Equal(setAction.ProgramPath, actualAction.ProgramPath);
        Assert.Equal(setAction.UrlPath, actualAction.UrlPath);
    }

    [Fact]
    public async Task SaveAsyncTest_IsEnabled()
    {
        // Arrange
        if (File.Exists(FilePath))
            File.Delete(FilePath);

        // Action
        var repository = new KeyboardBindingRepository();
        await repository.SaveAsync(true);

        // Assert
        using var openStream = File.OpenRead(FilePath);
        var actual = await JsonSerializer.DeserializeAsync<KeyboardBindingSettings>(openStream);

        Assert.NotNull(actual);
        Assert.True(actual.IsEnabledKeyboardBinding);
    }

    [Fact]
    public async Task DeleteAsyncTest()
    {
        // Arrange
        var targetKey = 1;
        var settings = new KeyboardBindingSettings();
        settings.KeyboardActions = new Dictionary<int, KeyboardAction>()
        {
            { targetKey, new KeyboardAction() {} }
        };
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new KeyboardBindingRepository();
        var exception = await Record.ExceptionAsync(async () => await repository.DeleteAsync(targetKey));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task DeleteAsyncTest_KeyActionNull()
    {
        // Arrange
        var targetKey = 1;
        var settings = new KeyboardBindingSettings();
        settings.KeyboardActions = null;
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

        using (var createStream = File.Create(FilePath))
        {
            await JsonSerializer.SerializeAsync(createStream, settings);
        }

        // Action
        var repository = new KeyboardBindingRepository();
        var exception = await Record.ExceptionAsync(async () => await repository.DeleteAsync(targetKey));

        // Assert
        Assert.Null(exception);
    }
}
