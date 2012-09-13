using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public abstract class HeroRandomizer : RandomizerBase
    {
        protected HeroRandomizer(string name, params string[] tags) :base(CardType.Hero, name, tags)
        {
            Name = name;
        }

        public override IEnumerable<Card> CreateCards()
        {
            return HeroDefs
                .OrderBy(x => x.Level)
                .SelectMany(def => Enumerable.Range(0, def.Count).Select(x => CreateCard(def)));
        }

        protected abstract IEnumerable<HeroDef> HeroDefs { get; }

        private Card CreateCard(HeroDef def)
        {
            var card = new Card
                           {
                               Level = def.Level,
                               Type = CardType.Hero,
                               Name = "{0} {1}".Template(Name, def.Name),
                               Cost = def.Cost,
                               Gold = def.Gold,
                               Light = def.Light,
                               MagicAttack = def.MagicAttack,
                               PhysicalAttack = def.PhysicalAttack,
                               Strength = def.Strength,
                               Text = def.Text,
                               Vp = def.Vp,
                               Xp = def.Xp,
                           };
            card.SetTags(Tags);
            card.SetTags("Level {0}".Template(def.Level));
            if (def.Modify != null)
                def.Modify(card);
            return card;
        }

    }
}
