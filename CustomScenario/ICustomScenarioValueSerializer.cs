namespace PluginCore.CustomScenario;
/// <summary>
/// 情景节点类型序列化接口,用于自定义类型的序列化<see cref="Kitopia.JsonConverters"/>中注册
/// </summary>
/// 
public interface ICustomScenarioValueSerializer
{
    public string Serialize<T>(T value);

    public object? Deserialize(string? value);
}