using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ood_project1;

public interface IObserver
{
    public void UpdateID(IDUpdateArgs args, Log log);
    public void UpdatePosition(PositionUpdateArgs args, Log log);
    public void UpdateContactInfo(ContactInfoUpdateArgs args, Log log);
}

public interface ISubject
{
    public void Subscribe(IObserver observer);
    public void Unsubscribe(IObserver observer);
    public void NotifyIDUpdate(IDUpdateArgs args, Log log);
    public void NotifyPositionUpdate(PositionUpdateArgs args, Log log);
    public void NotifyContactInfoUpdate(ContactInfoUpdateArgs args, Log log);
}
public class Subject: ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    public void AddAllObervers(List<Object> newObservers)
    {
        foreach (var obj in newObservers)
        {
            if (obj is IObserver observer)
                Subscribe(observer);
        }
    }
    public void Subscribe(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyIDUpdate(IDUpdateArgs args, Log log)
    {
        foreach (var observer in observers)
        {
            observer.UpdateID(args, log);
        }
    }

    public void NotifyPositionUpdate(PositionUpdateArgs args, Log log)
    {
        foreach (var observer in observers)
        {
            observer.UpdatePosition(args, log);
        }
    }

    public void NotifyContactInfoUpdate(ContactInfoUpdateArgs args, Log log)
    {
        foreach (var observer in observers)
        {
            observer.UpdateContactInfo(args, log);
        }
    }
}
