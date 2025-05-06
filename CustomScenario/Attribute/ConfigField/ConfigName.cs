using System;

namespace PluginCore.Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigName : System.Attribute
{
    public string Name { get; }

    public ConfigName(string name)
    {
        Name = name;
    }
}