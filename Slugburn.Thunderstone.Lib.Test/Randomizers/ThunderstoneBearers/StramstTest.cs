using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.ThunderstoneBearers;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.ThunderstoneBearers
{
    [TestFixture]
    public class StramstTest
    {
        [Test]
        public void Stramst_stays_in_the_last_rank()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var stramst = GivenStramstIsInTheLastRank(game);

            // Act
            game.Dungeon.Advance();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(stramst.Rank, Is.SameAs(lastRank), "Stramst should not advance");
            Assert.That(game.Dungeon.Ranks.All(x=>x.Card != null), Is.True);
        }

        [Test]
        public void Stramst_stays_in_the_last_rank_when_last_card_is_drawn_from_deck()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var stramst = GivenStramstIsInTheLastRank(game);

            // draw all but the last card from the dungeon deck
            var dungeonDeck = game.Dungeon.Deck;
            dungeonDeck.Draw(dungeonDeck.Count-1);

            // Act
            game.Dungeon.Advance();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(lastRank.Card, Is.Not.Null);
            Assert.That(stramst.Rank, Is.SameAs(lastRank), "Stramst should not advance");
            Assert.That(game.Dungeon.Ranks.All(x => x.Card != null), Is.True);
        }

        [Test]
        public void Stramst_advances_as_normal_when_the_dungeon_deck_is_empty()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var stramst = GivenStramstIsInTheLastRank(game);

            // empty the dungeon deck
            var dungeonDeck = game.Dungeon.Deck;
            dungeonDeck.Draw(dungeonDeck.Count);

            // Act
            game.Dungeon.Advance();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(stramst.Rank.Number, Is.EqualTo(lastRank.Number-1), "Stramst should advance");
            Assert.That(lastRank.Card, Is.Null);
        }

        private static Card GivenStramstIsInTheLastRank(Game game)
        {
            var stramst = new Stramst().CreateCards(game).First();
            var dungeon = game.Dungeon;
            dungeon.FirstActiveRank = dungeon.FirstRank;

            // Fill up the dungeon with monsters
            dungeon.RefillHall();

            // put Stramst into the last rank
            dungeon.Deck.AddToTop(stramst);
            dungeon.Advance();
            return stramst;
        }


    }
}
