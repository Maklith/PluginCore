using System;
using System.Collections.Generic;
using Core.ViewModel;

namespace PluginCore.SearchWindow.InputDataAnalyzer;
[Flags]
public enum IInputDataAnalyzeTimeFlags
{
    仅第一次打开时= 0,
    仅有搜索内容打开时= 1,
    搜索前= 2,
    搜索时= 4,
    仅用作文本索引=0b1000,
    
}
public interface IInputDataAnalyzer
{
    public IInputDataAnalyzeTimeFlags AnalyzeTimeFlags { get; }    
    public IEnumerable<SearchViewItem> AnalyzeInputData(IEnumerable<InputData.InputData> inputDatas);
}