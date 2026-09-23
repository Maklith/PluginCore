using System;
using System.Text.Json.Serialization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore;

public enum HotKeyType
{
    Keyboard,
    Mouse
}

public enum HotKeyProcessScope
{
    All,
    Include,
    Exclude
}

/// <summary>
///     快捷键模型
/// </summary>
public partial class HotKeyModel : ObservableObject
{
    [ObservableProperty] [JsonIgnore] 
    private bool _isEnabled;

    [ObservableProperty] [JsonIgnore] 
    [NotifyPropertyChangedFor(nameof(ScopeSummary))]
    private HotKeyType _type =HotKeyType.Keyboard;
    [ObservableProperty] [JsonIgnore] 
    private ushort? _mouseButton = ushort.MaxValue;
    [ObservableProperty] [JsonIgnore] 
    [NotifyPropertyChangedFor(nameof(ScopeSummary))]
    private ushort _pressTimeMillis = 1000;

    [ObservableProperty] [JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ProcessScopeDescription))]
    [NotifyPropertyChangedFor(nameof(ScopeSummary))]
    private HotKeyProcessScope _processScope;

    [ObservableProperty] [JsonIgnore]
    [NotifyPropertyChangedFor(nameof(ProcessScopeDescription))]
    [NotifyPropertyChangedFor(nameof(ScopeSummary))]
    private string[] _processNames = [];

    [ObservableProperty] [JsonIgnore]
    private bool _ignoreTextInput;

    [JsonIgnore]
    public string ProcessScopeDescription => ProcessScope switch
    {
        HotKeyProcessScope.Include => "仅在 " + string.Join("、", ProcessNames) + " 生效",
        HotKeyProcessScope.Exclude => "排除 " + string.Join("、", ProcessNames),
        _ => "所有进程"
    };

    [JsonIgnore]
    public string ScopeSummary => Type == HotKeyType.Mouse
        ? $"{ProcessScopeDescription} · 长按 {PressTimeMillis}ms"
        : ProcessScopeDescription;

    public bool CanExecuteInProcess(string? processName)
    {
        if (ProcessScope == HotKeyProcessScope.All) return true;
        if (string.IsNullOrWhiteSpace(processName)) return false;
        var name = NormalizeProcessName(processName);
        var matches = ProcessNames.Any(process =>
            string.Equals(NormalizeProcessName(process), name, StringComparison.OrdinalIgnoreCase));
        return ProcessScope switch
        {
            HotKeyProcessScope.Include => matches,
            HotKeyProcessScope.Exclude => !matches,
            _ => false
        };
    }

    public static string NormalizeProcessName(string name)
    {
        name = name.Trim().Trim('"').Replace('\\', '/');
        name = name[(name.LastIndexOf('/') + 1)..];
        return name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ? name[..^4] : name;
    }

    public HotKeyModel()
    {
        UUID = Guid.NewGuid().ToString();
    }

    public HotKeyModel(HotKeyModel source)
    {
        UUID = source.UUID;
        MainName = source.MainName;
        Name = source.Name;
        ApplySettings(source);
        ProcessNames = source.ProcessNames.ToArray();
    }

    public void ApplySettings(HotKeyModel source)
    {
        IsSelectCtrl = source.IsSelectCtrl;
        IsSelectAlt = source.IsSelectAlt;
        IsSelectShift = source.IsSelectShift;
        IsSelectWin = source.IsSelectWin;
        SelectKey = source.SelectKey;
        Type = source.Type;
        MouseButton = source.MouseButton;
        PressTimeMillis = source.PressTimeMillis;
        ProcessScope = source.ProcessScope;
        ProcessNames = source.ProcessNames;
        IgnoreTextInput = source.IgnoreTextInput;
        IsEnabled = source.IsEnabled;
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
