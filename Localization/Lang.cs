using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace PluginCore.Localization;

public static class Lang
{
    // The host supplies the shared dictionary without adding a host assembly dependency to plugins.
    public static Func<string, string> Lookup { private get; set; } = key => key;
    public static INotifyPropertyChanged? BindingSource { get; set; }

    public static string Get(string key) => Lookup(key);

    public static string Get(Enum value) => Get(value.GetType().GetField(value.ToString())
        ?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString());

    public static string Format(string key, params object?[] args) =>
        string.Format(CultureInfo.CurrentUICulture, Get(key), args);
}
