namespace PluginCore.Config;

public interface IConfigProvider
{
    T Get<T>() where T : ConfigBase;
}
