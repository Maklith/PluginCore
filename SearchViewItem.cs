#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinyin.NET;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

#endregion

namespace PluginCore;

public partial class SearchViewItem : ObservableObject, IDisposable
{
    [ObservableProperty] private Bitmap? _icon;
    [ObservableProperty] public bool _isStared = false;
    private string? _startDirectory;

    public string? ItemDisplayName { get; set; }

    public bool? IsVisible { set; get; }

    public bool IsPined { set; get; } = false;

    [ObservableProperty] public PinyinItem? pinyinItem;


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

    public string OnlyKey { set; get; } = "";

    public int IconSymbol { set; get; }


    public string? IconPath { set; get; }

    public Action<SearchViewItem?,string?>? Action { set; get; }

    public Func<SearchViewItem, Bitmap>? GetIconAction { set; get; }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Notify()
    {
        OnPropertyChanged(nameof(PinyinItem));
    }
}

public enum FileType
{
    应用程序,
    URL,
    Word文档,
    PPT文档,
    Excel文档,
    PDF文档,
    图像,
    剪贴板图像,
    文件夹,
    文件,
    命令,
    数学运算,
    UWP应用,
    自定义情景,
    便签,
    自定义,
    文本,
    None
}