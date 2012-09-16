using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class WhetmageTest
    {
        [Test]
        public void Level_up_other_hero()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.State = PlayerState.Dungeon;
            player.DiscardHand();
            var honer = new Whetmage().CreateCards().First(c => c.Level == 1);
            var otherHero = game.Village[CardType.Hero].First(x => x.TopCard.Level > 0).Draw();
            player.AddCardsToHand(new[] {honer, otherHero});
            var ability = honer.GetAbilities().First();
            player.Xp = 2;

            // Fill up the dungeon so the Whetmage has someone to battle
            while(game.Dungeon.Ranks[0].Card==null)
                game.AdvanceDungeon();

            // Act
            Assert.That(ability.IsUsableByOwner() && ability.Condition(player), "Whetmage ability should be usable");
            ability.Action(player);

            // Assert
            Assert.That(player.Hand.Count, Is.EqualTo(2));
            Assert.That(player.Hand.Contains(otherHero), Is.False);
            Assert.That(player.Hand.Single(c=>c != honer).Level, Is.EqualTo(2));
        }

        [Test]
        public void Polisher_can_use_ability_multiple_times()
        {
            // Arrange
            var context = new TestContext();
            var polisher = context.CreateCard<Whetmage>("Whetmage Polisher");
            var hero1 = context.GivenHeroFromTopOfDeck(x=>x.Level==1);
            var hero2 = context.GivenHeroFromTopOfDeck(x => x.Level == 1);
            context.GivenPlayerHand(polisher, hero1, hero2);
            context.Player.Xp = 10;
            var levelUp = polisher.GetAbility();

            // Act
            context.WhenUsingAbility(levelUp);

            // Assert
            Assert.That(context.Player.ActiveAbilities.Any(x=>x.Id == levelUp.Id));
        }

        [Test]
        public void Polisher_cannot_use_ability_to_level_up_card_it_already_leveled_up()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            player.State = PlayerState.Dungeon;
            var polisher = context.CreateCard<Whetmage>("Whetmage Polisher");
            var hero = context.GivenHeroFromTopOfDeck(x => x.Level == 1);
            context.GivenPlayerHand(polisher, hero);
            player.Xp = 10;
            var levelUp = polisher.GetAbility();

            // Act
            context.WhenUsingAbility(levelUp);

            // Assert
            Assert.That(levelUp.Condition(player), Is.False);
        }
    }
}
