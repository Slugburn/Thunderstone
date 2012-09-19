﻿using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Curses;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Curses
{
    [TestFixture]
    public class CurseOfDecayTest
    {
        [Test]
        public void When_whetmage_levels_other_hero_then_curse_of_decay_ability_is_no_longer_available()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfDecay>();
            var whetmage = context.CreateCard<Whetmage>();
            var hero1 = context.GivenHeroFromTopOfDeck(x => x.Level == 1);
            var hero2 = context.GivenHeroFromTopOfDeck(x => x.Level == 1);
            context.GivenPlayerHand(curse, whetmage, hero1, hero2);
            player.Xp = 2;

            var levelOtherHero = whetmage.GetAbilities().First();
            var curseAbility = curse.GetAbilities().First();

            // Act
            context.WhenUsingAbility(levelOtherHero);

            // Assert
            Assert.That(player.ActiveAbilities, Has.No.Member(curseAbility));
        }

        [Test]
        public void When_equipping_a_weapon_then_curse_of_decay_ability_is_still_available()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var curse = context.CreateCard<CurseOfDecay>();
            var regular = context.CreateBasicCard<Regular>();
            var longspear = context.CreateBasicCard<Longspear>();
            context.GivenPlayerHand(curse, regular, longspear);
            player.State = PlayerState.Dungeon;

            var equipLongspear = longspear.GetAbilities().First();
            var curseAbility = curse.GetAbilities().First();

            // Act
            context.WhenUsingAbility(equipLongspear);

            // Assert
            Assert.That(player.ActiveAbilities, Has.Member(curseAbility));
        }

    }
}
