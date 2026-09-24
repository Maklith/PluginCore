using System;

using System.Reflection;

namespace PluginCore;
/// <summary>
/// Kitopia核心服务
/// </summary>
public static class ServiceManager
{
    public static IServiceProvider Services { get; set; }
    
    public static string Version = typeof(ServiceManager).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion
        ?? throw new InvalidOperationException("PluginCore assembly is missing its product version.");
}
