using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class DisownedTest
    {
        [Test]
        public void Beserker_destroys_other_hero_during_aftermath()
        {
            // Arrange
            var context = new TestContext();
            var berserker = context.CreateCard<Disowned>("Disowned Berserker");
            var otherHero = context.CreateBasicCard<Regular>();
            var weapon = context.CreateCard<Pike>();
            context.SetPlayerHand(berserker, otherHero, weapon);
            context.SetTestPlayerState(Phase.Aftermath);

            // Act
            context.UseAbilityOf(berserker);

            // Assert
            Assert.That(context.Player.Hand, Is.EquivalentTo(new[]{berserker, weapon}));
        }

        [Test]
        public void Bloodrager_can_equip_two_weapons()
        {
            // Arrange
            var context = new TestContext();
            var bloodrager = context.CreateCard<Disowned>("Disowned Bloodrager");
            var weapon1 = context.CreateCard<Pike>();
            var weapon2 = context.CreateCard<SnakeheadFlail>();
            context.SetPlayerHand(bloodrager, weapon1, weapon2);
            context.SetTestPlayerState(Phase.Dungeon);

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

        [Test]
        public void Bloodrager_destroys_all_other_heroes_during_aftermath()
        {
            // Arrange
            var context = new TestContext();
            var bloodrager = context.CreateCard<Disowned>("Disowned Bloodrager");
            var hero1 = context.CreateBasicCard<Regular>();
            var hero2 = context.CreateBasicCard<Regular>();
            var weapon = context.CreateCard<Pike>();
            context.SetPlayerHand(bloodrager, hero1, hero2, weapon);
            context.SetTestPlayerState(Phase.Aftermath);

            // Act
            context.UseAbilityOf(bloodrager);

            // Assert
            Assert.That(context.Player.Hand, Is.EquivalentTo(new[] {weapon}));
        }

        [Test]
        public void Bloodrager_returns_to_top_of_deck_during_aftermath()
        {
            // Arrange
            var context = new TestContext();
            var bloodrager = context.CreateCard<Disowned>("Disowned Bloodrager");
            var weapon = context.CreateCard<Pike>();
            context.SetPlayerHand(bloodrager, weapon);
            context.HeroEquipsWeapon(bloodrager,weapon);
            context.SetTestPlayerState(Phase.Aftermath);

            // Act
            context.UseAbilityOf(bloodrager);

            // Assert
            Assert.That(context.Player.Hand, Is.EquivalentTo(new[] {weapon}));
            Assert.That(context.Player.Deck.TopCard, Is.SameAs(bloodrager));
            Assert.That(bloodrager.TotalAttack, Is.EqualTo(bloodrager.GetBaseValue(Attr.PhysicalAttack)));
        }
    }
}