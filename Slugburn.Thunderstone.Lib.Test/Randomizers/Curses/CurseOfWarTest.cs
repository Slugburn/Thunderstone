using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfWarTest
    {
        [Test]
        public void When_curse_is_in_hand_then_cant_prepare_or_rest()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfWar>();
            context.SetPlayerHand(curse);
            
            // Act
            player.StartTurn();

            // Assert
            var message = context.Get<StartTurnMessage>();
            Assert.That(message.AvailableActions, Is.EquivalentTo(new[]{"Village", "Dungeon"}));
        }

        [Test]
        public void Hand_must_have_at_least_3_cards_in_order_to_use_ability()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfWar>();
            var card1 = context.CreateBasicCard<Torch>();
            context.SetPlayerHand(curse, card1);
            context.SetTestPlayerState(Phase.Dungeon);
            var ability = context.GetAbilityOf(curse);

            // Act
            var canUse = ability.Condition(player);

            // Assert
            Assert.That(canUse, Is.False);
        }

        [Test]
        [TestCase(Phase.Village)]
        [TestCase(Phase.Dungeon)]
        public void Using_ability_destroys_curse(Phase phase)
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfWar>();
            var card1 = context.CreateBasicCard<Torch>();
            var card2 = context.CreateBasicCard<Regular>();
            context.SetPlayerHand(curse, card1, card2);
            context.SetTestPlayerState(phase);
            context.GivenSelectingFirstMatchingCards();

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand, Has.No.Member(curse));
        }

        [Test]
        public void Using_ability_destroys_one_random_card()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfWar>();
            var card1 = context.CreateBasicCard<Torch>();
            var card2 = context.CreateBasicCard<Regular>();
            context.SetPlayerHand(curse, card1, card2);
            context.SetTestPlayerState(Phase.Village);
            context.GivenSelectingFirstMatchingCards();

            // Act
            context.UseAbilityOf(curse);

            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(1));
        }
    }
}