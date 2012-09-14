using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class ThundermageTest
    {
        [Test]
        public void When_()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var bolter = new Thundermage().CreateCards().First(c => c.Level == 3);
            var discardRank1Monster = bolter.GetAbilities().First();

            // fill the dungeon hall
            var rank1 = game.Dungeon.Ranks[0];
            while (rank1.Card == null)
                game.AdvanceDungeon();
            var targetMonster = rank1.Card;

            var startingXp = player.Xp;
            var startingVp = player.Vp;

            // Act
            Assert.That(discardRank1Monster.Condition(player), Is.True);
            discardRank1Monster.Action(player);

            // Assert
            Assert.That(player.Discard.Contains(targetMonster));
            Assert.That(player.Xp, Is.EqualTo(startingXp), "Using the Thundermage Bolter's ability does not generate XP");
            Assert.That(player.Vp, Is.EqualTo(startingVp + targetMonster.Vp));
            Assert.That(rank1.Card, Is.Not.Null, "Dungeon hall should be refilled");
        }

    }
}
