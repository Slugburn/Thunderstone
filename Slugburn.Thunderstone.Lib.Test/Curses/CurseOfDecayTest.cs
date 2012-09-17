using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Curses
{
    [TestFixture]
    public class CurseOfDecayTest
    {
        [Test]
        public void When_whetmage_levels_other_hero_then_curse_of_decay_ability_is_no_longer_available()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.DiscardHand();
            var curse = new Curse().CreateCards(game).First(x => x.Name == "Curse of Decay");
            var whetmage = new Whetmage().CreateCards(game).First();
            player.AddCardsToHand(new[] {curse, whetmage});
            var otherHeroes = game.Village[CardType.Hero].First(x => x.TopCard.Level == 1).Draw(2);
            player.AddCardsToHand(otherHeroes);
            player.Xp = 2;

            var levelOtherHero = whetmage.GetAbilities().First();
            var curseAbility = curse.GetAbilities().First();

            // Act
            Assert.That(levelOtherHero.Condition(player), Is.True);
            player.UseAbility(levelOtherHero.Id);

            // Assert
            Assert.That(player.ActiveAbilities, Has.No.Member(curseAbility));
        }

        [Test]
        public void When_equipping_a_weapon_then_curse_of_decay_ability_is_still_available()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.State = PlayerState.Dungeon;
            player.DiscardHand();
            var hero = new Regular().Create(game);
            var longspear = new Longspear().Create(game);
            var curse = new Curse().CreateCards(game).First(x => x.Name == "Curse of Decay");
            player.AddCardsToHand(new[] {hero, longspear, curse});

            var equipLongspear = longspear.GetAbilities().First();
            var curseAbility = curse.GetAbilities().First();

            // Act
            player.UseAbility(equipLongspear.Id);

            // Assert
            Assert.That(player.ActiveAbilities, Has.Member(curseAbility));
        }

    }
}
