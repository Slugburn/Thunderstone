using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers;

namespace Slugburn.Thunderstone.Lib
{
    public class Village
    {
        public Game Game { get; private set; }
        private readonly Dictionary<long, Deck> _decksById;
        private readonly ILookup<CardType, Deck> _decksByType;

        public Village(Game game, IEnumerable<IRandomizer> randomizers)
        {
            Game = game;
            var basicDecks = CreateBasicDecks(3);
            var randomizerDecks = randomizers.Select(x => Deck.Create(x.CreateCards(Game)));
            var decks = basicDecks.Concat(randomizerDecks).ToArray();

            // Set owner of all cards in village
            decks.SelectMany(d => d.GetCards()).Each(c => c.Owner = CardOwner.Village);

            _decksById = decks.ToDictionary(x => x.Id);
            _decksByType = decks.ToLookup(x => x.TopCard.Type);
        }

        private IEnumerable<Deck> CreateBasicDecks(int count)
        {
            return new[]
                       {
                           Deck.Create(new Regular().Create(Game, count)),
                           Deck.Create(new Longspear().Create(Game, count) ),
                           Deck.Create(new Torch().Create(Game, count))
                       };
        }


        public Deck this[long id]
        {
            get { return _decksById[id]; }
        }

        public IEnumerable<Deck> this[CardType type]
        {
            get { return _decksByType[type]; }
        }

        public IEnumerable<Deck> Decks
        {
            get { return _decksById.Values; }
        }

    }
}
