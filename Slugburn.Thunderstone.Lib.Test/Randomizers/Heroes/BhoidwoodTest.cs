using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class BhoidwoodTest
    {
        [Test]
        public void Stalker_ability_switches_adjacent_monsters()
        {
            // Arrange
            var context = new TestContext();
            var stalker = context.CreateCard<Bhoidwood>("Bhoidwood Stalker");
            context.SetPlayerHand(stalker);
            context.SetPlayerState(Phase.Dungeon);
            var monster1 = context.GetMonsterInRank(1);
            var monster2 = context.GetMonsterInRank(2);
            context.WhenSelectingCardsSelect(monster1);

            // Act
            context.UseAbilityOf(stalker);

            // Assert
            Assert.That(context.GetMonsterInRank(1), Is.SameAs(monster2));
            Assert.That(context.GetMonsterInRank(2), Is.SameAs(monster1));
        }
    }
}
