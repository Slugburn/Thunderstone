using NUnit.Framework;

namespace Slugburn.Thunderstone.Lib.Test.Abilities
{
    [TestFixture]
    public class AbilityTest
    {
        private Game _game;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _game = new Game();
        }

        [Test]
        [TestCase(Phase.Village, true)]
        [TestCase(Phase.Dungeon, true)]
        [TestCase(Phase.Battle, false)]
        [TestCase(Phase.Aftermath, true)]
        [TestCase(Phase.Spoils, true)]
        [TestCase(Phase.Trophy, true)]
        public void Is_usable_when_owned_by_player(Phase phase, bool expected)
        {
            // Arrange
            var ability = new Ability(phase, "test", player => { }) {Card = new Card(_game) {Owner = CardOwner.Player}};

            // Act
            var isUsable = ability.IsUsableByOwner();

            // Assert
            Assert.That(isUsable, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(Phase.Village, false)]
        [TestCase(Phase.Dungeon, false)]
        [TestCase(Phase.Battle, true)]
        [TestCase(Phase.Aftermath, true)]
        [TestCase(Phase.Spoils, false)]
        [TestCase(Phase.Trophy, true)]
        public void Is_usable_when_owned_by_dungeon(Phase phase, bool expected)
        {
            // Arrange
            var ability = new Ability(phase, "test", player => { }) {Card = new Card(_game) {Owner = CardOwner.Dungeon}};

            // Act
            var isUsable = ability.IsUsableByOwner();

            // Assert
            Assert.That(isUsable, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(Phase.Aftermath, CardOwner.Dungeon, true)]
        [TestCase(Phase.Aftermath, CardOwner.Player, false)]
        [TestCase(Phase.Spoils, CardOwner.Dungeon, true)]
        [TestCase(Phase.Spoils, CardOwner.Player, false)]
        public void Some_monster_abilities_are_not_usable_from_hand(Phase phase, CardOwner owner, bool expected)
        {
            // Arrange
            var ability = new Ability(Phase.Spoils, "test", player => { }) { Card = new Card(_game) { Type = CardType.Monster, Owner = owner } };

            // Act
            var isUsable = ability.IsUsableByOwner();

            // Assert
            Assert.That(isUsable, Is.EqualTo(expected));
        }

    }
}
