using System;
using System.Collections.Generic;
using PluginCore;

namespace KitopiaEx;

public class ObservableValue : IObservable<CustomScenarioValue>
{
    public ObservableValue()
    {
        observers = new List<IObserver<CustomScenarioValue>>();
    }

    private List<IObserver<CustomScenarioValue>> observers;

    public CustomScenarioValue Value { get; init; }
    
    public IDisposable Subscribe(IObserver<CustomScenarioValue> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
        return new Unsubscriber(observers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<CustomScenarioValue>> _observers;
        private IObserver<CustomScenarioValue> _observer;

        public Unsubscriber(List<IObserver<CustomScenarioValue>> observers, IObserver<CustomScenarioValue> observer)
        {
            this._observers = observers;
            this._observer = observer;
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
        foreach (var observer in observers)
        {
            observer.OnNext(Value);
        }
    }
    public CustomScenarioValue GetValue()
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