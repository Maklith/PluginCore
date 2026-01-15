using System.Collections.Generic;
using PluginCore.SearchWindow.InputDataAnalyzer;

namespace Core.ViewModel;

public interface IInputDataIdentifier
{
    public IEnumerable<PluginCore.SearchWindow.InputData.InputData> IdentifyInputData(InputDataAnalyzeTimeFlags analyzeTimeFlags,string? s);
}