using System;
using System.Threading.Tasks;

namespace PluginCore;

public interface IScreenCaptureWindow
{
    public void CaptureScreen();
    public void RequestUserSelectScreenInfo(Action<ScreenCaptureInfo> action);
    public void RequestUserSelectScreenBytes(Action<ScreenCaptureResult> action, Action cancel );

    public Task<ScreenCaptureInfo> GetScreenCaptureInfo();
}