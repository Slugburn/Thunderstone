using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
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
            var ettin = context.SetMonsterInFirstRank<OgreHumanoid>("Ettin");

            // Act
            context.HeroEquipsWeapon(hero, pike);
            context.WhenBattling(ettin);

            // Assert
            Assert.That(player.Hand, Has.Member(hero), "Pike should protect hero from Ettin.");
        }

        [Test]
        public void Pike_does_not_protect_from_non_battle_effects()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var hero = context.CreateBasicCard<Regular>();
            var pike = context.CreateCard<Pike>();
            context.SetPlayerHand(hero, pike);
            var ogre = context.SetMonsterInFirstRank<OgreHumanoid>("Ogre");

            // Act
            context.HeroEquipsWeapon(hero, pike);
            context.SetTestPlayerState(Phase.Aftermath);
            player.OnSelectMonster(ogre);
            context.UseAbilityOf(ogre);

            // Assert
            Assert.That(player.Hand, Has.No.Member(hero), "Pike should not protect hero from Ogre.");
        }

        [Test]
        public void Pike_only_protects_the_hero_that_has_it_equipped()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var equippedHero = context.CreateCard<Criochan>();
            var unequippedHero = context.CreateCard<Criochan>();
            var pike = context.CreateCard<Pike>();
            context.SetPlayerHand(equippedHero, pike);
            var ettin = context.SetMonsterInFirstRank<OgreHumanoid>("Ettin");

            // Act
            context.HeroEquipsWeapon(equippedHero, pike);
            context.WhenBattling(ettin);

            // Assert
            Assert.That(player.Hand, Has.Member(equippedHero), "Pike should protect hero from Ettin.");
            Assert.That(player.Hand, Has.No.Member(unequippedHero), "Ettin eats unequipped hero.");
        }

        [Test]
        public void A_hero_not_armed_with_a_pike_is_not_immune_to_battle_effects()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var hero = context.CreateCard<Criochan>();
            var pike = context.CreateCard<Pike>();
            context.SetPlayerHand(hero, pike);
            var ettin = context.SetMonsterInFirstRank<OgreHumanoid>("Ettin");

            // Act
            context.WhenBattling(ettin);

            // Assert
            Assert.That(player.Hand, Has.No.Member(hero), "Ettin eats hero.");
        }
    }
}
