using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class RankModel
    {
        public int Number { get; set; }

        public CardModel Card { get; set; }

        public int Penalty { get; set; }

        public static List<RankModel> From(Rank[] ranks)
        {
            return ranks.Select(From).ToList();
        }

        public static RankModel From(Rank rank)
        {
            return new RankModel
                {
                    Number = rank.Number,
                    Card = rank.Card == null ? null : CardModel.From(rank.Card),
                    Penalty = rank.Darkness
                };
        }
    }
}
