using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;

namespace PluginCore;

public struct ScreenCaptureInfo()
{
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
    public ScreenInfo ScreenInfo;
}

public struct ScreenInfo()
{
    public IntPtr hMonitor;
    public IntPtr hdcMonitor;
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
}
public struct ScreenCaptureResult
{
    public Bitmap Source;
    public byte[] Bytes;
    public ScreenCaptureInfo Info;
}

public interface IScreenCapture
{
    public List<ScreenCaptureInfo> GetAllScreenInfo();
    
    public ScreenCaptureInfo GetScreenCaptureInfoByIndex(int index);
    public Stack<ScreenCaptureResult> CaptureAllScreenBitmap();
    public Stack<ScreenCaptureResult> CaptureAllScreenBytes();
    public ScreenCaptureResult CaptureScreenBitmap(ScreenCaptureResult captureAllScreenInfo);
    public ScreenCaptureResult CaptureScreenBytes(ScreenCaptureInfo screenCaptureInfo);
}