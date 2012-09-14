using System;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class GameSetupModel
    {
        public CardInfo ThunderstoneBearer { get; set; }

        public CardInfo[] Monsters { get; set; }

        public CardInfo[] Heroes { get; set; }

        public CardInfo[] Weapons { get; set; }

        public CardInfo[] Items { get; set; }

        public CardInfo[] Spells { get; set; }

        public CardInfo[] Villagers { get; set; }

        public static GameSetupModel From(GameSetup setup)
        {
            var byType = setup.GetRandomizersByType();
            Func<CardType, CardInfo[]> getInfoByType = type => byType[type].Select(x => x.GetInfo()).ToArray();
            var model = new GameSetupModel
                {
                    ThunderstoneBearer = getInfoByType(CardType.ThunderstoneBearer)[0],
                    Monsters = getInfoByType(CardType.Monster),
                    Heroes = getInfoByType(CardType.Hero),
                    Weapons = getInfoByType(CardType.Weapon),
                    Items = getInfoByType(CardType.Item),
                    Spells = getInfoByType(CardType.Spell),
                    Villagers = getInfoByType(CardType.Villager)
                };
            return model;
        }
    }
}