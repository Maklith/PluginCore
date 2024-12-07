using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.Media.Imaging;

namespace PluginCore;

public struct ScreenCaptureInfo()
{
    public uint Index = 0;
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
    public IntPtr hMonitor;
    public IntPtr hdcMonitor;
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
    public ScreenCaptureResult CaptureScreenBitmap(ScreenCaptureInfo screenCaptureInfo);
    public ScreenCaptureResult CaptureScreenBytes(ScreenCaptureInfo screenCaptureInfo);
}