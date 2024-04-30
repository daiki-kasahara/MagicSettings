using System.Text.Json;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories.Repositories;

public class OSSRepository : IOSSRepository
{
    private static readonly string DirPath = Path.Combine(AppContext.BaseDirectory, "OSS");
    private static readonly string FilePath = Path.Combine(DirPath, "oss.json");

    public async Task<OSSFile?> GetAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return null;

            using var openStream = File.OpenRead(FilePath);
            var license = await JsonSerializer.DeserializeAsync<OSSFile>(openStream);

            if (license is null)
                return null;

            foreach (var oss in license.OSS)
            {
                using var stream = new StreamReader(Path.Combine(DirPath, oss.Path));
                var content = await stream.ReadToEndAsync();
                oss.Content = content;
            }

            return license;
        }
        catch (Exception)
        {
            // 例外の場合、null と返す
            return null;
        }
    }
}
