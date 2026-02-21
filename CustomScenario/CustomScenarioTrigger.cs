using CommunityToolkit.Mvvm.Messaging;

namespace PluginCore.CustomScenario;

public class CustomScenarioTrigger
{
    protected static void Excite(string name)
    {
        WeakReferenceMessenger.Default.Send("", "CustomScenarioTrigger");
    }
}