using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;
using Slugburn.Thunderstone.Lib.Randomizers.Items;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfHorrorTest
    {
        [Test]
        public void Using_ability_reduces_player_light_to_zero()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfHorror>();
            var torch = context.CreateBasicCard<Torch>();
            context.SetPlayerHand(curse, torch);
            context.SetTestPlayerState(Phase.Dungeon);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.TotalLight, Is.EqualTo(0));
        }

        [Test]
        public void Using_ability_destroys_curse()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfHorror>();
            var torch = context.CreateBasicCard<Torch>();
            context.SetPlayerHand(curse, torch);
            context.SetTestPlayerState(Phase.Dungeon);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
        }

        [Test]
        [TestCase(Phase.Equip, false)]
        [TestCase(Phase.Dungeon, false)]
        [TestCase(Phase.Trophy, false)]
        [TestCase(Phase.Spoils, true)]
        public void Using_ability_disables_abilities(Phase phase, bool isActive)
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfHorror>();
            context.SetPlayerHand(curse);
            var otherAbility = context.AddAbilityStub(phase);
            context.SetTestPlayerState(Phase.Dungeon);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.ActiveAbilities.Contains(otherAbility), Is.EqualTo(isActive));
        }
    }
}
