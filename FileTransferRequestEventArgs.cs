using System;

namespace PluginCore;

public class FileTransferRequestEventArgs : EventArgs
{
    public string RequestId { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public DeviceModel Sender { get; }

    public FileTransferRequestEventArgs(string requestId, string fileName, long fileSize, DeviceModel sender)
    {
        RequestId = requestId;
        FileName = fileName;
        FileSize = fileSize;
        Sender = sender;
    }
}

