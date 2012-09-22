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
            var context = new TestContext();
            var player = context.Player;
            context.SetTestPlayerState(Phase.Trophy);
            var phoenix = context.CreateCard<BurnmarkedFire>("Phoenix");
            context.SetPlayerHand(phoenix);

            var startingXp = player.Xp;

            // Act
            context.UseAbilityOf(phoenix);
            player.EndTurn();

            // Assert
            Assert.That(player.Xp, Is.EqualTo(startingXp + 3));
            Assert.That(context.GetMonsterInRank(4), Is.SameAs(phoenix));
            Assert.That(phoenix.Owner, Is.EqualTo(CardOwner.Dungeon));
        }

    }
}
