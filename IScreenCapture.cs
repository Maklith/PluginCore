using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace PluginCore;

public enum ScreenCaptureType
{
    屏幕,
    窗口
}
public record struct ScreenCaptureInfo()
{
    public ScreenCaptureType ScreenCaptureType = ScreenCaptureType.屏幕;
    public Rect? RequestRect;//相对于图像
    public Rect? ScreenInfo;//用于查询显示器指针
    public IntPtr hMonitor;//显示器指针，窗口情况下用窗口查询，屏幕用screenInfo查询
    public float SdrWhiteLevelScale = 1.0f;//HDR显示器的SDR白电平缩放，默认为1.0f，表示不缩放
    public WindowInfo? WindowInfo;
}

public record struct WindowInfo {
    public string Title;
    public string ModuleFileName;
    public IntPtr Hwnd;
    public Rect Rect;
    public int ZIndex;
}

public record struct Rect(int X, int Y, int Width, int Height)
{
    
    public int X = X;
    public int Y = Y;
    public int Width = Width;
    public int Height = Height;
}
public struct ScreenCaptureResult
{
    public Mat? Source; 
    public ScreenCaptureInfo Info;
}

public interface IScreenCapture
{
    public List<ScreenCaptureInfo> GetAllScreenInfo();
    public List<WindowInfo> GetAllWindowInfo();
    
    public ScreenCaptureInfo GetScreenCaptureInfoByIndex(int index);
    public Stack<ScreenCaptureResult> CaptureAllScreenMat();
    public ScreenCaptureResult CaptureScreenMat(ScreenCaptureInfo screenCaptureInfo);
}