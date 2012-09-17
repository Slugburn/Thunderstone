using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Randomizers
{
    public interface IRandomizer
    {
        CardType Type { get; }

        string Name { get; }

        CardInfo GetInfo();
        
        IEnumerable<Card> CreateCards(Game game);
    }
}
