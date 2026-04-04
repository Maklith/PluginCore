using System;

namespace PluginCore;

public sealed class DeviceClipboardReceivedEventArgs : EventArgs
{
    public DeviceClipboardReceivedEventArgs(DeviceModel sender, string text)
    {
        Sender = sender;
        Text = text;
    }

    public DeviceModel Sender { get; }

    public string Text { get; }
}
