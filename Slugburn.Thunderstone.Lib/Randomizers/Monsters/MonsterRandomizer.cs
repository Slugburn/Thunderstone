using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public abstract class MonsterRandomizer : RandomizerBase
    {
        public int Level { get; set; }

        protected MonsterRandomizer(int level, params string[] tags) : base(CardType.Monster, tags.ConcatTags(), tags)
        {
            Level = level;
            Tags = tags;
        }

        public override CardInfo GetInfo()
        {
            return new CardInfo
                       {
                           Name = Name,
                           Tags = new[] { "Monster Group", string.Format("Level {0}", Level) }.ConcatTags(),
                           Text = Text
                       };
        }

        public override IEnumerable<Card> CreateCards()
        {
            return MonsterDefs.SelectMany(def => Enumerable.Range(0, def.Count).Select(x => CreateCard(def)));
        }

        private Card CreateCard(MonsterDef def)
        {
            var card = new Card
                           {
                               Type = CardType.Monster, 
                               Name = def.Name,
                               Health = def.Health,
                               Gold = def.Gold,
                               Text = def.Text,
                               Xp = def.Xp,
                               Vp = def.Vp,
                           };
            card.SetTags(Tags);
            card.SetTags("Level {0}".Template(Level));
            if (def.Modify != null)
                def.Modify(card);

            return card;
        }

        protected abstract IEnumerable<MonsterDef> MonsterDefs { get; } 
    }
}
