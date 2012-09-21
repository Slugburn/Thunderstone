using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Weapons
{
    [TestFixture]
    public class PikeTest
    {
        [Test]
        public void A_hero_armed_with_a_pike_is_immune_to_battle_effects()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var hero = context.CreateCard<Criochan>();
            var pike = context.CreateCard<Pike>();
            context.SetPlayerHand(hero, pike);
            var ettin = context.GivenMonsterInFirstRank<OgreHumanoid>("Ettin");

            // Act
            context.HeroEquipsWeapon(hero, pike);
            context.WhenBattling(ettin);

            // Assert
            Assert.That(player.Hand, Has.Member(hero), "Pike should protect hero from Ettin.");
        }
    }
}
