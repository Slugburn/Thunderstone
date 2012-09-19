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
            context.GivenPlayerHand(curse, torch);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

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
            context.GivenPlayerHand(curse, torch);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
        }

        [Test]
        public void Using_ability_disables_equip_dungeon_and_trophy_abilities()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfHorror>();
            context.GivenPlayerHand(curse);
            var equipAbility = context.AddAbilityStub(Phase.Equip);
            var dungeonAbility = context.AddAbilityStub(Phase.Dungeon);
            var trophyAbility = context.AddAbilityStub(Phase.Trophy);
            var spoilsAbility = context.AddAbilityStub(Phase.Spoils);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

            // Assert
            Assert.That(player.ActiveAbilities, Has.No.Member(equipAbility));
            Assert.That(player.ActiveAbilities, Has.No.Member(dungeonAbility));
            Assert.That(player.ActiveAbilities, Has.No.Member(trophyAbility));
            Assert.That(player.ActiveAbilities, Has.Member(spoilsAbility));
        }
    }
}
