using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore.SearchWindow.InputData;

public partial class InputData : ObservableObject,IDisposable
{
    [ObservableProperty]
    private InputType _inputType;

    [ObservableProperty] 
    private object _data;
    public Action<InputData>? DisposeAction { set; get; }

    public void Dispose()
    {
        DisposeAction?.Invoke(this);
    }
    ~InputData()
    {
        Dispose();
    }
}