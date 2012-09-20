using System;
using System.Collections.Concurrent;

namespace Slugburn.Thunderstone.Lib.Events
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent ev);
        IDisposable Subscribe<TEvent>(Action<TEvent> action);
    }
}
