using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Weapons
{
    [TestFixture]
    public class FalconArbalestTest
    {
        [Test]
        public void Ability_ids_are_unique()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;

            // Act
            var cards = context.CreateCards<FalconArbalest>();

            // Assert
            Assert.That(cards.Select(c => c.Id).Distinct().Count(), Is.EqualTo(cards.Length));
            Assert.That(cards.SelectMany(c=>c.GetAbilities().Select(a=>a.Id)).Count(), Is.EqualTo(cards.Length));
        }

    }
}