using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PluginCore.CustomScenario;

public class ToolTipConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        if (value is CustomScenarioValue valueTuple)
        {
            if (valueTuple.Value ==null)
            {
                return null;
            }
            if (Kitopia.ToolTipConverters.ContainsKey(valueTuple.ShowType))
                return Kitopia.ToolTipConverters[valueTuple.ShowType].Invoke(valueTuple.Value);
            else
                return valueTuple.Value?.ToString();
        }

        if (Kitopia.ToolTipConverters.ContainsKey(value.GetType()))
            return (string)(Kitopia.ToolTipConverters[value.GetType()].Invoke(value));
        
        else
        {
            var convert = value.ToString();
            if (string.IsNullOrEmpty(convert))
            {
                return null;
            }
            return convert;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}