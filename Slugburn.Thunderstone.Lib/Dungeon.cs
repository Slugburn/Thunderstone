using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public class Dungeon
    {
        public Dungeon(Game game,  Card thunderstoneBearer, IEnumerable<Card> monsters)
        {
            Game = game;
            var shuffled = monsters.Shuffle();
            var top = shuffled.Skip(10);
            var bottom = shuffled.Take(10).Concat(new[] {thunderstoneBearer}).Shuffle();
            Deck = Deck.Create(top.Concat(bottom));
            Deck.GetCards().Each(c => c.Owner = CardOwner.Dungeon);
            Ranks = Enumerable.Range(1,4).Select(x=>new Rank(this, x,-x)).ToArray();
        }

        public Game Game { get; private set; }

        public Deck Deck { get; private set; }

        public Rank[] Ranks { get; private set; }

        public Rank FirstActiveRank { get; set; }

        public Rank LastRank
        {
            get { return Ranks.Last(); }
        }

        public Rank FirstRank
        {
            get { return Ranks.First(); }
        }

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

        public void RefillHall()
        {
            // Get active ranks
            var activeRanks = Ranks.Where(r => r.Number >= FirstActiveRank.Number).ToArray();

            // While any active rank is empty
            while (activeRanks.Any(r=>r.Card == null))
            {
                // Get last empty rank
                var emptyRank = activeRanks.Last(r => r.Card == null);

                // Move all cards to right of last empty rank to the left
                GetRanksAfter(emptyRank).OrderBy(r=>r.Number).Each(r=>
                    {
                        r.NextRank.Card = r.Card;
                        r.Card = null;
                    });

                // Fill last rank with card drawn from deck
                var drawn = Deck.Draw();
                if (drawn == null) 
                    break;

                LastRank.Card = drawn;
                Game.SubscribeCardToEvents(drawn);
                Game.Publish(new EnteredDungeonHall(drawn));
            }
            Game.Publish(new DungeonHallRefilled(Game));
            Game.SendUpdateDungeon();
        }

        public IEnumerable<Rank> GetRanksAfter(Rank rank)
        {
            return Ranks.Where(r => r.Number > rank.Number);
        }

        public void Advance()
        {
            // Advance the first active rank by 1
            if (FirstActiveRank == null)
            {
                FirstActiveRank = LastRank;
            }
            else
            {
                if (FirstActiveRank != FirstRank)
                    FirstActiveRank = FirstActiveRank.NextRank;
            }

            var rank1 = FirstRank;
            var rank1Card = rank1.Card;
            // Rank 1 escapes, penalize the player
            if (rank1Card != null)
            {
                rank1Card.Rank = null;
                var lostVp = rank1Card.Vp ?? 0;
                Game.CurrentPlayer.Log("{0} escapes: {1} VP".Template(rank1Card.Name, -lostVp));
                Game.CurrentPlayer.Vp -= lostVp;
                Game.CurrentPlayer.View.UpdateStatus(StatusModel.From(Game.CurrentPlayer));
            }
            rank1.RemoveCard();

            RefillHall();
        }
    }
}
