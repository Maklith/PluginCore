using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media.Imaging;

namespace PluginCore;

public enum ScreenCaptureType
{
    屏幕,
    窗口
}
public struct ScreenCaptureInfo()
{
    public ScreenCaptureType ScreenCaptureType = ScreenCaptureType.屏幕;
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
    public ScreenInfo ScreenInfo;
    public WindowInfo WindowInfo;
}

public struct ScreenInfo()
{
    public IntPtr hMonitor;
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
}

public struct WindowInfo()
{
    public string Title;
    public IntPtr Hwnd;
    public Rect Rect;
    public int ZIndex;
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
    public List<WindowInfo> GetAllWindowInfo();
    
    public ScreenCaptureInfo GetScreenCaptureInfoByIndex(int index);
    public Stack<ScreenCaptureResult> CaptureAllScreenBitmap();
    public Stack<ScreenCaptureResult> CaptureAllScreenBytes();
    public ScreenCaptureResult CaptureScreenBitmap(ScreenCaptureResult captureAllScreenInfo);
    public ScreenCaptureResult CaptureScreenBytes(ScreenCaptureInfo screenCaptureInfo);
}