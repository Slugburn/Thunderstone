using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfDiscordTest
    {
        [Test]
        public void Curse_of_discord_cannot_discard_itself()
        {
            // Arrange
            var context = new TestContext();
            context.Player.State = PlayerState.Village;
            var curse = context.CreateCard<CurseOfDiscord>();
            var regular = context.CreateBasicCard<Regular>();
            var torch = context.CreateBasicCard<Torch>();
            var longspear = context.CreateBasicCard<Longspear>();
            context.GivenPlayerHand(curse, regular, torch, longspear);

            // Act
            context.WhenUsingAbilityOf(curse);

            // Assert
            Assert.That(context.SelectCardsIds, Has.No.Member(curse.Id));
        }

        [Test]
        public void Must_be_at_least_2_other_cards_to_use_ability()
        {
            // Arrange
            var context = new TestContext();
            var curse = context.CreateCard<CurseOfDiscord>();
            var regular = context.CreateBasicCard<Regular>();
            context.GivenPlayerHand(curse, regular);

            // Act
            var usable = curse.GetAbilities().First().Condition(context.Player);

            // Assert
            Assert.That(usable, Is.False);
        }
    }
}
