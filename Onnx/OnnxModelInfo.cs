using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PluginCore.Onnx;


public partial class OnnxModelInfo :ObservableObject
{
    public CancellationTokenSource _cancellationTokenSource;
    private bool _needDownload;
    private bool _canDownload;
    private ICommand _downloadCommand;
    private bool _isIndeterminate;
    private bool _isDownloading;
    private double _progress;
    private ICommand _cancelCommand;
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string SignName { get; set; }
    public string ModelPath { get; set; }
    public IReadOnlyList<string> RequiredFiles { get; set; } = [];
    public bool IsBundled { get; set; }

    public OnnxModelInfo()
    {
        
    }

    public bool NeedDownload
    {
        get
        {
            if (!File.Exists(ModelPath))
            {
                return true;
            }

            foreach (var requiredFile in RequiredFiles)
            {
                if (!File.Exists(requiredFile))
                {
                    return true;
                }
            }

            return false;
        }
    }
    public void NotifyNeedDownload()
    {
        OnPropertyChanged(nameof(NeedDownload));
        OnPropertyChanged(nameof(ModelPath));
    }


    public bool CanDownload
    {
        get => _canDownload;
        set => SetProperty(ref _canDownload,value);
    }

    public ICommand DownloadCommand
    {
        get => _downloadCommand;
        set => SetProperty(ref _downloadCommand,value);
    }

    public ICommand CancelCommand {
        get => _cancelCommand;
        set => SetProperty(ref _cancelCommand,value);
    }

    public bool IsIndeterminate
    {
        get => _isIndeterminate;
        set =>  SetProperty(ref _isIndeterminate,value);
    }

    public bool IsDownloading
    {
        get => _isDownloading;
        set =>  SetProperty(ref _isDownloading,value);
    }

    public double Progress
    {
        get => _progress;
        set =>  SetProperty(ref _progress,value);
    }
}
