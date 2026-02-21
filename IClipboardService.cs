#region

using System.Threading.Tasks;
using OpenCvSharp;

#endregion

namespace PluginCore;

/// <summary>
/// 剪贴板服务
/// </summary>
public interface IClipboardService
{
    bool HasText();
    string? GetText();
    bool SetText(string text);
    bool HasImage();
    Mat? GetImage();
    Task<bool> SetImageAsync(ScreenCaptureResult screenCaptureResult);
}