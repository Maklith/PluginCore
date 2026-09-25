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
        ParameterName = parameterName?
            .Select(value => value.Split('=', 2))
            .Where(parts => parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]))
            .ToDictionary(parts => parts[0], parts => parts[1]);
    }


    public string Name { get; set; }

    /// <summary>
    /// Stable identifier used to resolve the method after a source rename.
    /// </summary>
    public string? Id { get; set; }

    public Dictionary<string, string>? ParameterName { get; set; }

    public string GetParameterName(string? key)
    {
        if (key is not null && ParameterName?.TryGetValue(key, out var name) == true) return name;

        return key ?? string.Empty;
    }
}
