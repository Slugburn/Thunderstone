using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class SelectCardsMessage
    {
        public string Caption { get; set; }

        public string Message { get; set; }

        public IList<CardModel> Cards { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }
    }
}
