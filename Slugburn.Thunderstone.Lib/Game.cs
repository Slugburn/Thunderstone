﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib
{
    public class Game
    {
        public Game()
        {
            Players = new List<Player>();
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

        public void SendAll(string messageId, object body)
        {
            Players.Each(x => x.SendMessage(messageId, body));
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
            SendAll("UpdateDeck", deck.CreateMessage());
        }

        public void EndTurn()
        {
            if (!CurrentPlayer.Won)
                AdvanceDungeon();
            NextPlayer();
        }

        private void AdvanceDungeon()
        {
            var rank1Card = Dungeon.Ranks[0].Card;
            // Rank 1 escapes, penalize the player
            if (rank1Card != null)
            {
                rank1Card.Rank = null;
                var lostVp = rank1Card.Vp ?? 0;
                CurrentPlayer.Log("{0} escapes: {1} VP".Template(rank1Card.Name, -lostVp));
                CurrentPlayer.Vp -= lostVp;
                CurrentPlayer.SendUpdateStatus();
            }

            for (var i = 0; i< Dungeon.Ranks.Length -1;i++)
            {
                Dungeon.Ranks[i].Card = Dungeon.Ranks[i + 1].Card;
            }
            var cardLast = Dungeon.Deck.Draw();
            Dungeon.Ranks[Dungeon.Ranks.Length - 1].Card = cardLast;
            SendUpdateDungeon();
        }

        public void SendUpdateDungeon()
        {
            SendAll("UpdateDungeon", Dungeon.CreateMessage());
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
            if (rank.Card != null)
                rank.Card.Rank = null;
            rank.Card = null;
            SendUpdateDungeon();
        }

        public void RefillHall(Rank fromRank)
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
                ranks[lastRankIndex].Card = Dungeon.Deck.Draw();
            SendUpdateDungeon();
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
