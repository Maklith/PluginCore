using System;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore;

public partial class DeviceModel : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _customName = string.Empty;

    [ObservableProperty]
    private IPAddress _address = IPAddress.None;

    [ObservableProperty]
    private int _port;

    [ObservableProperty]
    private DateTime _lastSeen;

    public string ComputerName => string.IsNullOrWhiteSpace(Name) ? "未知设备" : Name;

    public string DisplayName => string.IsNullOrWhiteSpace(CustomName) ? ComputerName : $"{CustomName} ({ComputerName})";

    partial void OnNameChanged(string value)
    {
        OnPropertyChanged(nameof(ComputerName));
        OnPropertyChanged(nameof(DisplayName));
    }

    partial void OnCustomNameChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
    }

    public override string ToString() => $"{DisplayName} ({Address}:{Port})";
}
