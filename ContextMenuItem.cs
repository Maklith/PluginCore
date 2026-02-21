// Author: liaom
// SolutionName: Kitopia
// ProjectName: Core
// FileName:ContextMenuItem.cs
// Date: 2026/01/17 21:01
// FileEffect:

using System.Collections.Generic;

namespace PluginCore;

public class ContextMenuItem
{
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string Arguments { get; set; } = string.Empty;
    public List<ContextMenuItem> SubItems { get; set; } = new();
}