using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Helper;

namespace MagicSettings.Repositories;

/// <summary>
/// スクリーンのリポジトリ
/// </summary>
public class ScreenRepository : IScreenRepository
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "screen.json");

    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public async Task<ScreenSettings> GetAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new();

            using var openStream = File.OpenRead(FilePath);
            return (await JsonSerializer.DeserializeAsync<ScreenSettings>(openStream)) ?? new();
        }
        catch (Exception)
        {
            // 例外の場合、デフォルト値を返す
            return new();
        }
    }

    /// <summary>
    /// 軽減率を保存する
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task SaveAsync(BlueLightBlocking value)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var screenSettings = await GetAsync();
            screenSettings.BlueLightBlocking = value;

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, screenSettings, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }

    /// <summary>
    /// 有効無効を保存する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public async Task SaveAsync(bool isEnabled)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var screenSettings = await GetAsync();
            screenSettings.IsEnabledBlueLightBlocking = isEnabled;

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, screenSettings, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }
}
