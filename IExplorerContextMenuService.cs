using System.Threading.Tasks;

namespace PluginCore;

public interface IExplorerContextMenuService
{
    Task<bool> RegisterAsync();
    Task<bool> UnregisterAsync();
}
