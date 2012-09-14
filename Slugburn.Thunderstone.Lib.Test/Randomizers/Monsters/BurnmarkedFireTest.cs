using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json;
using Slugburn.Thunderstone.Lib.Models;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
{
    [TestFixture]
    public class BurnmarkedFireTest
    {
        [Test]
        public void When_using_phoenix_trophy()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var phoenix = new BurnmarkedFire().CreateCards().First(x => x.Name == "Phoenix");
            player.AddCardToHand(phoenix);

            // Act
            phoenix.GetAbilities(Phase.Village).First().Action(player);
            player.EndTurn();

            // Assert
            Assert.That(game.Dungeon.Ranks.Last().Card, Is.SameAs(phoenix));
            Assert.That(phoenix.Owner, Is.EqualTo(CardOwner.Dungeon));
            var message = DungeonModel.From(game.Dungeon);
            var serialized = JsonConvert.SerializeObject(message);
        }

    }
}
