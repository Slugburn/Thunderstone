using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public class Game
    {
        private readonly IEventAggregator _events;

        public Game()
        {
            Players = new List<Player>();
            _events = new EventAggregator();
        }

        public List<Player> Players { get; private set; }

        public Player CurrentPlayer { get; set; }

        public Dungeon Dungeon { get; set; }

        public Village Village { get; set; }

        public Deck Curses { get; set; }

        public void UseVillageAbilities()
        {
        }

        public IEnumerable<Deck> GetBuyableDecks(int availableGold)
        {
            return Village.Decks.Where(x=>x.TopCard != null && x.TopCard.Cost <= availableGold);
        }

        public void LevelHeroes()
        {
        }

        public void SendAll(Action<IPlayerView> action)
        {
            Players.Each(p=>action(p.View));
        }

        public void Initialize(GameSession session)
        {
            Players = session.GetPlayers();
            session.Setup.CreateGame(this);
        }

        public Card BuyCard(long deckId)
        {
            var deck = Village[deckId];
            var card = deck.Draw();
            SendUpdateDeck(deck);
            return card;
        }

        public void SendUpdateDeck(Deck deck)
        {
            SendAll(view => view.UpdateDeck(DeckModel.From(deck)));
        }

        public void EndTurn()
        {
            if (!CurrentPlayer.Won)
                AdvanceDungeon();
            NextPlayer();
        }

        public void AdvanceDungeon()
        {
            var rank1 = Dungeon.Ranks[0];
            var rank1Card = rank1.Card;
            // Rank 1 escapes, penalize the player
            if (rank1Card != null)
            {
                rank1Card.Rank = null;
                var lostVp = rank1Card.Vp ?? 0;
                CurrentPlayer.Log("{0} escapes: {1} VP".Template(rank1Card.Name, -lostVp));
                CurrentPlayer.Vp -= lostVp;
                CurrentPlayer.View.UpdateStatus(StatusModel.From(CurrentPlayer));
            }
            RemoveCardFromHall(rank1);

            RefillHallFrom(rank1);
        }

        public void RefillHallFrom(Rank fromRank)
        {
            var ranks = Dungeon.Ranks;
            var firstRankIndex = fromRank.Number - 1;
            var lastRankIndex = ranks.Length - 1;
            for (var i = firstRankIndex; i < lastRankIndex; i++)
            {
                if (ranks[i].Card != null) continue;
                ranks[i].Card = ranks[i + 1].Card;
                ranks[i + 1].Card = null;
            }
            if (ranks[lastRankIndex].Card == null)
            {
                var drawn = Dungeon.Deck.Draw();
                if (drawn != null)
                {
                    ranks[lastRankIndex].Card = drawn;
                    drawn.Subscribe(_events);
                }
            }
            Publish(new DungeonHallRefilled(this));
            SendUpdateDungeon();
        }

        public void Publish<TEvent>(TEvent ev)
        {
            _events.Publish(ev);
        }

        public void SendUpdateDungeon()
        {
            SendAll(view => view.UpdateDungeon(DungeonModel.From(Dungeon)));
        }

        private void NextPlayer()
        {
            var nextIndex = Players.IndexOf(CurrentPlayer) + 1;
            if (nextIndex >= Players.Count)
                nextIndex = 0;
            CurrentPlayer = Players[nextIndex];
            CurrentPlayer.StartTurn();
        }

        public void RemoveCardFromHall(int? rankNumber)
        {
            var rank = Dungeon.Ranks.Single(x => x.Number == rankNumber);
            RemoveCardFromHall(rank);
            SendUpdateDungeon();
        }

        private static void RemoveCardFromHall(Rank rank)
        {
            var removedCard = rank.Card;
            if (removedCard != null)
            {
                removedCard.Reset();
            }
            rank.Card = null;
        }

        public void GiveCurseTo(Player player)
        {
            var curse = Curses.Draw();
            Log("Player gains {0}.".Template(curse.Name));
            player.AddToDiscard(curse);
        }

        private void Log(string message)
        {
            Players.Each(p=>p.Log(message));
        }
    }
}
