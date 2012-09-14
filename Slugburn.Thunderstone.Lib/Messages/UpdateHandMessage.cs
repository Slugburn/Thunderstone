using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class UpdateHandMessage
    {
        public IList<CardModel> Hand { get; set; }

        public static UpdateHandMessage From(IList<Card> hand)
        {
            return new UpdateHandMessage {Hand = CardModel.From(hand)};
        }
    }
}
