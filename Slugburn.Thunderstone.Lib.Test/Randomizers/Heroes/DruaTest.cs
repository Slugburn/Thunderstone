using System;
using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json;
using Slugburn.Thunderstone.Lib.MessageHandlers;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;

namespace Slugburn.Thunderstone.Lib.Test.Randomizers.Heroes
{
    [TestFixture]
    public class DruaTest
    {
        [Test]
        public void DungeonDestroyCard()
        {
            var player = new Player(Guid.NewGuid(), (s, o) => { });
            var card = new Drua().CreateCards().First();
            player.AddCardToHand(card);
            var ability = card.GetAbilities(Phase.Dungeon).Single(x => x.Description == "Destroy 1 card in your hand.");
            Assert.That(ability.Condition(player), Is.True);

            var handler = new UseAbilityHandler();
            var message = new Message(player, "UseAbility", JsonConvert.SerializeObject(new UseAbilityHandler.UseAbilityResponse {AbilityId = ability.Id}));
            handler.Handle(message);
        }
    }
}
