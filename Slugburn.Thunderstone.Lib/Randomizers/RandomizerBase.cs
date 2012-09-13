using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Randomizers
{
    public abstract class RandomizerBase : IRandomizer
    {
        protected RandomizerBase(CardType type, string name, string[] tags)
        {
            Type = type;
            Name = name;
            Tags = tags;
        }

        public string Name { get; set; }

        public string[] Tags { get; set; }

        public string Text { get; set; }

        public CardType Type { get; private set; }

        public virtual CardInfo GetInfo()
        {
            return new CardInfo
            {
                Name = Name,
                Tags = Tags.ConcatTags(),
                Text = Text
            };
        }

        public abstract IEnumerable<Card> CreateCards();
    }
}
