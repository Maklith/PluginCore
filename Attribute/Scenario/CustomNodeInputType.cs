using System;

namespace PluginCore.Attribute.Scenario;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class CustomNodeInputType : System.Attribute
{
    public Type Type { get; set; }

    public CustomNodeInputType(Type type)
    {
        Type = type;
    }
}