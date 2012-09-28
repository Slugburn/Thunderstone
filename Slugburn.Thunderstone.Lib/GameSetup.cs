using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Randomizers;

namespace Slugburn.Thunderstone.Lib
{
    public class GameSetup
    {
        private readonly IRandomizer[] _randomizers;
        private RandomizerStore _store;

        public GameSetup()
        {
            _store = RandomizerStore.Instance;
            _randomizers =  RandomThunderstoneBearers()
                .Concat(RandomMonsters())
                .Concat(RandomHeroes())
                .Concat(RandomVillage())
                .ToArray();
        }

        private IEnumerable<IRandomizer> RandomThunderstoneBearers()
        {
            return new[] { _store[CardType.ThunderstoneBearer].PickRandom() };
        }

        private IEnumerable<IRandomizer> RandomMonsters()
        {
            return _store[CardType.Monster].Shuffle().Take(3);
        }

        private IEnumerable<IRandomizer> RandomHeroes()
        {
            return _store[CardType.Hero].Shuffle().Take(4);
        }

        private IEnumerable<IRandomizer> RandomVillage()
        {
            var maxCount = new Dictionary<CardType, int>
                {
                    {CardType.Weapon, 3},
                    {CardType.Item, 2},
                    {CardType.Spell, 3},
                    {CardType.Villager, 3}
                };
            var shuffled = _store[CardType.Weapon]
                .Concat(_store[CardType.Item])
                .Concat(_store[CardType.Spell])
                .Concat(_store[CardType.Villager])
                .Shuffle();
            var list = new List<IRandomizer>();

            var i = 0;
            while(list.Count < 8)
            {
                var rand = shuffled[i];
                var type = rand.Type;
                if (list.Count(x=>x.Type==type) < maxCount[type])
                    list.Add(rand);
                i++;
            }
            return list;
        }

        public ILookup<CardType, IRandomizer> GetRandomizersByType()
        {
            return _randomizers.ToLookup(x => x.Type);
        }

        public IRandomizer GetRandomizer(string name)
        {
            return _randomizers.SingleOrDefault(x => x.Name == name);
        }

        public void CreateGame(Game game)
        {
            game.Players.Each(x=>x.StartGame(game));
            game.CurrentPlayer = game.Players.PickRandom();
            game.Dungeon = CreateDungeon(game);
            game.Village = CreateVillage(game);
            game.Curses = CreateCurses(game);
        }

        private static Deck CreateCurses(Game game)
        {
            var cards = RandomizerStore.Instance[CardType.Curse].SelectMany(x => x.CreateCards(game));
            return new Deck(cards.Shuffle());
        }

        private Dungeon CreateDungeon(Game game)
        {
            var thunderstoneBearer = _randomizers.Single(x => x.Type == CardType.ThunderstoneBearer).CreateCards(game).Single();
            var monsters = _randomizers.Where(x => x.Type == CardType.Monster).SelectMany(x => x.CreateCards(game));
            return new Dungeon(game, thunderstoneBearer, monsters);
        }

        private Village CreateVillage(Game game)
        {
            var randomizers = _randomizers
                .Where(x=>x.Type==CardType.Hero 
                    || x.Type == CardType.Weapon 
                    || x.Type == CardType.Item 
                    || x.Type== CardType.Spell 
                    || x.Type==CardType.Villager);
            return new Village(game, randomizers);
        }

    }
}
