using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
{
    [TestFixture]
    public class KoboldHumanoidTest
    {
        [Test]
        public void Laird_gains_health_equal_to_other_kobolds_in_hall()
        {
            // Arrange
            var context = new TestContext();
            var laird = context.CreateCard<KoboldHumanoid>("Drakeclan Laird");
            var ambusher = context.CreateCard<KoboldHumanoid>("Drakeclan Ambusher");
            var cutter = context.CreateCard<KoboldHumanoid>("Drakeclan Cutter");
            var shaman = context.CreateCard<KoboldHumanoid>("Drakeclan Shaman");
            context.GivenTopOfDungeonDeck(laird, ambusher, cutter, shaman);

            // Act
            context.WhenMonsterInFirstRank(laird);

            // Assert
            Assert.That(laird.Health, Is.EqualTo(laird.GetBaseValue(Attr.Health) + ambusher.Health + cutter.Health + shaman.Health));
        }
        [Test]

        public void Two_lairds_gains_health_equal_to_other_kobolds_in_hall()
        {
            // Arrange
            var context = new TestContext();
            var laird1 = context.CreateCard<KoboldHumanoid>("Drakeclan Laird");
            var laird2 = context.CreateCard<KoboldHumanoid>("Drakeclan Laird");
            var cutter = context.CreateCard<KoboldHumanoid>("Drakeclan Cutter");
            var shaman = context.CreateCard<KoboldHumanoid>("Drakeclan Shaman");
            context.GivenTopOfDungeonDeck(laird1, laird2, cutter, shaman);

            // Act
            context.WhenMonsterInFirstRank(laird1);

            // Assert
            Assert.That(laird1.Health, Is.EqualTo(laird1.GetBaseValue(Attr.Health) + laird2.GetBaseValue(Attr.Health) + cutter.Health + shaman.Health));
            Assert.That(laird2.Health, Is.EqualTo(laird1.GetBaseValue(Attr.Health) + laird1.GetBaseValue(Attr.Health) + cutter.Health + shaman.Health));
        }

        [Test]
        public void When_laird_is_defeated_then_all_kobolds_in_hall_are_defeated()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var laird = context.CreateCard<KoboldHumanoid>("Drakeclan Laird");
            var ambusher = context.CreateCard<KoboldHumanoid>("Drakeclan Ambusher");
            var cutter = context.CreateCard<KoboldHumanoid>("Drakeclan Cutter");
            var shaman = context.CreateCard<KoboldHumanoid>("Drakeclan Shaman");
            var kobolds = new[] {laird, ambusher, cutter, shaman};
            context.GivenTopOfDungeonDeck(kobolds);
            context.WhenMonsterInFirstRank(laird);
            var hand = Enumerable.Range(0, 6).Select(x => context.CreateCard<Criochan>("Criochan Captain")).ToArray();
            context.SetPlayerHand(hand);
            var startingXp = player.Xp;
            var startingVp = player.Vp;

            // Act
            context.WhenBattling(laird);
            player.DetermineBattleResult();

            // Assert
            kobolds.Each(x => Assert.That(player.Discard.Contains(x), "Discard does not contain {0}".Template(x.Name)));
            context.Game.Dungeon.Ranks.Each(r=> Assert.That(r.Card, Is.Not.Null, "Rank {0} is empty".Template(r.Number)));
            kobolds.Each(x => Assert.That(context.Game.Dungeon.Ranks.All(rank => rank.Card != x)));
            Assert.That(player.Xp, Is.EqualTo(startingXp + laird.Xp));
            Assert.That(player.Vp, Is.EqualTo(startingVp + kobolds.Sum(x => x.Vp)));
        }
    }
}
