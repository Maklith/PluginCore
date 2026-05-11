using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore;

public enum HotKeyType
{
    Keyboard,
    Mouse
}

/// <summary>
///     快捷键模型
/// </summary>
public partial class HotKeyModel : ObservableObject
{
    [ObservableProperty] [JsonIgnore] 
    private bool _isEnabled;

    [ObservableProperty] [JsonIgnore] 
    private HotKeyType _type =HotKeyType.Keyboard;
    [ObservableProperty] [JsonIgnore] 
    private ushort? _mouseButton = ushort.MaxValue;
    [ObservableProperty] [JsonIgnore] 
    private ushort _pressTimeMillis = 1000;

    public HotKeyModel()
    {
        UUID = Guid.NewGuid().ToString();
    }

    [Obsolete("此方法仅供Json反序列化使用")]
    public HotKeyModel(string uuid)
    {
        UUID = uuid;
    }

    public string UUID { get; init; }
    public string? MainName { get; init; }

    /// <summary>
    ///     设置项名称
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    ///     是否勾选Ctrl按键
    /// </summary>
    [JsonIgnore] [ObservableProperty] private bool _isSelectCtrl;

    /// <summary>
    ///     是否勾选Shift按键
    /// </summary>
    [JsonIgnore] [ObservableProperty] private bool _isSelectShift;

    /// <summary>
    ///     是否勾选Alt按键
    /// </summary>
    [JsonIgnore] [ObservableProperty] private bool _isSelectAlt;

    /// <summary>
    ///     是否勾选Alt按键
    /// </summary>
    [JsonIgnore] [ObservableProperty] private bool _isSelectWin;

    /// <summary>
    ///     选中的按键
    /// </summary>
    [JsonIgnore] [ObservableProperty] private EKey _selectKey;


    


    [JsonIgnore] public string SignName => $"{MainName}_{Name}";
}