using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class ThundermageTest
    {
        [Test]
        public void When_thundermage_bolter_discards_monster()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            context.SetPlayerState(Phase.Dungeon);
            var bolter = context.CreateCard<Thundermage>("Thundermage Bolter");
            context.SetPlayerHand(bolter);

            var targetMonster = context.GetMonsterInRank(1);

            var startingXp = player.Xp;
            var startingVp = player.Vp;

            // Act
            context.UseAbilityOf(bolter);

            // Assert
            Assert.That(player.Discard.Contains(targetMonster));
            Assert.That(player.Xp, Is.EqualTo(startingXp), "Using the Thundermage Bolter's ability does not generate XP");
            Assert.That(player.Vp, Is.EqualTo(startingVp + targetMonster.Vp));
            Assert.That(context.GetMonsterInRank(1), Is.Not.Null, "Dungeon hall should be refilled");
        }

    }
}
