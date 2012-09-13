using System;

namespace Slugburn.Thunderstone.Lib.Events
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent ev);
        IObservable<TEvent> Observe<TEvent>();
    }
}
