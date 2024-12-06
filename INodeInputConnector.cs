using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Styling;
using KitopiaEx;

namespace PluginCore;

public interface INodeInputConnector
{
    public StyleInclude Style { get; }
    public IDataTemplate IDataTemplate { get; }
    public ObservableValue Value { get; set; }
}