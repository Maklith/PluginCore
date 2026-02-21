#region

using System;
using System.Linq;
using PluginCore.Config;

#endregion

namespace PluginCore.CustomScenario.Attribute.ConfigField;

[AttributeUsage(AttributeTargets.Field| AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ConfigField<TEnum> : ConfigField where TEnum : struct, Enum
{
    public ConfigField(string title, string description, int symbol = 0) :
        base(title, description, symbol, ConfigFieldType.自定义选项, Enum.GetValues(typeof(TEnum)).Cast<object>().ToArray())
    {
    }
}

[AttributeUsage(AttributeTargets.Field| AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ConfigField : System.Attribute
{
    public ConfigField(string title, string description, int symbol = 0,
        ConfigFieldType fieldType = ConfigFieldType.字符串, object[]? options = null, int maxValue = 0, int minValue = 0,
        int step = 0, string actionName = null
    )
    {
        Tittle = title;
        Description = description;
        Symbol = symbol;
        FieldType = fieldType;
        Options = options;
        MaxValue = maxValue;
        MinValue = minValue;
        Step = step;
        ActionName = actionName;
    }

    public string Tittle { get; set; }

    public string Description { get; set; }

    public int Symbol { get; set; }
    public ConfigFieldType FieldType { get; set; }

    public object[]? Options { get; set; }
    public int MaxValue { get; set; }
    public int MinValue { get; set; }
    public int Step { get; set; }
    public string? ActionName { get; set; }
}