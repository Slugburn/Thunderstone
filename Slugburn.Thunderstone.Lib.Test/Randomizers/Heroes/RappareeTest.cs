using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class RappareeTest
    {
        [Test]
        public void Looter_spoils_ability_buys_card_and_puts_on_top_of_deck()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var looter = context.CreateCard<Rapparee>("Rapparee Looter");
            context.SetPlayerHand(looter);
            context.SetPlayerState(Phase.Spoils);
            var longspear = context.GetVillageDeck(CardType.Weapon, card => card.Name == "Longspear").TopCard;
            context.WhenSelectingCardsSelect(longspear);

            // Act
            context.UseAbilityOf(looter);

            // Assert
            Assert.That(player.Deck.TopCard, Is.SameAs(longspear));
            Assert.That(player.Discard, Has.No.Member(longspear));
        }
    }
}