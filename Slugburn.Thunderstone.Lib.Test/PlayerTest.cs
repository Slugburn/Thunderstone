using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.BasicCards;

namespace Slugburn.Thunderstone.Lib.Test
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void When_destroying_curse_then_it_is_added_back_to_deck()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var curse = game.Curses.Draw();
            player.AddCardToHand(curse);

            // Act
            player.DestroyCard(curse, "test");

            // Assert
            Assert.That(game.Curses.GetCards().Last(), Is.SameAs(curse));
        }

        [Test]
        public void When_removing_equipped_weapon_from_hand_then_hero_loses_modifier()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var regular = new Regular().Create();
            var spear = new Longspear().Create();
            player.DiscardHand();
            player.AddCardsToHand(new[] {regular, spear});
            spear.GetAbilities().First().Action(player);

            // Act
            player.RemoveFromHand(spear);

            // Assert
            Assert.That(regular.PhysicalAttack, Is.EqualTo(1));
            player.View.AssertWasCalled(x => x.Log("Regular no longer has Longspear equipped."));
        }
    }
}
