using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public interface ICardSource
    {
        IEnumerable<Card> GetCards();
        void Destroy(IEnumerable<Card> cards);
        void Draw(IEnumerable<Card> cards);
        void Discard(IEnumerable<Card> cards);
        Player Player { get; }
    }
}