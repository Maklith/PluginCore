using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PluginCore;

public class ToolTipConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        if (value is CustomScenarioValue valueTuple)
        {
            if (Kitopia.ToolTipConverters.ContainsKey(valueTuple.RealType))
                return Kitopia.ToolTipConverters[valueTuple.RealType].Invoke(valueTuple.Value);
            else
                return valueTuple.Value?.ToString();
        }

        if (Kitopia.ToolTipConverters.ContainsKey(value.GetType()))
            return (string)(Kitopia.ToolTipConverters[value.GetType()].Invoke(value));
        else return value.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}