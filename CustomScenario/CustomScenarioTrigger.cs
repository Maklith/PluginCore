using CommunityToolkit.Mvvm.Messaging;

namespace PluginCore.CustomScenario;

public class CustomScenarioTrigger
{
    protected static void Excite(string name)
    {
        WeakReferenceMessenger.Default.Send(name, "CustomScenarioTrigger");
    }

    protected static void Excite<TTrigger>() where TTrigger : CustomScenarioTrigger
    {
        WeakReferenceMessenger.Default.Send(typeof(TTrigger), "CustomScenarioTrigger");
    }
}
