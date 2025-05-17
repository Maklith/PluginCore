using System.Collections.Generic;
using Core.ViewModel;

namespace PluginCore.SearchWindow.InputDataAnalyzer;

public interface IInputDataAnalyzer
{
    public IEnumerable<SearchViewItem> AnalyzeInputData(IEnumerable<InputData> inputDatas);
}