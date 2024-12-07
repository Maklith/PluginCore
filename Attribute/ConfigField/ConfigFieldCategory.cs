namespace PluginCore.Attribute;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ConfigFieldCategory : System.Attribute
{
    public string Category { get; set; }

    public ConfigFieldCategory(string category)
    {
        Category = category;
    }
}