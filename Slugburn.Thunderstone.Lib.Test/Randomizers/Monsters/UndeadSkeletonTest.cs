﻿using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
{
    [TestFixture]
    public class UndeadSkeletonTest
    {
        [Test]
        public void Ossuous_cannot_be_attacked_if_no_heroes_of_at_least_level_1_are_present()
        {
            // Arrange
            var context = new TestContext();
            var hero = context.DrawHeroFromTopOfDeck(x => x.Level == 0);
            context.SetPlayerHand(hero);
            var ossuous = context.SetMonsterInFirstRank<UndeadSkeleton>("Ossuous");

            // Act
            Assert.That(context.Game.Dungeon.Ranks[0].Card, Is.SameAs(ossuous));
            context.Player.SelectMonster();

            // Assert
            Assert.That(context.SelectCardsIds, Has.No.Member(ossuous.Id), "Valid targets should not contain Ossuous");
        }

        [Test]
        public void Ossuous_can_be_attacked_if_hero_of_at_least_level_1_is_present()
        {
            // Arrange
            var context = new TestContext();
            var hero = context.DrawHeroFromTopOfDeck(x => x.Level == 1);
            context.SetPlayerHand(hero);
            var ossuous = context.SetMonsterInFirstRank<UndeadSkeleton>("Ossuous");

            // Act
            Assert.That(context.Game.Dungeon.Ranks[0].Card, Is.SameAs(ossuous));
            context.Player.SelectMonster();

            // Assert
            Assert.That(context.SelectCardsIds, Has.Member(ossuous.Id), "Valid targets should contain Ossuous");
        }

        [Test]
        public void Necrophidius_trophy_ability()
        {
            // Arrange
            var context = new TestContext();
            var necrophidius = context.CreateCard<UndeadSkeleton>("Necrophidius");
            var hero0 = context.DrawHeroFromTopOfDeck(x => x.Level == 0);
            var hero1 = context.DrawHeroFromTopOfDeck(x => x.Level == 1);
            var hero2 = context.GetHeroFromVillage(x => x.Level == 2);
            context.SetPlayerHand(necrophidius, hero0, hero1, hero2);
            context.SetPlayerState(Phase.Trophy);

            // Act
            context.UseAbilityOf(necrophidius);

            // Assert
            Assert.That(context.SelectCardsIds, Is.EquivalentTo(new[] {hero0.Id, hero1.Id}));
        }
    }
}
