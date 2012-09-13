using System;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public static class SelectionExtensions
    {
        public static ISelectionCallback Destroy(this IDefineSelectionOrCallback context, string destructionSource)
        {
            var c = (SelectionContext)context;
            var message = c.TriggeredBy == null
                ? "Destroy 1 card."
                : "{0} destroys 1 card.".Template(c.TriggeredBy.Name);
            c.Message(message)
                .Callback(x => x.Source.Destroy(x.Selected, destructionSource));
            return c;
        }

    }
}
