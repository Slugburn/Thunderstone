using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public class RandomHandSource : HandSource
    {
        private readonly int _count;
        private readonly Func<Card, bool> _selector;

        public RandomHandSource(Player player, int count, Func<Card, bool> selector) : base(player)
        {
            _count = count;
            _selector = selector;
        }

        public override IEnumerable<Card> GetCards()
        {
            return Player.Hand.Where(_selector).Shuffle().Take(_count);
        }
    }
}