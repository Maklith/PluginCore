using System;

namespace PluginCore.Attribute;

/// <summary>
/// 在方法是<see cref="ScenarioMethodAttribute"/> 的情况下参数使用该特性, 表明该参数可以由用户手动输入>
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class SelfInput : System.Attribute
{
}