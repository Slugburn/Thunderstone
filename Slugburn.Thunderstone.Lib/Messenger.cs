using System;
using System.Collections.Concurrent;

namespace Slugburn.Thunderstone.Lib
{
    public class Messenger
    {
        static Messenger()
        {
            Default = new Messenger();
        }

        public static Messenger Default { get; set; }

        private readonly ConcurrentDictionary<Guid,Action<string, object>> _subscriptions = new ConcurrentDictionary<Guid, Action<string, object>>();

        public void Send(Guid playerId, string messageId, object body)
        {
            Action<string, object> onMessage;
            if (_subscriptions.TryGetValue(playerId, out onMessage))
                onMessage(messageId, body);
        }

        public void Subscribe(Guid playerId, Action<string, object> onMessage)
        {
            _subscriptions.TryAdd(playerId, onMessage);
        }
    }
}
