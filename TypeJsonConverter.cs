using System;
using System.Collections.Generic;
using System.Text;
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
        var name = reader.GetString();
        
        name = name.Replace("[", ",").Replace("]","");
        var strings = name.Split(",");
        int index = 0;
        var read = ParseType(strings,ref index);
        return read;
       
    }
    public Type ParseType(String[] typeNames,ref int index)
    {
        var typeName = typeNames[index];
        if (typeName.Contains("`"))
        {
            var type = GetType(typeName);
            var types = new List<Type>();
            var length = type.GetGenericArguments().Length;
            while (types.Count<length&& index+1<typeNames.Length)
            {
                index++;
                types.Add(ParseType(typeNames,ref index));
            }

            return type.MakeGenericType(types.ToArray());
        }
        else
        {
            return GetType(typeName);
        }

    
        return null;
    }
    private Type GetType(string typeName)
    {
        var strings = typeName!.Split(" ",2);
        if (strings[0] == "System")
        {
            var type = Type.GetType(strings[1], false, true);
            if (type != null) return type;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type1 in assembly.GetTypes())
                if (type1.FullName == strings[1])
                    return type1;

        }

        var type2 = ServiceManager.Services.GetService<IPluginManger>()!.GetType(strings);
        if (type2 is null)
        {
            
            throw new CustomScenarioLoadFromJsonException(CustomScenarioLoadFromJsonFailedType.类未找到, strings[0],
                strings[1]);
        }
        return type2;
    }

    public override void Write(Utf8JsonWriter writer, Type type, JsonSerializerOptions options)
    {
        writer.WriteStringValue(GetTypeName(type));
    }
    //System List
    public string GetTypeName(Type type)
    {
        var plugin = ServiceManager.Services.GetService<IPluginManger>()!.GetPluginInfo(type);
        var from = plugin is null ? "System" : plugin.ToPlgString();
        var typeName = type.Name;
        if (type.IsGenericType)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < type.GetGenericArguments().Length-1; i++)
            {
                sb.Append(GetTypeName(type.GetGenericArguments()[i]));
                sb.Append(",");
            }
            sb.Append(GetTypeName(type.GetGenericArguments()[^1]));
            return ($"{from} {type.Namespace}.{typeName}[{sb}]");
        }

        return ($"{from} {type.Namespace}.{typeName}");
    }
}