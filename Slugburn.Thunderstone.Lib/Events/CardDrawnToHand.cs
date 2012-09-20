namespace Slugburn.Thunderstone.Lib.Events
{
    public class CardDrawnToHand
    {
        public Card Card { get; set; }

        public CardDrawnToHand(Card card)
        {
            Card = card;
        }
    }
}
