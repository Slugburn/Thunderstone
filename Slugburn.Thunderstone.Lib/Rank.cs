namespace Slugburn.Thunderstone.Lib
{
    public class Rank
    {
        private readonly Dungeon _dungeon;

        public Rank(Dungeon dungeon, int number, int darkness)
        {
            _dungeon = dungeon;
            Number = number;
            Darkness = darkness;
        }

        private Card _card;
        
        public int Number { get; private set; }
        public int Darkness { get; private set; }

        public Card Card
        {
            get { return _card; }
            set
            {
                _card = value;
                if (_card != null)
                    _card.Rank = this;
            }
        }

        public Rank NextRank 
        {
            get { return _dungeon.GetRankNumber(Number - 1); }
        }

        public Rank PreviousRank
        {
            get { return _dungeon.GetRankNumber(Number + 1); }
        }

        public override string ToString()
        {
            return "Rank {0}".Template(Number);
        }

        public void RemoveCard()
        {
            var removedCard = Card;
            if (removedCard != null)
                removedCard.Reset();
            Card = null;
        }
    }
}