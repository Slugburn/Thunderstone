using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Messages;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class GameBoardModel
    {
        public DungeonModel Dungeon { get; set; }

        public VillageSectionModel HeroDecks { get; set; }

        public VillageSectionModel WeaponDecks { get; set; }

        public VillageSectionModel ItemDecks { get; set; }

        public VillageSectionModel SpellDecks { get; set; }

        public VillageSectionModel VillagerDecks { get; set; }

        public IList<CardModel> Hand { get; set; }

        public StatusModel Status { get; set; }

        public static GameBoardModel From(Player player)
        {
            var game = player.Game;
            var heroDecks = VillageSectionModel.Create(game,"Guilds and Taverns", CardType.Hero);
            var weaponDecks = VillageSectionModel.Create(game, "Armory", CardType.Weapon);
            var itemDecks = VillageSectionModel.Create(game, "Marketplace", CardType.Item);
            var spellDecks = VillageSectionModel.Create(game, "Wizard's College", CardType.Spell);
            var villagerDecks = VillageSectionModel.Create(game, "Town Square", CardType.Villager);
            var hand = player.Hand;
            return new GameBoardModel
                {
                    Dungeon = DungeonModel.From(game.Dungeon),
                    HeroDecks = heroDecks,
                    WeaponDecks = weaponDecks,
                    ItemDecks = itemDecks,
                    SpellDecks = spellDecks,
                    VillagerDecks = villagerDecks,
                    Hand = CardModel.From(hand),
                    Status = StatusModel.From(player),
                };
        }
    }
}