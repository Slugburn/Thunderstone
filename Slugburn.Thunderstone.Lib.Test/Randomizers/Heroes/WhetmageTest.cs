using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class WhetmageTest
    {
        [Test]
        public void Level_up_other_hero()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            context.SetPlayerState(Phase.Dungeon);
            var honer = context.CreateCard<Whetmage>();
            var otherHero = context.DrawHeroFromTopOfDeck(c => c.Level > 0);
            context.SetPlayerHand(honer, otherHero);
            player.Xp = 2;

            // Act
            context.UseAbilityOf(honer);

            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(2));
            Assert.That(player.Hand.Contains(otherHero), Is.False);
            Assert.That(player.Hand.Single(c=>c != honer).Level, Is.EqualTo(2));
        }

        [Test]
        public void Hand_must_contain_another_hero()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var honer = context.CreateCard<Whetmage>("Whetmage Honer");
            context.SetPlayerHand(honer);
            player.Xp = 2;
            context.SetPlayerState(Phase.Dungeon);
            var ability = context.GetAbilityOf(honer);

            // Act
            var isUsable = context.IsAbilityUsable(ability);

            // Assert
            Assert.That(isUsable, Is.False);
        }

        [Test]
        public void Polisher_can_use_ability_multiple_times()
        {
            // Arrange
            var context = new TestContext();
            context.SetPlayerState(Phase.Dungeon);
            var polisher = context.CreateCard<Whetmage>("Whetmage Polisher");
            var hero1 = context.DrawHeroFromTopOfDeck(x=>x.Level==1);
            var hero2 = context.DrawHeroFromTopOfDeck(x => x.Level == 1);
            context.SetPlayerHand(polisher, hero1, hero2);
            context.Player.Xp = 10;
            var levelUp = polisher.GetAbility();

            // Act
            context.UseAbilityOf(polisher);

            // Assert
            Assert.That(context.Player.ActiveAbilities.Any(x=>x.Id == levelUp.Id));
        }

        [Test]
        public void Polisher_cannot_use_ability_to_level_up_card_it_already_leveled_up()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            player.State = PlayerState.Dungeon;
            var polisher = context.CreateCard<Whetmage>("Whetmage Polisher");
            var hero = context.DrawHeroFromTopOfDeck(x => x.Level == 1);
            context.SetPlayerHand(polisher, hero);
            player.Xp = 10;
            var levelUp = polisher.GetAbility();

            // Act
            context.UseAbility(levelUp);

            // Assert
            Assert.That(levelUp.Condition(player), Is.False);
        }
    }
}
