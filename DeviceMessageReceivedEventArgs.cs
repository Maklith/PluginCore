using System;

namespace PluginCore;

public sealed class DeviceMessageReceivedEventArgs : EventArgs
{
    public DeviceMessageReceivedEventArgs(DeviceModel sender, string message)
    {
        Sender = sender;
        Message = message;
    }

    public DeviceModel Sender { get; }

    public string Message { get; }
}
