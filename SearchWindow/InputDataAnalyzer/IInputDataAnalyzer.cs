using System;
using System.Collections.Generic;

namespace PluginCore.SearchWindow.InputDataAnalyzer;
[Flags]
public enum InputDataAnalyzeTimeFlags
{
    None = 0,
    
    /// <summary>
    /// 插件加载时调用，用于注册静态项目到索引中。
    /// </summary>
    PluginLoad = 1,

    /// <summary>
    /// 搜索窗口显示时调用，直接显示在列表中。
    /// </summary>
    WindowShow = 2,

    /// <summary>
    /// 搜索内容为空时调用，直接显示在列表中。
    /// </summary>
    InputEmpty = 4,

    /// <summary>
    /// 搜索内容改变时调用，直接显示在列表中。
    /// </summary>
    InputChanged = 8,

    /// <summary>
    /// 每次窗口打开会添加一些新的索引，并且删除之前的索引。
    /// </summary>
    WindowOpenUpdateIndex = 16,
}
public interface IInputDataAnalyzer
{
    public InputDataAnalyzeTimeFlags AnalyzeTimeFlags { get; }    
    public IEnumerable<SearchViewItem> AnalyzeInputData(IEnumerable<InputData.InputData> inputDatas);
}