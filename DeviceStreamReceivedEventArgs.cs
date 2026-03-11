using System;
using System.IO;

namespace PluginCore;

public class DeviceStreamReceivedEventArgs : EventArgs
{
    public DeviceModel Sender { get; }
    public Stream Stream { get; }
    public string? MetaData { get; }

    public DeviceStreamReceivedEventArgs(DeviceModel sender, Stream stream, string? metaData = null)
    {
        Sender = sender;
        Stream = stream;
        MetaData = metaData;
    }
}


