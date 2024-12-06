using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.SDKs.CustomScenario;
using Core.SDKs.Services;
using Microsoft.Extensions.DependencyInjection;
using PluginCore;


namespace Core.JsonConverter;

public class TypeJsonConverter : JsonConverter<Type>
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Type);
    }

    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeName = reader.GetString();
        var strings = typeName!.Split(" ");
        if (strings[0] == "System")
        {
            var type = Type.GetType(strings[1], false, true);
            if (type != null) return type;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type1 in assembly.GetTypes())
                {
                    if (type1.FullName == strings[1])
                    {
                        return type1;
                    }
                }
            }

            throw new CustomScenarioLoadFromJsonException(CustomScenarioLoadFromJsonFailedType.类未找到,strings[0], strings[1]);
        }

        return ServiceManager.Services.GetService<IPluginManger>()!.GetType(strings);
    }

    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        var plugin = ServiceManager.Services.GetService<IPluginManger>()!.GetPluginInfo(type);
        if (plugin is null)
        {
            writer.WriteStringValue($"System {type.FullName}");
            return;
        }

        writer.WriteStringValue($"{plugin} {type.FullName}");
    }
}