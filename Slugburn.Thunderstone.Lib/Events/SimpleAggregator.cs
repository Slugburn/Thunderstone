using System;
using System.Collections.Concurrent;

namespace Slugburn.Thunderstone.Lib.Events
{
    public class SimpleAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, object> _events = new ConcurrentDictionary<Type, object>();

        public void Publish<TEvent>(TEvent ev)
        {
            object events;
            if (_events.TryGetValue(typeof (TEvent), out events))
                ((ConcurrentDictionary<long, Action<TEvent>>)events).Values.Each(action => action(ev));
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> action)
        {
            var id = UniqueId.Next();
            var events = (ConcurrentDictionary<long, Action<TEvent>>)_events.GetOrAdd(typeof(TEvent), t => new ConcurrentDictionary<long, Action<TEvent>>());
            events.TryAdd(id, action);
            var disposer = new Disposer(() =>
                                            {
                                                object evs;
                                                if (!_events.TryGetValue(typeof (TEvent), out evs)) return;
                                                Action<TEvent> a;
                                                ((ConcurrentDictionary<long, Action<TEvent>>)evs).TryRemove(id, out a);
                                            });
            return disposer;
        }

        private class Disposer : IDisposable
        {
            private readonly Action _action;

            public Disposer(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}
