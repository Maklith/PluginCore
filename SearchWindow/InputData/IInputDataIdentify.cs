using System.Collections.Generic;

namespace Core.ViewModel;

public interface IInputDataIdentifier
{
    public IEnumerable<InputData> IdentifyInputData(string? s);
}