using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Weapons
{
    [TestFixture]
    public class SnakeheadFlailTest
    {
        [Test]
        public void The_flail_has_no_potential_attack_if_no_heroes_in_hand()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var flail = context.CreateCard<SnakeheadFlail>();
            context.SetPlayerHand(flail);

            // Act
            var potential = flail.PotentialPhysicalAttack;

            // Assert
            Assert.That(potential, Is.Null);
        }
    }
}