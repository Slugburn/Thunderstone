using System;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class GameSetupHandler : MessageHandlerBase
    {
        public GameSetupHandler() : base("GameSetup")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            var setup = new GameSetup();
            player.Session.Setup = setup;
            var byType = setup.GetRandomizersByType();
            Func<CardType, CardInfo[]> getInfoByType = type => byType[type].Select(x => x.GetInfo()).ToArray();
            var body = new
            {
                ThunderstoneBearer = getInfoByType(CardType.ThunderstoneBearer)[0],
                Monsters = getInfoByType(CardType.Monster),
                Heroes = getInfoByType(CardType.Hero),
                Weapons = getInfoByType(CardType.Weapon),
                Items = getInfoByType(CardType.Item),
                Spells = getInfoByType(CardType.Spell),
                Villagers = getInfoByType(CardType.Villager)
            };
            player.Session.SendAll("GameSetup", body);
        }
    }
}
