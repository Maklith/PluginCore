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
    /// <summary>Sets topmost state without activating the window when overlays are disabled.</summary>
    void SetWindowTopMost(IntPtr hWnd, bool topMost, bool showOverlay) =>
        throw new NotSupportedException("This window service does not support explicit topmost overlay options.");
    void SelectAndSetWindowTopMost();
    IEnumerable<WindowInfo> GetAllWindows();
    Bitmap? GetWindowIcon(IntPtr hWnd);
    Window? GetForegroundWindow();
}
