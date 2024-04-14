using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;

namespace MagicSettings.Repositories;

public class ScreenRepository : IScreenRepository
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "screen.json");

    public async Task<BlueLightBlocking> GetAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return BlueLightBlocking.Ten;

            using var openStream = File.OpenRead(FilePath);
            return (await JsonSerializer.DeserializeAsync<ScreenSettings>(openStream))?.BlueLightBlocking ?? BlueLightBlocking.Ten;
        }
        catch (Exception)
        {
            // 例外の場合、10を返す
            return BlueLightBlocking.Ten;
        }
    }

    public async Task SaveAsync(BlueLightBlocking value)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var screenSettings = new ScreenSettings()
            {
                BlueLightBlocking = value
            };

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, screenSettings);
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }
}
