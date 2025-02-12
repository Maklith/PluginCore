using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Core.JsonConverter;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace PluginCore;

public class PluginDependency
{
    public string Item1 { get; set; }
    public string Item2 { get; set; }

    public void Deconstruct(out string plgStr, out string verStr)
    {
        plgStr = Item1;
        verStr = Item2;
    }
}

[ObservableObject]
public partial class PluginInfo
{
    public int Id { set; get; }

    public string AuthorName { set; get; }

    public int AuthorId { set; get; }

    public string Name { set; get; }
    public string NameSign { set; get; }
    public bool IsPublic { set; get; }

    public string Version { set; get; }
    public int VersionId { set; get; }

    public string Description { set; get; }

    public string Main { set; get; }
    [JsonIgnore]public string FullPath { set; get; }
    [JsonIgnore]public string Path { set; get; }

    public Dictionary<string, string> Dependencies { set; get; }

    [JsonIgnore] [property: JsonIgnore] [ObservableProperty]
    private Bitmap? _icon;

    [JsonIgnore][ObservableProperty] public bool isEnabled;
    [JsonIgnore][ObservableProperty] public bool unloadFailed;

    [property: JsonIgnore][JsonIgnore][ObservableProperty] private bool canUpdata;
    [property: JsonIgnore][JsonIgnore][ObservableProperty] private string canUpdateVersion;
    [property: JsonIgnore][JsonIgnore][ObservableProperty] private int canUpdateVersionId;
    [JsonIgnore] public int UpdateTargetVersion { set; get; }

    public string ToPlgString()
    {
        return $"{Id}_{AuthorId}_{NameSign}";
    }

    public override string ToString()
    {
        return ToPlgString();
    }
}