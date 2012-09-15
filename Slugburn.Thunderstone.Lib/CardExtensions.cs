using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public static class CardExtensions
    {
        private static readonly Random Random = new Random();

        public static IEnumerable<Card> Create(this ICardGen gen, int count)
        {
            return Enumerable.Range(0, count).Select(x => gen.Create());
        }

        public static List<Card> Shuffle(this IEnumerable<Card> cards)
        {
            var list = cards as IList<Card> ?? new List<Card>(cards);
            var shuffled = new List<Card>();
            while (list.Count > 0)
            {
                var randomIndex = Random.Next(list.Count);
                shuffled.Add(list[randomIndex]);
                list.RemoveAt(randomIndex);
            }
            return shuffled;
        }

        public static T PickRandom<T>(this IEnumerable<T> items)
        {
            var list = items as IList<T> ?? new List<T>(items);
            var randomIndex = Random.Next(list.Count);
            return list[randomIndex];
        }

        public static string ConcatTags(this IEnumerable<string> tags)
        {
            return String.Join(" • ", tags);
        }

        public static bool IsHero(this Card card)
        {
            return card.Type == CardType.Hero;
        }

        public static bool IsWeapon(this Card card)
        {
            return card.Type == CardType.Weapon;
        }

        public static bool IsSameTypeAs(this Card card1, Card card2)
        {
            return card2.Name.Split(' ')[0] == card1.Name.Split(' ')[0];
        }

        public static bool HasMagicAttack(this Card card)
        {
            return (card.MagicAttack ?? 0) + (card.PotentialMagicAttack ?? 0) > 0;
        }

        public static bool HasPhysicalAttack(this Card card)
        {
            return (card.PhysicalAttack ?? 0) + (card.PotentialPhysicalAttack) > 0;
        }
    }
}
