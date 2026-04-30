using System.Threading.Tasks;
using Avalonia.Controls.Notifications;

namespace PluginCore;

/// <summary>
/// 系统Toast服务
/// </summary>
public interface IToastService
{
    public void Init();
    public Task Show(string header, string text, NotificationType notificationType = NotificationType.Information);
    public Task Show(ToastRequest request);
    public IToastProgressHandle ShowProgress(string header, string text,
        NotificationType notificationType, double initialProgress = 0,
        bool isIndeterminate = false);
    public void Unregister();
}
