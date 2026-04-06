using System;

namespace PluginCore;

public enum DeviceCommunicationEventType
{
    MessageReceived,
    ClipboardTextReceived,
    ClipboardSyncAuthorized,
    ClipboardSyncStateChanged,
    FileTransferRequested,
    FileTransferProgress,
    FileTransferCompleted,
    TransferInterrupted
}

public sealed class FileTransferProgressEventArgs : EventArgs
{
    public FileTransferProgressEventArgs(
        string requestId,
        string fileName,
        long transferredBytes,
        long totalBytes,
        bool isSending,
        DeviceModel peer)
    {
        RequestId = requestId;
        FileName = fileName;
        TransferredBytes = transferredBytes;
        TotalBytes = totalBytes;
        IsSending = isSending;
        Peer = peer;
    }

    public string RequestId { get; }
    public string FileName { get; }
    public long TransferredBytes { get; }
    public long TotalBytes { get; }
    public bool IsSending { get; }
    public DeviceModel Peer { get; }
}

public sealed class FileTransferCompletedEventArgs : EventArgs
{
    public FileTransferCompletedEventArgs(
        string requestId,
        string fileName,
        long fileSize,
        string filePath,
        bool isSending,
        DeviceModel peer)
    {
        RequestId = requestId;
        FileName = fileName;
        FileSize = fileSize;
        FilePath = filePath;
        IsSending = isSending;
        Peer = peer;
    }

    public string RequestId { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public string FilePath { get; }
    public bool IsSending { get; }
    public DeviceModel Peer { get; }
}

public sealed class DeviceCommunicationEventArgs : EventArgs
{
    public DeviceCommunicationEventArgs(DeviceCommunicationEventType type, EventArgs payload)
    {
        Type = type;
        Payload = payload;
    }

    public DeviceCommunicationEventType Type { get; }

    public EventArgs Payload { get; }
}
