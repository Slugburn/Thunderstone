using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;

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

        public static object CreateMessage(this Card card)
        {
            return new
                       {
                           card.Name,
                           card.Cost,
                           card.Gold,
                           card.Health,
                           card.Id,
                           card.Level,
                           card.Light,
                           card.MagicAttack,
                           card.PhysicalAttack,
                           card.PotentialMagicAttack,
                           card.PotentialPhysicalAttack,
                           card.Rank,
                           card.Strength,
                           card.Tags,
                           card.Text,
                           card.Type,
                           card.Vp,
                           card.Xp
                       };
        }

        public static object CreateMessage(this IEnumerable<Card> cards)
        {
            return cards.Select(card => card.CreateMessage()).ToArray();
        }

        public static bool IsSameTypeAs(this Card card1, Card card2)
        {
            return card2.Name.Split(' ')[0] == card1.Name.Split(' ')[0];
        }

        public static bool HasMagicAttack(this Card card)
        {
            return (card.MagicAttack ?? 0) > 0;
        }

        public static bool HasPhysicalAttack(this Card card)
        {
            return (card.PhysicalAttack ?? 0) > 0;
        }
    }
}
