using System;

namespace PluginCore.CustomScenario.Attribute.ConfigField;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
public class ConfigFieldCategory : System.Attribute
{
    public string Category { get; set; }
    public int Symbol { get; set; }

    public ConfigFieldCategory(string category, int symbol = 0)
    {
        Category = category;
        Symbol = symbol;
    }
}
