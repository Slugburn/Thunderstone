﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Slugburn.Thunderstone.Lib.Test
{
    [TestFixture]  
    public class DungeonTest
    {
        [Test]
        public void When_dungeon_deck_is_empty_and_advancing_dungeon()
        {
            // Arrange
            var game = TestFactory.CreateGame();

            var deck = game.Dungeon.Deck;

            // draw all cards from deck
            Enumerable.Range(0, deck.Count).Each(x => deck.Draw());

            // Act
            game.Dungeon.Advance();

            // Assert
            Assert.That(game.Dungeon.Ranks.Last().Card, Is.Null);
        }

        [Test]
        public void When_dungeon_deck_is_empty_and_refilling_hall()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var dungeon = game.Dungeon;
            dungeon.FirstActiveRank = dungeon.FirstRank;
            var lastRank = dungeon.LastRank;
            lastRank.Card = null;

            var deck = dungeon.Deck;

            // draw all cards from deck
            Enumerable.Range(0, deck.Count).Each(x => deck.Draw());

            // Act
            dungeon.RefillHall();

            // Assert
            Assert.That(lastRank.Card, Is.Null);
        }

    }
}
