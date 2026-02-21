#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace PluginCore.CustomScenario.Attribute.Scenario;

/// <summary>
/// 由该特性标记的方法会被Scenario索引为方法(Node)
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class ScenarioMethodAttribute : System.Attribute
{
    public ScenarioMethodAttribute()
    {
    }

    public ScenarioMethodAttribute(string name, params string[]? parameterName)
    {
        Name = name;
        ParameterName = parameterName?.Select(s => s.Split('='))
            .ToDictionary(s => s[0], s => s[1]);
    }


    public string Name { get; set; }

    public Dictionary<string, string>? ParameterName { get; set; }

    public string GetParameterName(string key)
    {
        if (ParameterName.TryGetValue(key, out var name)) return name;

        return key;
    }
}