using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;
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
            var ability = context.GetAbilityOf(terror);

            // Act
            context.UseAbility(ability);

            // Assert
            Assert.That(terror.PhysicalAttack, Is.EqualTo(8));
            Assert.That(terror.Strength, Is.EqualTo(8));
            Assert.That(context.Player.Hand, Has.No.Member(otherCard));
            Assert.That(context.Player.Discard, Has.Member(otherCard));
            Assert.That(context.Player.ActiveAbilities, Has.No.Member(ability));
        }

        [Test]
        public void If_terror_destroys_disease_then_can_use_ability_one_additional_time()
        {
            var context = new TestContext();
            var terror = context.CreateCard<Skinshifter>("Terror");
            var disease1 = context.CreateCard<CurseOfDecay>();
            var disease2 = context.CreateCard<CurseOfDecay>();
            var disease3 = context.CreateCard<CurseOfDecay>();
            context.SetPlayerHand(terror, disease1, disease2, disease3);
            context.SetPlayerState(Phase.Dungeon);
            context.WhenSelectingOptionSelect("Destroy");
            context.WhenSelectingCardsSelectFirst();
            var ability = context.GetAbilityOf(terror);

            // Act
            context.UseAbility(ability);
            context.UseAbilityOf(terror);

            // Assert
            Assert.That(context.Player.Hand, Has.No.Member(disease1));
            Assert.That(context.Player.Hand, Has.No.Member(disease2));
            Assert.That(context.Player.ActiveAbilities, Has.No.Member(ability));
        }
    }
}
