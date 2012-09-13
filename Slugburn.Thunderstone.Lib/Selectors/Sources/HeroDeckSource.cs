using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public class HeroDeckSource : ICardSource
    {
        public Player Player { get; private set; }

        public HeroDeckSource(Player player)
        {
            Player = player;
        }

        public IEnumerable<Card> GetCards()
        {
            return Player.Game.Village[CardType.Hero].SelectMany(x => x.GetCards().GroupBy(c => c.Level).Select(g=>g.First()));
        }

        public void Destroy(IEnumerable<Card> cards)
        {
            cards.Each(RemoveCardFromDeck);
        }

        public void Draw(IEnumerable<Card> cards)
        {
            cards.Each(card =>
                           {
                               RemoveCardFromDeck(card);
                               Player.AddCardToHand(card);
                           });
        }

        public void Discard(IEnumerable<Card> cards)
        {
            cards.Each(card =>
                           {
                               RemoveCardFromDeck(card);
                               Player.AddToDiscard(card);
                           });
        }


        private void RemoveCardFromDeck(Card card)
        {
            var deck = Player.Game.Village[CardType.Hero].Single(x => x.GetCards().Contains(card));
            deck.Remove(card);
            Player.Game.SendUpdateDeck(deck);
        }
    }
}
