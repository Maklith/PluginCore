using System;

namespace PluginCore.Attribute;

[AttributeUsage(AttributeTargets.Field| AttributeTargets.Method)]
public class ConfigFieldCategory : System.Attribute
{
    public string Category { get; set; }

    public ConfigFieldCategory(string category)
    {
        Category = category;
    }
}