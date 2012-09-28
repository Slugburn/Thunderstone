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
            _events = new SimpleAggregator();
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
                Dungeon.Advance();
            NextPlayer();
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
            rank.RemoveCard();
            SendUpdateDungeon();
        }

        public void RemoveCardFromHall(Card card)
        {
            card.Rank.RemoveCard();
        }

        public void GiveCurseTo(Player player)
        {
            var curse = Curses.Draw();
            Log("Player gains {0}.".Template(curse.Name));
            player.AddToDiscard(curse);
        }

        public void Log(string message)
        {
            Players.Each(p=>p.Log(message));
        }

        public void SubscribeCardToEvents(Card card)
        {
            card.Subscribe(_events);
        }
    }
}
