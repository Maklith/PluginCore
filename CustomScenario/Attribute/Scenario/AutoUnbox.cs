#region

using System;

#endregion

namespace PluginCore.CustomScenario.Attribute.Scenario;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AutoUnbox : System.Attribute
{
}