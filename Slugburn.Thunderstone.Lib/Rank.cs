namespace Slugburn.Thunderstone.Lib
{
    public class Rank
    {
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

        public Rank(int number, int darkness)
        {
            Number = number;
            Darkness = darkness;
        }

        public void RefillHall(Game game)
        {
            var ranks = game.Dungeon.Ranks;
            var lastRankIndex = ranks.Length - 1;
            var attackedRankIndex = Number - 1;
            for (var i = attackedRankIndex; i < lastRankIndex - 1; i++)
            {
                if (ranks[i].Card == null)
                {
                    ranks[i].Card = ranks[i + 1].Card;
                    ranks[i + 1].Card = null;
                }
            }
            if (ranks[lastRankIndex].Card == null)
                ranks[lastRankIndex].Card = game.Dungeon.Deck.Draw();
        }

        public override string ToString()
        {
            return "Rank {0}".Template(Number);
        }
    }
}