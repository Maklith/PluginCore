using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace PluginCore;

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
    /// 请求发送文件
    /// </summary>
    Task RequestFileTransferAsync(DeviceModel target, string filePath);

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
    event EventHandler<string>? MessageReceived;

    /// <summary>
    /// 接收到文件传输请求时触发
    /// </summary>
    event EventHandler<FileTransferRequestEventArgs>? FileTransferRequested;
}
