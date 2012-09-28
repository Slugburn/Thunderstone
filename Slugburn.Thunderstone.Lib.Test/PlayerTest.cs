using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Messages;

namespace Slugburn.Thunderstone.Lib.Test
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void When_destroying_curse_then_it_is_added_back_to_deck()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            var curse = game.Curses.Draw();
            player.AddCardToHand(curse);

            // Act
            player.DestroyCard(curse, "test");

            // Assert
            Assert.That(game.Curses.GetCards().Last(), Is.SameAs(curse));
        }

        [Test]
        public void When_removing_equipped_weapon_from_hand_then_hero_loses_modifier()
        {
            // Arrange
            var context = new TestContext();
            var player = context.Player;
            var regular = context.CreateBasicCard<Regular>();
            var spear = context.CreateBasicCard<Longspear>();
            context.SetPlayerHand(regular, spear);
            context.SetPlayerState(Phase.Dungeon, Phase.Equip);
            context.HeroEquipsWeapon(regular, spear);

            // Act
            player.RemoveFromHand(spear);

            // Assert
            Assert.That(regular.PhysicalAttack, Is.EqualTo(1));
            player.View.AssertWasCalled(x => x.Log("Regular no longer has Longspear equipped."));
        }

        [Test]
        public void When_leveling_hero_then_level_up_is_not_required()
        {
            // Arrange
            var game = TestFactory.CreateGame();
            var player = game.CurrentPlayer;
            player.Xp = 10;
            var hero = game.Village[CardType.Hero].First().Draw();
            player.AddCardToHand(hero);
            SelectCardsMessage message = null;
            player.View.Stub(v => v.SelectCards(Arg<SelectCardsMessage>.Is.Anything))
                .WhenCalled(inv =>
                    {
                        message = (SelectCardsMessage) inv.Arguments[0];
                    });

            // Act
            player.LevelHeroes();
            // simulate selection callback
            player.SelectCardsCallback(new long[0]);

            // Assert
            Assert.That(message, Is.Not.Null);
            Assert.That(message.Min, Is.EqualTo(0));
        }
    }
}
