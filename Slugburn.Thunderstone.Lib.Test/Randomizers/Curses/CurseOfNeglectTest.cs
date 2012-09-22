using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfNeglectTest
    {
        [Test]
        public void Using_ability_destroys_curse()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfNeglect>();
            var longspear = context.CreateBasicCard<Longspear>();
            context.SetPlayerHand(curse, longspear);
            context.SetPlayerState(Phase.Village);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
        }

        [Test]
        public void Using_ability_reduces_available_gold_by_2()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfNeglect>();
            var longspear = context.CreateBasicCard<Longspear>();
            context.SetPlayerHand(curse, longspear);
            context.SetPlayerState(Phase.Village);

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.AvailableGold, Is.EqualTo(0));
        }
    }
}