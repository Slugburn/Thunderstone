using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Randomizers.ThunderstoneBearers
{
    public abstract class ThunderstoneBearer : RandomizerBase
    {
        protected ThunderstoneBearer(string name)
            : base(CardType.ThunderstoneBearer, name, new[] { "Guardian", "Thunderstone Bearer" })
        {
        }

        public int Health { get; set; }
        public int Gold { get; set; }
        public int Xp { get; set; }
        public int Vp { get; set; }

        public override CardInfo GetInfo()
        {
            return new CardInfo
                       {
                           Name = Name,
                           Tags = Tags.ConcatTags(),
                           Health = Health,
                           Gold = Gold,
                           Xp = Xp,
                           Vp = Vp,
                           Text = Text
                       };
        }

        public override IEnumerable<Card> CreateCards()
        {
            var card = new Card
                           {
                               Type = CardType.ThunderstoneBearer,
                               Name = Name,
                               Health = Health,
                               Gold = Gold,
                               Text = Text,
                               Xp = Xp,
                               Vp = Vp
                           };
            card.SetTags(Tags);
            return new[] {card};
        }
    }
}
