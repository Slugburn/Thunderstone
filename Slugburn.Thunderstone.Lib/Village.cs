using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Randomizers;

namespace Slugburn.Thunderstone.Lib
{
    public class Village
    {
        private readonly Dictionary<long, Deck> _decksById;
        private readonly ILookup<CardType, Deck> _decksByType;

        public Village(IEnumerable<IRandomizer> randomizers)
        {
            var basicDecks = CreateBasicDecks(3);
            var randomizerDecks = randomizers.Select(x => Deck.Create(x.CreateCards()));
            var decks = basicDecks.Concat(randomizerDecks).ToArray();

            // Set owner of all cards in village
            decks.SelectMany(d => d.GetCards()).Each(c => c.Owner = CardOwner.Village);

            _decksById = decks.ToDictionary(x => x.Id);
            _decksByType = decks.ToLookup(x => x.TopCard.Type);
        }

        private static IEnumerable<Deck> CreateBasicDecks(int count)
        {
            return new[]
                       {
                           Deck.Create(new Regular().Create(count)),
                           Deck.Create(new Longspear().Create(count) ),
                           Deck.Create(new Torch().Create(count))
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
