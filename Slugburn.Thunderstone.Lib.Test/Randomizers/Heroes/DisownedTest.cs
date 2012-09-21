using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class DisownedTest
    {
        [Test]
        public void Bloodrager_can_equip_two_weapons()
        {
            // Arrange
            var context = new TestContext();
            context.SetTestPlayerState(Phase.Dungeon);
            var bloodrager = context.CreateCard<Disowned>("Disowned Bloodrager");
            var weapon1 = context.CreateCard<Pike>();
            var weapon2 = context.CreateCard<SnakeheadFlail>();
            context.SetPlayerHand(bloodrager, weapon1, weapon2);

            // Act
            context.HeroEquipsWeapon(bloodrager, weapon1);
            context.HeroEquipsWeapon(bloodrager, weapon2);

            // Assert
            Assert.That(bloodrager.IsEquipped, Is.True);
            Assert.That(bloodrager.GetEquipped(), Is.EquivalentTo(new[] {weapon1, weapon2}));
            Assert.That(weapon1.IsEquipped, Is.True);
            Assert.That(weapon2.IsEquipped, Is.True);
            Assert.That(bloodrager.TotalAttack, Is.EqualTo(14));
        }
    }
}