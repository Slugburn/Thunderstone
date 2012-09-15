using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Models;

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

        public void Destroy(IEnumerable<Card> cards, string source)
        {
            throw new NotImplementedException();
        }

        public void Draw(IEnumerable<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void Discard(IEnumerable<Card> cards)
        {
            var game = Player.Game;
            cards.Each(card=>
                           {
                               game.RemoveCardFromHall(card.Rank.Number);
                               Player.AddToDiscard(card);
                           });
            game.RefillHallFrom(game.Dungeon.Ranks[0]);
            Player.View.UpdateStatus(StatusModel.From(Player));
        }

        public Player Player { get; private set; }
    }
}