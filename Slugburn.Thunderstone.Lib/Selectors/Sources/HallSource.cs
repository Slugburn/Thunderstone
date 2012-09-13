using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public class HallSource : ICardSource
    {
        private readonly Dungeon _dungeon;

        public HallSource(Player player)
        {
            Player = player;
            _dungeon = Player.Game.Dungeon;
        }

        public IEnumerable<Card> GetCards()
        {
            return _dungeon.Ranks.Select(rank => rank.Card).Where(c => c != null);
        }

        public void Destroy(IEnumerable<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void Draw(IEnumerable<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void Discard(IEnumerable<Card> cards)
        {
            cards.Each(card=>
                           {
                               Player.Game.RemoveCardFromHall(card.Rank);
                               Player.AddToDiscard(card);
                           });
            Player.SendUpdateStatus();
        }

        public Player Player { get; private set; }
    }
}