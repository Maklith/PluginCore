using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PluginCore;

namespace Core.SDKs.Services;

public interface IScreenCaptureWindow
{
    public void CaptureScreen();
    public void RequestUserSelectScreenInfo(Action<ScreenCaptureInfo> action);
    public void RequestUserSelectScreenBytes(Action<ScreenCaptureResult> action, Action cancel );

    public Task<ScreenCaptureInfo> GetScreenCaptureInfo();
}