using System;
using MagicSettings.Domains;
using MagicSettings.Models;
using Microsoft.UI.Xaml.Data;

namespace MagicSettings.Views.Converters;

internal class EnumToKeyDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not VKeys key)
            return string.Empty;

        return CustomDisplayKeys.Keys.TryGetValue((int)key, out var keyString) ? keyString : key.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
