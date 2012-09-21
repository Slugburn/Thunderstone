using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public interface ISelectionContext
    {
        Player Player { get; }
        Ability TriggeredBy { get; }
        ICardSource Source { get; }
        IList<Card> Selected { get; set; }
        IList<Card> PreviousSelection { get; }
        Game Game { get; }
    }
}