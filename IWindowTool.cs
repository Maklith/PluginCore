using System;
using Avalonia.Controls;

namespace PluginCore;

public interface IWindowTool
{
    void SetForegroundWindow(IntPtr hWnd);
    void MoveWindowToMouseScreenCenter(Window window);
    void SetWindowTopMost(IntPtr hWnd, bool topMost);
    void SelectAndSetWindowTopMost();
}