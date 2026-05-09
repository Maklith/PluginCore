using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Window = Avalonia.Controls.Window;

namespace PluginCore;

public interface IWindowTool
{
    void SetForegroundWindow(IntPtr hWnd);
    void MoveWindowToMouseScreenCenter(Window window);
    void SetWindowTopMost(IntPtr hWnd, bool topMost);
    void SelectAndSetWindowTopMost();
    IEnumerable<WindowInfo> GetAllWindows();
    Bitmap? GetWindowIcon(IntPtr hWnd);
    Window? GetForegroundWindow();
}