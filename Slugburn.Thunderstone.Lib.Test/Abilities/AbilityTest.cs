using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Slugburn.Thunderstone.Lib.Test.Abilities
{
    [TestFixture]
    public class AbilityTest
    {
        [Test]
        [TestCase(Phase.Village, true)]
        [TestCase(Phase.Dungeon, true)]
        [TestCase(Phase.Battle, false)]
        [TestCase(Phase.Aftermath, false)]
        [TestCase(Phase.Spoils, true)]
        [TestCase(Phase.Trophy, true)]
        public void Is_usable_when_owned_by_player(Phase phase, bool expected)
        {
            // Arrange
            var ability = new Ability(phase, "test", player => { }) {Card = new Card {Owner = CardOwner.Player}};

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
            var ability = new Ability(phase, "test", player => { }) {Card = new Card {Owner = CardOwner.Dungeon}};

            // Act
            var isUsable = ability.IsUsableByOwner();

            // Assert
            Assert.That(isUsable, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(CardOwner.Dungeon, true)]
        [TestCase(CardOwner.Player, false)]
        public void Monster_spoils_abilities_are_not_usable_from_hand(CardOwner owner, bool expected)
        {
            // Arrange
            var ability = new Ability(Phase.Spoils, "test", player => { }) { Card = new Card { Type = CardType.Monster, Owner = owner } };

            // Act
            var isUsable = ability.IsUsableByOwner();

            // Assert
            Assert.That(isUsable, Is.EqualTo(expected));
        }
    }
}
