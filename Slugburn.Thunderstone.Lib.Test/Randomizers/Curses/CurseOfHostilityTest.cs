using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfHostilityTest
    {
        [Test]
        public void Using_ability_destroys_curse_and_reduces_xp_by_2()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfHostility>();
            context.SetPlayerHand(curse);
            context.SetTestPlayerState(Phase.Dungeon);
            player.Xp = 2;

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
            Assert.That(player.Xp, Is.EqualTo(0));
        }
    }
}
