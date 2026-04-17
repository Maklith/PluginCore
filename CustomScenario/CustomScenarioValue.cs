using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore.CustomScenario;

public partial class CustomScenarioValue : ObservableObject
{
    public CustomScenarioValue()
    {
    }

    public CustomScenarioValue(Type type, object o)
    {
        SerializeType = type;
        Value = o;
    }
    /// <summary>
    /// 实际存储的值的类型，用于序列化的类型
    /// </summary>
    [JsonConverter(typeof(TypeJsonConverter))]
    public Type SerializeType { get; set; }

    [JsonIgnore] private Type? _realType;
    /// <summary>
    /// 实际它对应的类型
    /// </summary>

    [JsonConverter(typeof(TypeJsonConverter))]
    public Type ShowType
    {
        get => _realType ?? SerializeType;
        set => _realType = value;
    }

    [ObservableProperty] private bool _isSelf = false;
    

    [ObservableProperty] private object? value;

    // partial void OnValueChanged(object? value)
    // {
    //     OnPropertyChanged(nameof(Value));
    // }
}