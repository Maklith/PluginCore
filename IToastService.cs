using Avalonia.Controls.Notifications;

namespace PluginCore;

/// <summary>
/// 系统Toast服务
/// </summary>
public interface IToastService
{
    public void Init();
    public void Show(string header, string text, NotificationType notificationType = NotificationType.Information);
    public void Show(ToastRequest request);
    public IToastProgressHandle ShowProgress(string header, string text,
        NotificationType notificationType = NotificationType.Information, double initialProgress = 0,
        bool isIndeterminate = false);
    public void Unregister();
}
