using NUnit.Framework;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Test.Selectors
{
    [TestFixture]
    public class SelectionBehaviorTest
    {
        [Test]
        [TestCase(1,1)]
        [TestCase(2,2)]
        [TestCase(2,1)]
        public void When_selecting_cards_and_matching_cards_lte_min_number_then_no_input_is_required_by_player(int requestedCount, int matchingCardCount)
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var callbackExecuted = false;
            var continuationExecuted = false;
            player.DiscardHand();
            player.Draw(matchingCardCount);
            
            // Act
            player.SelectCard()
                .FromHand()
                .Min(requestedCount)
                .Max(requestedCount)
                .Callback(context => { callbackExecuted = true; })
                .SendRequest(p => { continuationExecuted = true; });

            // Assert
            player.View.AssertWasNotCalled(x => x.SelectCards(Arg<object>.Is.Anything));
            Assert.That(callbackExecuted, Is.True);
            Assert.That(continuationExecuted, Is.True);
        }
    }
}
