using System.ComponentModel;

namespace PluginCore.SearchWindow.InputData;

public enum InputType
{
    [Description("lang.kitopia.text")] 文本,
    [Description("lang.kitopia.search.type.image")] 图像,
    [Description("lang.kitopia.search.type.directory")] 目录,
    [Description("lang.kitopia.search.type.file")] 文件,
    [Description("lang.kitopia.search.type.url")] 网址,
    [Description("lang.kitopia.search.type.command")] 命令,
    [Description("lang.kitopia.search.type.scenario")] 情景,
    [Description("lang.kitopia.search.type.math_expression")] 数学表达式
    
}