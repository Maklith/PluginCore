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
    public float SdrWhiteLevelScale = 1.0f;
}

public struct WindowInfo()
{
    public string Title;
    public string ModuleFileName;
    public IntPtr Hwnd;
    public Rect Rect;
    public int ZIndex;
}

public struct Rect(int x, int y, int width, int height)
{
    
    public int X = x;
    public int Y = y;
    public int Width = width;
    public int Height = height;
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