using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Monsters
{
    [TestFixture]
    public class UndeadSkeletonTest
    {
        private Game _game;
        private Player _player;
        private SelectCardsMessage _message;

        [Test]
        public void Ossuous_cannot_be_attacked_if_no_heroes_of_at_least_level_1_are_present()
        {
            // Arrange
            GivenGame();
            var ossuous = GivenOssuousInHall();
            GivenSelectCardsMessageExpected();

            // Act
            Assert.That(_game.Dungeon.Ranks[0].Card, Is.SameAs(ossuous));
            _player.SelectMonster();

            // Assert
            Assert.That(_message.Cards.Any(x => x.Id == ossuous.Id), Is.False, "Valid targets should not contain Ossuous");
        }

        [Test]
        public void Ossuous_can_be_attacked_if_hero_of_at_least_level_1_is_present()
        {
            // Arrange
            GivenGame();
            var ossuous = GivenOssuousInHall();
            GivenSelectCardsMessageExpected();
            var hero = GivenLevelOneHero();
            _player.AddCardToHand(hero);

            // Act
            Assert.That(_game.Dungeon.Ranks[0].Card, Is.SameAs(ossuous));
            _player.SelectMonster();

            // Assert
            Assert.That(_message.Cards.Any(x => x.Id == ossuous.Id), Is.True, "Valid targets should contain Ossuous");
        }

        private void GivenSelectCardsMessageExpected()
        {
            _message = null;
            _player.View.Stub(x => x.SelectCards(null)).IgnoreArguments().WhenCalled(inv => _message = (SelectCardsMessage) inv.Arguments[0]);
        }

        private void GivenGame()
        {
            _game = TestFactory.CreateGame();
            _player = _game.CurrentPlayer;
            _player.DiscardHand();
        }

        private Card GivenOssuousInHall()
        {
            var ossuous = new UndeadSkeleton().CreateCards().First(x => x.Name == "Ossuous");
            _game.Dungeon.AddToTopOfDeck(ossuous);
            while (_game.Dungeon.Ranks[0].Card == null)
                _game.AdvanceDungeon();
            return ossuous;
        }

        private Card GivenLevelOneHero()
        {
            return _game.Village[CardType.Hero].First(x => x.TopCard.Level == 1).Draw();
        }
    }
}
