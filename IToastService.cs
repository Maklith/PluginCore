using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace PluginCore;

/// <summary>
/// 系统Toast服务
/// </summary>
public interface IToastService
{
    public void Init();
    public Task Show(string header, string text, NotificationType notificationType = NotificationType.Information,
        Window? dialogWindow = null);
    public Task Show(ToastRequest request, Window? dialogWindow = null);
    public IToastProgressHandle ShowProgress(string header, string text,
        NotificationType notificationType, double initialProgress = 0,
        bool isIndeterminate = false);
    public bool HasUnreadSuppressedNotifications();
    public bool TryOpenLatestSuppressedNotification();
    public bool ShowSuppressedNotificationCenter();
    public void ClearUnreadSuppressedNotifications();
    public void Unregister();
}
