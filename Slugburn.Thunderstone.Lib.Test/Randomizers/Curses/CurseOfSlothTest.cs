using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfSlothTest
    {
        [Test]
        public void Using_ability_reduces_total_attack_value_by_3()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfSloth>();
            var sergeant = context.CreateCard<Criochan>();
            var summoner = context.CreateCard<Thundermage>();
            context.GivenPlayerHand(curse, sergeant, summoner);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

            // Assert
            Assert.That(player.TotalAttack, Is.EqualTo(sergeant.TotalAttack + summoner.TotalAttack - 3));
        }

        [Test]
        public void Using_ability_destroys_curse()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfSloth>();
            context.GivenPlayerHand(curse);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

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
            var curse = context.CreateCard<CurseOfSloth>();
            context.GivenPlayerHand(curse);
            var otherAbility = context.AddAbilityStub(phase);
            context.GivenTestPlayerState(Phase.Dungeon);

            // Act
            context.WhenUsingAbilityOf(curse);

            // Assert
            Assert.That(player.ActiveAbilities.Contains(otherAbility), Is.EqualTo(isActive));
        }

    }
}