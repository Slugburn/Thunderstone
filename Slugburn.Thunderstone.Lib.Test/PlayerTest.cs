using System.Linq;
using NUnit.Framework;

namespace Slugburn.Thunderstone.Lib.Test
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void When_destroying_curse_then_it_is_added_back_to_deck()
        {
            // Arrange
            var game = TestFactory.CreateGame((s, o) => { });
            var player = game.CurrentPlayer;
            var curse = game.Curses.Draw();
            player.AddCardToHand(curse);

            // Act
            player.DestroyCard(curse);

            // Assert
            Assert.That(game.Curses.GetCards().Last(), Is.SameAs(curse));
        }
    }
}
