using System;

namespace PluginCore;

public interface IPluginManger
{
    public Type GetType(string[] name);
    public PluginBaseInfo? GetPluginInfo(Type name);
    public bool IsTypeFromThePlugin(Type type, string pluginName);
}