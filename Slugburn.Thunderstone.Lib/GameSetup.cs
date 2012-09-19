using System;
using System.Linq;
using Slugburn.Thunderstone.Lib.Randomizers;
using Slugburn.Thunderstone.Lib.Randomizers.Heroes;
using Slugburn.Thunderstone.Lib.Randomizers.Items;
using Slugburn.Thunderstone.Lib.Randomizers.Monsters;
using Slugburn.Thunderstone.Lib.Randomizers.Spells;
using Slugburn.Thunderstone.Lib.Randomizers.ThunderstoneBearers;
using Slugburn.Thunderstone.Lib.Randomizers.Villagers;
using Slugburn.Thunderstone.Lib.Randomizers.Weapons;

namespace Slugburn.Thunderstone.Lib
{
    public class GameSetup
    {
        private readonly IRandomizer[] _randomizers;

        public GameSetup()
        {
            _randomizers = new IRandomizer[]
                               {
                                   new Stramst(),
                                   new BurnmarkedFire(),
                                   new KoboldHumanoid(), 
                                   new UndeadSkeleton(), 
                                   new Whetmage(),
                                   new Criochan(),
                                   new Drua(),
                                   new Thundermage(),
                                   new FalconArbalest(),
                                   new SnakeheadFlail(),
                                   new KingCaelansWrit(),
                                   new Moonstone(),
                                   new MassTeleport(),
                                   new SummonStorm(),
                                   new BattlescarredSoldier(),
                                   new BountyHunter()
                               };
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
            return new Dungeon(thunderstoneBearer, monsters);
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
