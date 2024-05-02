using System.Text.Json;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Helper;
using MagicSettings.Repositories.Models.SettingsFile;

namespace MagicSettings.Repositories;

/// <summary>
/// キーバインディングのリポジトリ
/// </summary>
public class KeyboardBindingRepository : IKeyboardBindingRepository
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "keybinding.json");

    /// <summary>
    /// 取得する
    /// </summary>
    /// <returns></returns>
    public async Task<KeyboardBindingSettings> GetAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new();

            using var openStream = File.OpenRead(FilePath);
            return (await JsonSerializer.DeserializeAsync<KeyboardBindingSettings>(openStream)) ?? new();
        }
        catch (Exception)
        {
            // 例外の場合、デフォルト値を返す
            return new();
        }
    }

    /// <summary>
    /// キーバインディングを保存する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task SaveAsync(int key, KeyboardAction action)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var keyActions = await GetAsync();

            keyActions.KeyboardActions ??= [];

            if (!keyActions.KeyboardActions.TryAdd(key, action))
            {
                keyActions.KeyboardActions[key] = action;
            }

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, keyActions, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }

    /// <summary>
    /// キーバインディングの有効無効を保存する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public async Task SaveAsync(bool isEnabled)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var keyActions = await GetAsync();
            keyActions.IsEnabledKeyboardBinding = isEnabled;

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, keyActions, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }

    /// <summary>
    /// キーバインディングを削除する
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int key)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var keyActions = await GetAsync();

            if (keyActions.KeyboardActions is null)
                return;

            _ = keyActions.KeyboardActions.Remove(key);

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, keyActions, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }
}
