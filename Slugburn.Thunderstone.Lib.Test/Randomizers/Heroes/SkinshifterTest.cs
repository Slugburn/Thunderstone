using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class SkinshifterTest
    {
        [Test]
        public void Terror_can_destroy_card()
        {
            // Arrange
            var context = new TestContext();
            var terror = context.CreateCard<Skinshifter>("Terror");
            var otherCard = context.CreateBasicCard<Torch>();
            context.SetPlayerHand(terror, otherCard);
            context.SetPlayerState(Phase.Dungeon);
            context.WhenSelectingOptionSelect("Destroy");

            // Act
            context.UseAbilityOf(terror);

            // Assert
            Assert.That(terror.PhysicalAttack, Is.EqualTo(8));
            Assert.That(terror.Strength, Is.EqualTo(8));
            Assert.That(context.Player.Hand, Has.No.Member(otherCard));
            Assert.That(context.Player.Discard, Has.No.Member(otherCard));
        }

        [Test]
        public void Terror_can_discard_card()
        {
            // Arrange
            var context = new TestContext();
            var terror = context.CreateCard<Skinshifter>("Terror");
            var otherCard = context.CreateBasicCard<Torch>();
            context.SetPlayerHand(terror, otherCard);
            context.SetPlayerState(Phase.Dungeon);
            context.WhenSelectingOptionSelect("Discard");

            // Act
            context.UseAbilityOf(terror);

            // Assert
            Assert.That(terror.PhysicalAttack, Is.EqualTo(8));
            Assert.That(terror.Strength, Is.EqualTo(8));
            Assert.That(context.Player.Hand, Has.No.Member(otherCard));
            Assert.That(context.Player.Discard, Has.Member(otherCard));
        }
    }
}
