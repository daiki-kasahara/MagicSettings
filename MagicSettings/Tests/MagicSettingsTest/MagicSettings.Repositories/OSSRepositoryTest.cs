using MagicSettings.Repositories.Repositories;

namespace MagicSettingsTest.MagicSettings.Repositories;

public class OSSRepositoryTest : IDisposable
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "OSS", "oss.json");

    public OSSRepositoryTest()
    {
        // 評価前に正しいファイルをコピーしておく
        Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);
        File.Copy(FilePath, FilePath + ".temp", true);
    }

    // 評価後に正しいファイルに戻しておく
    public void Dispose() => File.Move(FilePath + ".temp", FilePath, true);

    [Fact]
    public async Task GetAsyncTest()
    {
        // Arrange

        // Action
        var repository = new OSSRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsyncTest_IsNull()
    {
        // Arrange
        using (var createStream = new StreamWriter(FilePath))
        {
            await createStream.WriteAsync("InvalidJson");
        }

        // Action
        var repository = new OSSRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsyncTest_FileNotExists()
    {
        // Arrange
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        // Action
        var repository = new OSSRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.Null(result);
    }
}
