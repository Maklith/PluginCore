using System;
using System.Net;

namespace PluginCore;

public class DeviceModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public IPAddress Address { get; set; } = IPAddress.None;
    public int Port { get; set; }
    public DateTime LastSeen { get; set; }
    
    public override string ToString() => $"{Name} ({Address}:{Port})";
}

