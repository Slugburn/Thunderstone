using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public class HandSource : ICardSource
    {
        public Player Player { get; private set; }

        public HandSource(Player player)
        {
            Player = player;
        }

        public virtual IEnumerable<Card> GetCards()
        {
            return Player.Hand;
        }

        public void Destroy(IEnumerable<Card> cards, string source)
        {
            Player.DestroyCards(cards, source);
        }

        public void Draw(IEnumerable<Card> cards)
        {
            throw new InvalidOperationException("Drawing from a player's hand is not supported.");
        }

        public void Discard(IEnumerable<Card> cards)
        {
            var cardList = cards.ToList();
            Player.RemoveFromHand(cardList);
            Player.DiscardCards(cardList);
        }
    }
}