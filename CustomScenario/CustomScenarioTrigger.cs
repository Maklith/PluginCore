using CommunityToolkit.Mvvm.Messaging;

namespace PluginCore;

public class CustomScenarioTrigger
{
    protected static void Excite(string name)
    {
        WeakReferenceMessenger.Default.Send("", "CustomScenarioTrigger");
    }
}