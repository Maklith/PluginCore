using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace PluginCore;

public class TransferInterruptionEventArgs : EventArgs
{
    public string RequestId { get; }
    public string Reason { get; }
    public bool IsSending { get; }

    public TransferInterruptionEventArgs(string requestId, string reason, bool isSending)
    {
        RequestId = requestId;
        Reason = reason;
        IsSending = isSending;
    }
}
/// <summary>
/// 设备通信服务接口
/// </summary>
public interface IDeviceCommunication
{
    /// <summary>
    /// 已发现的设备列表
    /// </summary>
    ObservableCollection<DeviceModel> DiscoveredDevices { get; }

    /// <summary>
    /// 开始设备发现
    /// </summary>
    void StartDiscovery();

    /// <summary>
    /// 停止设备发现
    /// </summary>
    void StopDiscovery();

    /// <summary>
    /// 发送数据流到指定设备
    /// </summary>
    /// <param name="target">目标设备</param>
    /// <param name="stream">数据流</param>
    /// <param name="metaData">元数据（可选）</param>
    /// <returns></returns>
    Task SendStreamAsync(DeviceModel target, Stream stream, string? metaData = null);

    /// <summary>
    /// 发送文本消息
    /// </summary>
    Task SendMessageAsync(DeviceModel target, string message);

    /// <summary>
    /// 发送剪贴板文本（用于设备间实时同步）
    /// </summary>
    Task SendClipboardTextAsync(DeviceModel target, string text);

    /// <summary>
    /// 请求与目标设备建立剪贴板实时同步（需对方同意）
    /// </summary>
    Task<bool> RequestClipboardSyncAsync(DeviceModel target);

    /// <summary>
    /// 启用与指定设备的双向剪贴板同步（内部会发起同意请求）
    /// </summary>
    Task<bool> EnableClipboardSyncAsync(DeviceModel target);

    /// <summary>
    /// 关闭当前剪贴板同步
    /// </summary>
    void DisableClipboardSync();

    /// <summary>
    /// 当前是否已启用剪贴板同步
    /// </summary>
    bool IsClipboardSyncEnabled { get; }

    /// <summary>
    /// 当前剪贴板同步目标设备
    /// </summary>
    DeviceModel? ClipboardSyncTargetDevice { get; }

    /// <summary>
    /// 请求发送文件（可选传入 filePath，不传时由服务处理文件选择）
    /// </summary>
    Task RequestFileTransferAsync(DeviceModel target, string? filePath = null);

    /// <summary>
    /// 响应文件传输请求
    /// </summary>
    Task RespondToFileRequestAsync(DeviceModel target, string requestId, bool accepted, string? savePath = null);

    /// <summary>
    /// 接收到数据流时触发
    /// </summary>
    event EventHandler<DeviceStreamReceivedEventArgs>? StreamReceived;

    /// <summary>
    /// 接收到文本消息时触发
    /// </summary>
    event EventHandler<DeviceMessageReceivedEventArgs>? MessageReceived;

    /// <summary>
    /// 接收到剪贴板文本时触发
    /// </summary>
    event EventHandler<DeviceClipboardReceivedEventArgs>? ClipboardTextReceived;

    /// <summary>
    /// 剪贴板同步已授权（本机请求被同意，或本机同意了对方请求）时触发
    /// </summary>
    event EventHandler<DeviceClipboardSyncAuthorizedEventArgs>? ClipboardSyncAuthorized;

    /// <summary>
    /// 剪贴板同步状态变化时触发
    /// </summary>
    event EventHandler<DeviceClipboardSyncStateChangedEventArgs>? ClipboardSyncStateChanged;

    /// <summary>
    /// 接收到文件传输请求时触发
    /// </summary>
    event EventHandler<FileTransferRequestEventArgs>? FileTransferRequested;
    
    event EventHandler<TransferInterruptionEventArgs>? TransferInterrupted;
}
