using CommunityToolkit.Mvvm.ComponentModel;

namespace Core.ViewModel;

public partial class InputData : ObservableObject
{
    [ObservableProperty]
    private InputType _inputType;

    [ObservableProperty] 
    private object _data;
}