using System;
using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public interface ISelectionCallback : IDefineSelectionOrCallback
    {
        void SendRequest(Action<SelectionContext> continuation);
    }

    public static class SelectionCallbackExtensions
    {
        public static ISelectionCallback Callback(this IDefineSelectionOrCallback context, Action<ISelectionContext> action)
        {
            var c = (SelectionContext)context;
            c.Callbacks.Add(action);
            return c;
        }

        public static ISelectionCallback Discard(this IDefineSelectionOrCallback context)
        {
            var c = (SelectionContext)context;
            context.Callback(x => x.Source.Discard(x.Selected));
            return c;
        }

        public static IEnumerable<Card> Matches(this IDefineSelectionOrCallback context)
        {
            var c = (SelectionContext)context;
            return c.GetSourceCards();
        }
    }
}
