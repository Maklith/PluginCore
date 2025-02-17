using System;

namespace PluginCore;

public interface IPluginManger
{
    public Type GetType(string[] name);
    public PluginBaseInfo? GetPluginInfo(Type name);
}