using System;

namespace PluginCore;

public interface IPluginManger
{
    public Type GetType(string[] name);
    public PluginInfo? GetPluginInfo(Type name);
}