using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Slugburn.Thunderstone.Lib.Test
{
    [TestFixture]
    public class GameTest
    {
        [Test]
        public void When_game_is_created_then_every_card_is_assigned_to_game()
        {
            // Arrange
            var context = new TestContext();
            var game = context.Game;

            // Act
            var allCards = game.Dungeon.Ranks.Select(x => x.Card)
                .Concat(game.Dungeon.Deck.GetCards())
                .Concat(game.Players.SelectMany(p=>p.Hand.Concat(p.Deck.GetCards()).Concat(p.Discard)))
                .Concat(game.Village.Decks.SelectMany(d=>d.GetCards()))
                .Concat(game.Curses.GetCards());

            // Assert
            Assert.That(allCards.All(c=>c.Game == game));
        }
    }
}
