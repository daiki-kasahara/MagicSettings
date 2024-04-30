using MagicSettings.Repositories.Repositories;

namespace MagicSettings.Repositories;

public class AssemblyInfoRepositoryTest
{
    [Fact]
    public async Task GetAsyncTest()
    {
        // Arrange

        // Action
        var repository = new AssemblyInfoRepository();
        var result = await repository.GetAsync();

        // Assert
        Assert.NotEqual(string.Empty, result.AppName);
        Assert.NotEqual(string.Empty, result.AppVersion);
        Assert.NotEqual(string.Empty, result.Copyright);
    }
}
