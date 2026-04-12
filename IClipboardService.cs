#region

using System.Collections.Generic;
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
    bool HasFiles();
    IReadOnlyList<string> GetFiles();
    bool HasImage();
    Mat? GetImage();
    Task<bool> SetImageAsync(ScreenCaptureResult screenCaptureResult);
}
