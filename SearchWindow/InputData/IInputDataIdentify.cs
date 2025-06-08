using System.Collections.Generic;
using PluginCore.SearchWindow.InputDataAnalyzer;

namespace Core.ViewModel;

public interface IInputDataIdentifier
{
    public IEnumerable<InputData> IdentifyInputData(IInputDataAnalyzeTimeFlags analyzeTimeFlags,string? s);
}