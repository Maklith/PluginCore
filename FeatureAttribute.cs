using System;

namespace PluginCore;

public enum FeatureActivationMode
{
    Direct,
    ScreenCapture
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class FeatureAttribute : Attribute
{
    public FeatureAttribute(
        string id,
        string name,
        string description,
        string category,
        int iconSymbol,
        int order)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        IconSymbol = iconSymbol;
        Order = order;
    }

    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Category { get; }
    public int IconSymbol { get; }
    public int Order { get; }
    public FeatureActivationMode Activation { get; set; }
}
