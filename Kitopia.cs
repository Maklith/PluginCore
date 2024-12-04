using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PluginCore;

public class Kitopia
{
    public static ISearchItemTool ISearchItemTool;
    public static IToastService IToastService;
    public static Dictionary<string, string> _i18n;
    public static Dictionary<Type, Func<object, string>> ToolTipConverters;
    public static Dictionary<Type, JsonConverter> JsonConverters;
}