using System;

namespace PluginCore.Attribute;
[AttributeUsage(AttributeTargets.Method)]
public class CaptureAttribute : System.Attribute
{
    public string Description { get; set; }

    public int Symbol { get; set; }

    public CaptureAttribute( string description, int symbol = 0)
    {
        Description = description;
        Symbol = symbol;
    }
}