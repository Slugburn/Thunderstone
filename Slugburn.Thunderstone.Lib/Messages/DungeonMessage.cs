using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class DungeonMessage
    {
        public IList<dynamic> Ranks { get; set; }

        public int DeckCount { get; set; }
    }
}
