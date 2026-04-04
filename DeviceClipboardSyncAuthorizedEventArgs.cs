using System;

namespace PluginCore;

public sealed class DeviceClipboardSyncAuthorizedEventArgs : EventArgs
{
    public DeviceClipboardSyncAuthorizedEventArgs(DeviceModel peer, bool initiatedByPeer)
    {
        Peer = peer;
        InitiatedByPeer = initiatedByPeer;
    }

    public DeviceModel Peer { get; }

    public bool InitiatedByPeer { get; }
}
