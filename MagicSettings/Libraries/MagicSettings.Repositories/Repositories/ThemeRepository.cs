﻿using System.Text.Json;
using MagicSettings.Domains;
using MagicSettings.Repositories.Contracts;
using MagicSettings.Repositories.Helper;

namespace MagicSettings.Repositories;

public class ThemeRepository : IThemeRepository
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "Settings", "theme.json");

    public async Task<AppTheme> GetAsync()
    {
        try
        {
            if (!File.Exists(FilePath))
                return AppTheme.System;

            using var openStream = File.OpenRead(FilePath);
            return (await JsonSerializer.DeserializeAsync<ThemeSetting>(openStream))?.Theme ?? AppTheme.System;
        }
        catch (Exception)
        {
            // 例外の場合、Systemと返す
            return AppTheme.System;
        }
    }

    public async Task SaveAsync(AppTheme theme)
    {
        try
        {
            Directory.CreateDirectory(Directory.GetParent(FilePath)!.FullName);

            var themeSettings = new ThemeSetting()
            {
                Theme = theme
            };

            await using var createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, themeSettings, JsonOptionHelper.GetOption());
        }
        catch (Exception)
        {
            // 例外の場合、何もしない
        }
    }
}
