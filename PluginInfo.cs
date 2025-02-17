using System;
using System.Collections.Generic;
using System.Drawing;
using Core.JsonConverter;

namespace PluginCore;

public struct PluginBaseInfo : IEquatable<PluginBaseInfo>
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
    public Dictionary<string, string> Dependencies { set; get; }
    public string ToPlgString()
    {
        return NameSign;
    }

    public override string ToString()
    {
        return NameSign;
    }

    public override int GetHashCode()
    {
        return NameSign.GetHashCode();
    }

    public bool Equals(PluginBaseInfo other)
    {
        return NameSign == other.NameSign;
    }

    public override bool Equals(object? obj)
    {
        return NameSign == obj?.ToString();
    }
}