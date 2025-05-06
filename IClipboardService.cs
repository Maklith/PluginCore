#region

using System.Threading.Tasks;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

#endregion

namespace PluginCore;
/// <summary>
/// 剪贴板服务
/// </summary>
public interface IClipboardService
{
    bool HasText();
    string GetText();
    bool SetText(string text);
    bool HasImage();
    Bitmap? GetImage();
    bool SetImage(Bitmap image);
    Task<bool> SetImageAsync(ScreenCaptureResult screenCaptureResult);
}

