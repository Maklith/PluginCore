

namespace PluginCore;
/// <summary>
/// 系统Toast服务
/// </summary>
public interface IToastService
{
    public void Init();
    public void Show(string header, string text);
    public void Unregister();
}