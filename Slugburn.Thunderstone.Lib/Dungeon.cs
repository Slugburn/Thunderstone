using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public class Dungeon
    {
        public Dungeon(Card thunderstoneBearer, IEnumerable<Card> monsters)
        {
            var shuffled = monsters.Shuffle();
            var top = shuffled.Skip(10);
            var bottom = shuffled.Take(10).Concat(new[] {thunderstoneBearer}).Shuffle();
            Deck = Deck.Create(top.Concat(bottom));
            Deck.GetCards().Each(c => c.Owner = CardOwner.Dungeon);
            Ranks = Enumerable.Range(1,4).Select(x=>new Rank(x,-x)).ToArray();
        }

        public Deck Deck { get; set; }

        public Rank[] Ranks { get; private set; }

        public Rank GetRankOf(Card monster)
        {
            return Ranks.SingleOrDefault(x => x.Card == monster);
        }

        public Rank GetRankNumber(int number)
        {
            return Ranks.SingleOrDefault(x => x.Number == number);
        }

        public void AddToTopOfDeck(Card card)
        {
            card.Owner = CardOwner.Dungeon;
            Deck.AddToTop(card);
        }
    }
}
