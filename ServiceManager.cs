using System;

namespace PluginCore;
/// <summary>
/// Kitopia核心服务
/// </summary>
public static class ServiceManager
{
    public static IServiceProvider Services { get; set; }
    
    public static string Version = "0.2.1.4";
}