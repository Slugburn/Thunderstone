using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class DungeonModel
    {
        public IList<RankModel> Ranks { get; set; }

        public int DeckCount { get; set; }

        public static DungeonModel From(Dungeon dungeon)
        {
            return new DungeonModel
                {
                    Ranks = RankModel.From(dungeon.Ranks),
                    DeckCount = dungeon.Deck.Count
                };
        }
    }
}
