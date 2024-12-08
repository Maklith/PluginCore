using System.Collections.Generic;

namespace PluginCore;

public interface IScreenCaptureManager
{
    public void SetCaptureMethodName(string methodName);
    public List<string> GetCaptureMethodName();
    public List<ScreenCaptureInfo> GetAllScreenInfo();
    
    public ScreenCaptureInfo GetScreenCaptureInfoByIndex(int index);
    public Stack<ScreenCaptureResult> CaptureAllScreenBitmap();
    public Stack<ScreenCaptureResult> CaptureAllScreenBytes();
    public ScreenCaptureResult CaptureScreenBitmap(ScreenCaptureResult captureAllScreenInfo);
    public ScreenCaptureResult CaptureScreenBytes(ScreenCaptureInfo screenCaptureInfo);
}