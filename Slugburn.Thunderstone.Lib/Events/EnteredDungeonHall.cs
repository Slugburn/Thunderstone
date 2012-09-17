namespace Slugburn.Thunderstone.Lib.Events
{
    public class EnteredDungeonHall
    {
        public Card Card { get; set; }

        public EnteredDungeonHall(Card card)
        {
            Card = card;
        }
    }
}
