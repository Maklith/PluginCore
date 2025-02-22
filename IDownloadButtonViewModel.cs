using System.Windows.Input;

namespace PluginCore;

public interface IDownloadButtonViewModel
{
    public bool NeedDownload { get;  }
    public bool CanDownload { get;  }
    public ICommand DownloadCommand { get; }
    
    public ICommand CancelCommand { get; }
    public bool IsIndeterminate { get;  }
    public bool IsDownloading { get;  }
    public double Progress { get;  }
}