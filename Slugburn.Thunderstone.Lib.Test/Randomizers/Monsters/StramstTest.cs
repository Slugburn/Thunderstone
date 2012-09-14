using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.ThunderstoneBearers;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
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
            game.AdvanceDungeon();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(stramst.Rank, Is.EqualTo(lastRank.Number), "Stramst should not advance");
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
            game.AdvanceDungeon();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(stramst.Rank, Is.EqualTo(lastRank.Number), "Stramst should not advance");
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
            game.AdvanceDungeon();

            // Assert
            var lastRank = game.Dungeon.Ranks.Last();
            Assert.That(stramst.Rank, Is.EqualTo(lastRank.Number-1), "Stramst should advance");
            Assert.That(lastRank.Card, Is.Null);
        }

        private static Card GivenStramstIsInTheLastRank(Game game)
        {
            var stramst = new Stramst().CreateCards().First();

            // Fill up the dungeon with monsters
            var rank1 = game.Dungeon.Ranks[0];
            while (rank1.Card == null)
                game.RefillHallFrom(rank1);

            // put Stramst into the last rank
            game.Dungeon.Deck.AddToTop(stramst);
            game.AdvanceDungeon();
            return stramst;
        }


    }
}
