using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PluginCore.CustomScenario;

public class ToolTipConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var fallback = parameter?.ToString();
        if (value == null) return fallback;
        if (value is CustomScenarioValue valueTuple)
        {
            if (valueTuple.Value == null) return fallback;

            if (Kitopia.ToolTipConverters.TryGetValue(valueTuple.ShowType, out var valueConverter))
            {
                var convertedValue = valueConverter.Invoke(valueTuple.Value);
                return string.IsNullOrEmpty(convertedValue) ? fallback : convertedValue;
            }

            return valueTuple.Value.ToString();
        }

        if (Kitopia.ToolTipConverters.TryGetValue(value.GetType(), out var converter))
        {
            var convertedValue = converter.Invoke(value);
            return string.IsNullOrEmpty(convertedValue) ? fallback : convertedValue;
        }

        var text = value.ToString();
        return string.IsNullOrEmpty(text) ? fallback : text;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
