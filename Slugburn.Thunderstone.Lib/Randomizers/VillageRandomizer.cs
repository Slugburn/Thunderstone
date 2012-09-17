using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Randomizers
{
    abstract class VillageRandomizer : RandomizerBase
    {
        protected VillageRandomizer(CardType type, string name, params string[] additionalTags)
            : base(type, name, new[] { type.ToString() }.Concat(additionalTags).ToArray())
        {
        }

        public int? Gold { get; set; }
        public int? Cost { get; set; }
        public int? Light { get; set; }
        public int? Strength { get; set; }
        public int? Vp { get; set; }

        public override CardInfo GetInfo()
        {
            var info = base.GetInfo();
            info.Gold = Gold;
            info.Cost = Cost;
            info.Light = Light;
            info.Strength = Strength;
            return info;
        }

        public override IEnumerable<Card> CreateCards(Game game)
        {
            return Enumerable.Range(0, 8).Select(x => CreateCard(game));
        }

        private Card CreateCard(Game game)
        {
            var card = new Card(game)
                           {
                               Type = Type,
                               Cost = Cost,
                               Gold = Gold,
                               Light = Light,
                               Name = Name,
                               Strength = Strength,
                               Text = Text,
                               Vp = Vp,
                           };
            card.SetTags(Tags);
            Modify(card);
            return card;
        }

        protected virtual void Modify(Card card)
        {
            // do nothing by default
        }
    }
}
