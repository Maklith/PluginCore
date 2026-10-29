#region

using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

#endregion

namespace PluginCore;

public partial class SearchViewItem : ObservableObject
{
    [ObservableProperty] private Bitmap? _icon;
    [ObservableProperty] private bool _isStared = false;
    private string? _startDirectory;

    [ObservableProperty] public bool[]? pinyinItem;

    public required string ItemDisplayName { get; set; }

    public bool? IsVisible { set; get; }

    [ObservableProperty] private bool _isPined = false;
    [ObservableProperty] private long? _fileSize;
    [ObservableProperty] private DateTime? _dateModified;
    [ObservableProperty] private bool _isFolder;

    public string FormattedFileSize
    {
        get
        {
            if (IsFolder || FileSize == null) return string.Empty;
            return FormatBytes(FileSize.Value);
        }
    }

    public string FormattedDateModified
    {
        get
        {
            if (DateModified == null) return string.Empty;
            return DateModified.Value.ToString("yyyy/M/d HH:mm");
        }
    }

    partial void OnFileSizeChanged(long? value)
    {
        OnPropertyChanged(nameof(FormattedFileSize));
    }

    partial void OnDateModifiedChanged(DateTime? value)
    {
        OnPropertyChanged(nameof(FormattedDateModified));
    }

    partial void OnIsFolderChanged(bool value)
    {
        OnPropertyChanged(nameof(FormattedFileSize));
    }

    public static string FormatBytes(long bytes)
    {
        if (bytes < 0) return string.Empty;
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024L * 1024) return $"{(bytes / 1024.0):F1} KB";
        if (bytes < 1024L * 1024 * 1024) return $"{(bytes / (1024.0 * 1024.0)):F1} MB";
        if (bytes < 1024L * 1024 * 1024 * 1024) return $"{(bytes / (1024.0 * 1024.0 * 1024.0)):F2} GB";
        return $"{(bytes / (1024.0 * 1024.0 * 1024.0 * 1024.0)):F2} TB";
    }

    //给插件使用，固定项目和历史项目不适用
    public bool ShowAsMiniApp { set; get; } = false;
    public FileType FileType { set; get; }

    public string? StartDirectory
    {
        set => _startDirectory = value;
        get
        {
            if (_startDirectory == null)
            {
                return OnlyKey.LastIndexOf('\\') != -1 ? OnlyKey.Remove(OnlyKey.LastIndexOf('\\')) : OnlyKey;
            }

            return _startDirectory;
        }
    }

    public string? Arguments { set; get; }

    public string? LaunchPath { set; get; }

    public string OnlyKey { set; get; } = "";

    public int IconSymbol { set; get; }


    public string? IconPath { set; get; }

    public Action<SearchViewItem?, string?>? Action { set; get; }

    public Func<SearchViewItem, Bitmap>? GetIconAction { set; get; }


    public void Notify()
    {
        OnPropertyChanged(nameof(PinyinItem));
    }
}

public enum FileType
{
    [Description("lang.kitopia.search.type.application")] 应用程序,
    [Description("lang.kitopia.search.type.url")] URL,
    [Description("lang.kitopia.search.type.word")] Word文档,
    [Description("lang.kitopia.search.type.powerpoint")] PPT文档,
    [Description("lang.kitopia.search.type.excel")] Excel文档,
    [Description("lang.kitopia.search.type.pdf")] PDF文档,
    [Description("lang.kitopia.search.type.image")] 图像,
    [Description("lang.kitopia.search.type.clipboard_image")] 剪贴板图像,
    [Description("lang.kitopia.search.type.folder")] 文件夹,
    [Description("lang.kitopia.search.type.file")] 文件,
    [Description("lang.kitopia.search.type.command")] 命令,
    [Description("lang.kitopia.search.type.math")] 数学运算,
    [Description("lang.kitopia.search.type.uwp")] UWP应用,
    [Description("lang.kitopia.search.type.custom_scenario")] 自定义情景,
    [Description("lang.kitopia.search.type.note")] 便签,
    [Description("lang.kitopia.search.type.custom")] 自定义,
    [Description("lang.kitopia.search.type.window")] 窗口,
    [Description("lang.kitopia.text")] 文本,
    [Description("lang.kitopia.search.type.control_panel")] 控制面板,
    [Description("lang.kitopia.none")] None
}
