using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
{
    [TestFixture]
    public class BurnmarkedFireTest
    {
        [Test]
        public void When_using_phoenix_trophy()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.State = PlayerState.Village;
            var phoenix = new BurnmarkedFire().CreateCards().First(x => x.Name == "Phoenix");
            player.AddCardToHand(phoenix);
            var startingXp = player.Xp;
            var trophyAbility = phoenix.GetAbilities(Phase.Trophy).First();

            // Act
            trophyAbility.Action(player);
            player.EndTurn();

            // Assert
            Assert.That(player.Xp, Is.EqualTo(startingXp + 3));
            Assert.That(game.Dungeon.Ranks.Last().Card, Is.SameAs(phoenix));
            Assert.That(phoenix.Owner, Is.EqualTo(CardOwner.Dungeon));
        }

    }
}
