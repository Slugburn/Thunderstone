using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Slugburn.Thunderstone.Lib.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public IObservable<TEvent> Observe<TEvent>()
        {
            var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent),t => new Subject<TEvent>());
            return subject.AsObservable();
        }

        public void Publish<TEvent>(TEvent ev)
        {
            object subject;
            if (_subjects.TryGetValue(typeof (TEvent), out subject))
                ((ISubject<TEvent>) subject).OnNext(ev);
        }
    }
}
