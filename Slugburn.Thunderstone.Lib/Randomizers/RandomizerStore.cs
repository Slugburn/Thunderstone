using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Randomizers
{
    public class RandomizerStore
    {
        private readonly IRandomizer[] _randomizers;
        private readonly Dictionary<CardType, IEnumerable<IRandomizer>> _byType;

        static RandomizerStore()
        {
            Instance = new RandomizerStore();
        }

        public static RandomizerStore Instance { get; set; }

        public RandomizerStore()
        {
            _randomizers = GetInstancesOfType<IRandomizer>();
            _byType = _randomizers.ToLookup(x => x.Type).ToDictionary(x => x.Key, x => (IEnumerable<IRandomizer>) x);
        }

        public IEnumerable<IRandomizer> this[CardType type]
        {
            get { return _byType[type]; }
        }

        private static T[] GetInstancesOfType<T>()
        {
            return typeof (Game).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof (T).IsAssignableFrom(type))
                .Select(x => x.GetConstructor(new Type[0]))
                .Where(constructor => constructor != null)
                .Select(constructor => constructor.Invoke(new object[0]))
                .Cast<T>()
                .ToArray();
        }

    }
}
