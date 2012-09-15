using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class WhetmageTest
    {
        [Test]
        public void Level_up_other_hero()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.State = PlayerState.Dungeon;
            player.DiscardHand();
            var honer = new Whetmage().CreateCards().First(c => c.Level == 1);
            var otherHero = game.Village[CardType.Hero].First(x => x.TopCard.Level > 0).Draw();
            player.AddCardsToHand(new[] {honer, otherHero});
            var ability = honer.GetAbilities().First();
            player.Xp = 2;

            // Fill up the dungeon so the Whetmage has someone to battle
            while(game.Dungeon.Ranks[0].Card==null)
                game.AdvanceDungeon();

            // Act
            Assert.That(ability.IsUsableByOwner() && ability.Condition(player), "Whetmage ability should be usable");
            ability.Action(player);

            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(2));
            Assert.That(player.Hand.Contains(otherHero), Is.False);
            Assert.That(player.Hand.Single(c=>c != honer).Level, Is.EqualTo(2));
        }

    }
}
