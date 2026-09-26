namespace PluginCore.CustomScenario;

public struct CustomScenarioTriggerInfo
{
    public string PluginInfo { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public System.Type? TriggerType { get; set; }
}
