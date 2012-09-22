using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfShameTest
    {
        [Test]
        public void Using_ability_destroys_curse()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfShame>();
            context.SetPlayerHand(curse);
            context.SetPlayerState(Phase.Village);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
        }

        [Test]
        public void Using_ability_reduces_number_of_cards_drawn()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfShame>();
            context.SetPlayerHand(curse);
            context.SetPlayerState(Phase.Village);

            // Act
            context.UseAbilityOf(curse);
            context.Player.EndTurn();

            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(4));
        }

    }
}