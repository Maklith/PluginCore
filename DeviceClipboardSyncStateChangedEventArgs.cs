using System;

namespace PluginCore;

public sealed class DeviceClipboardSyncStateChangedEventArgs : EventArgs
{
    public DeviceClipboardSyncStateChangedEventArgs(bool isEnabled, DeviceModel? targetDevice, string status)
    {
        IsEnabled = isEnabled;
        TargetDevice = targetDevice;
        Status = status;
    }

    public bool IsEnabled { get; }

    public DeviceModel? TargetDevice { get; }

    public string Status { get; }
}
