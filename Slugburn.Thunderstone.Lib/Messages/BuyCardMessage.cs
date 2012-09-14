using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class BuyCardMessage
    {
        public IEnumerable<long> AvailableDecks { get; set; }

        public static BuyCardMessage From(IEnumerable<Deck> availableDecks)
        {
            return new BuyCardMessage {AvailableDecks = availableDecks.Select(x => x.Id)};
        }
    }
}