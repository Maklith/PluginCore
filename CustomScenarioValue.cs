using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.JsonConverter;

namespace PluginCore;

public partial class CustomScenarioValue : ObservableObject
{
    public CustomScenarioValue()
    {
    }

    public CustomScenarioValue(Type type, object o)
    {
        Type = type;
        Value = o;
    }
    [JsonConverter(typeof(TypeJsonConverter))]
    public Type Type { get; set; }
    [JsonIgnore] private Type? _realType;
    [JsonConverter(typeof(TypeJsonConverter))]
    public Type RealType
    {
        get => _realType ?? Type;
        set => _realType = value;
    }

    partial void OnValueChanged(object? value)
    {
        // WeakReferenceMessenger.Default.Send(new CustomScenarioChangeMsg()
        //     { ScenarioMethodNode = Source, ConnectorItem = this });
    }


    [ObservableProperty]
    private object? value;


}