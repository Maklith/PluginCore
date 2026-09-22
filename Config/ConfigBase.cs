using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PluginCore.Config;

public class ConfigChangedArgs : EventArgs
{
    public string Name { get; }
    public object? Value { get; }

    public ConfigChangedArgs(string name, object? value)
    {
        Name = name;
        Value = value;
    }
}

public class ConfigBase
{
    public event EventHandler<ConfigChangedArgs> ConfigChanged;

    public void OnConfigChanged(object sender, string name, object? value)
    {
        ConfigChanged?.Invoke(sender, new ConfigChangedArgs(name, value));
    }

    [JsonIgnore] public string Name { get; set; } = string.Empty;
    public int ConfigVersion { get; set; }

    [JsonIgnore]
    public virtual int CurrentConfigVersion => 0;

    [Obsolete("仅兼容旧插件的加载回调；在回调中使用 this，其他位置通过 IConfigProvider.Get<T>() 获取配置。")]
    public static ConfigBase? Instance;
    [JsonIgnore] public Dictionary<string, object> invokes { get; init; } = new();

    public virtual void MigrateConfig(JsonElement root)
    {
    }


    public virtual void BeforeLoad()
    {
    }

    public virtual void AfterLoad()
    {
    }

    public virtual void BeforeSave()
    {
    }

    public virtual void AfterSave()
    {
    }
}
