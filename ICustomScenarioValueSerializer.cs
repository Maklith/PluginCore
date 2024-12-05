using System;

namespace Core.SDKs.CustomScenario;

public interface ICustomScenarioValueSerializer
{
    public string Serialize<T>(T value);
    
    public object Deserialize(ReadOnlySpan<byte> value);
}