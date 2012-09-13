using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.BasicCards;

namespace Slugburn.Thunderstone.Lib
{
    public class Deck
    {
        private List<Card> _cards = new List<Card>();

        public Deck()
        {
            Id = UniqueId.Next();
        }

        public Deck(IEnumerable<Card> cards) : this()
        {
            Add(cards);
        }

        public long Id { get; private set; }

        public void Shuffle()
        {
            _cards = _cards.Shuffle();
        }

        public void Add(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public void Add(Card card)
        {
            _cards.Add(card);
        }

        public static Deck Create(IEnumerable<Card> cards)
        {
            return new Deck {_cards = cards.ToList()};
        }

        public Card TopCard
        {
            get { return _cards.Count > 0 ? _cards[0] : null; }
        }

        public int Count
        {
            get { return _cards.Count; }
        }

        public List<Card> Draw(int count)
        {
            var drawn = _cards.Take(count).ToList();
            _cards.RemoveRange(0, count);
            return drawn;
        }

        public Card Draw()
        {
            return Draw(1)[0];
        }

        public object CreateMessage()
        {
            return new { Id, TopCard = TopCard.CreateMessage(), Count };
        }

        public void AddToTop(Card card)
        {
            _cards.Insert(0, card);
        }

        public IEnumerable<Card> GetCards()
        {
            return new List<Card>(_cards);
        }

        public void Remove(Card card)
        {
            _cards.Remove(card);
        }
    }
}