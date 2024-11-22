using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace PluginCore;

public struct ScreenCaptureInfo()
{
    public int Index = 0;
    public int X = -1;
    public int Y = -1;
    public int Width = -1;
    public int Height = -1;
}

public struct ScreenCaptureResult
{
    public Bitmap Source;
    public ScreenCaptureInfo Info;
}
public interface IScreenCapture
{
    public Stack<ScreenCaptureResult> CaptureAllScreen();


    public (Bitmap?, Bitmap?)? CaptureScreen(ScreenCaptureInfo screenCaptureInfo, bool withMosaic = false);

    public ScreenCaptureInfo GetScreenCaptureInfoByUserManual();
}