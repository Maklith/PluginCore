using System;
using System.Collections.Generic;

namespace PluginCore;

public class ObservableValue : IObservable<CustomScenario.CustomScenarioValue>
{
    public ObservableValue()
    {
        observers = new List<IObserver<CustomScenario.CustomScenarioValue>>();
    }

    private List<IObserver<CustomScenario.CustomScenarioValue>> observers;

    public CustomScenario.CustomScenarioValue Value { get; init; }

    public IDisposable Subscribe(IObserver<CustomScenario.CustomScenarioValue> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
        return new Unsubscriber(observers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<CustomScenario.CustomScenarioValue>> _observers;
        private IObserver<CustomScenario.CustomScenarioValue> _observer;

        public Unsubscriber(List<IObserver<CustomScenario.CustomScenarioValue>> observers, IObserver<CustomScenario.CustomScenarioValue> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }


    public void SetValue(object? loc)
    {
        Value.Value = loc;
        foreach (var observer in observers) observer.OnNext(Value);
    }

    public CustomScenario.CustomScenarioValue GetValue()
    {
        return Value;
    }

    public void EndTransmission()
    {
        foreach (var observer in observers.ToArray())
            if (observers.Contains(observer))
                observer.OnCompleted();

        observers.Clear();
    }
}