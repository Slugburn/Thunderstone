using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class CardModel
    {
        public int? Gold { get; set; }

        public string Name { get; set; }

        public int? Cost { get; set; }

        public int? Health { get; set; }

        public long Id { get; set; }

        public int Level { get; set; }

        public int? Light { get; set; }

        public int? MagicAttack { get; set; }

        public int? PhysicalAttack { get; set; }

        public int? PotentialMagicAttack { get; set; }

        public int? PotentialPhysicalAttack { get; set; }

        public int? Rank { get; set; }

        public int? Strength { get; set; }

        public string Tags { get; set; }

        public string Text { get; set; }

        public CardType Type { get; set; }

        public int? Vp { get; set; }

        public int? Xp { get; set; }

        public static CardModel From(Card card)
        {
            return new CardModel
                {
                    Name = card.Name,
                    Cost = card.Cost,
                    Gold = card.Gold,
                    Health = card.Health,
                    Id = card.Id,
                    Level = card.Level,
                    Light = card.Light,
                    MagicAttack = card.MagicAttack,
                    PhysicalAttack = card.PhysicalAttack,
                    PotentialMagicAttack = card.PotentialMagicAttack,
                    PotentialPhysicalAttack = card.PotentialPhysicalAttack,
                    Rank = card.Rank,
                    Strength = card.Strength,
                    Tags = card.Tags,
                    Text = card.Text,
                    Type = card.Type,
                    Vp = card.Vp,
                    Xp = card.Xp
                };
        }

        public static IList<CardModel> From(IEnumerable<Card> cards)
        {
            return cards.Select(From).ToArray();
        }
    }
}