using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Selectors.Sources
{
    public class TopOfDeckSource : ICardSource
    {
        private readonly CardType _type;
        private readonly IEnumerable<Deck> _decks;
        private readonly bool _all;

        public TopOfDeckSource(Player player, CardType type)
        {
            Player = player;
            _type = type;
            _decks = Player.Game.Village.Decks;
            _all = false;
        }

        public TopOfDeckSource(Player player)
        {
            Player = player;
            _decks = Player.Game.Village.Decks;
            _all = true;
        }

        public IEnumerable<Card> GetCards()
        {
            return _decks.Select(x => x.TopCard)
                .Where(x => x != null && (_all || x.Type == _type))
                .OrderByDescending(x=>x.Cost);
        }

        public void Destroy(IEnumerable<Card> cards, string source)
        {
            GetSelectedDecks(cards).Each(deck => DrawCardFromDeck(deck));
        }

        private Card DrawCardFromDeck(Deck deck)
        {
            var card = deck.Draw();
            Player.Game.SendUpdateDeck(deck);
            return card;
        }

        public void Draw(IEnumerable<Card> cards)
        {
            GetSelectedDecks(cards).Each(deck => DrawCardFromDeck(deck));
        }

        public void Discard(IEnumerable<Card> cards)
        {
            var cardList = cards.ToList();
            GetSelectedDecks(cardList).Each(deck => DrawCardFromDeck(deck));
            Player.Vp += cardList.Sum(x => x.Vp ?? 0);
            Player.AddToDiscard(cardList);
        }

        public Player Player { get; private set; }

        private IEnumerable<Deck> GetSelectedDecks(IEnumerable<Card> cards)
        {
            return _decks.Where(x => cards.Contains(x.TopCard));
        }
    }
}