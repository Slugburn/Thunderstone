namespace Slugburn.Thunderstone.Lib.Models
{
    public class DeckModel
    {
        public long Id { get; set; }

        public CardModel TopCard { get; set; }

        public int Count { get; set; }

        public static DeckModel From(Deck deck)
        {
            return new DeckModel { Id = deck.Id, TopCard = CardModel.From(deck.TopCard), Count = deck.Count };
        }
    }
}