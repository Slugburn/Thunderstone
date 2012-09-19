using System;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public interface ISelectSource
    {
        Player Player { get; }
        ISelectionContext SelectionContext { get; }
    }

    public static class SelectSourceExtensions
    {
        public static IDefineSelection FromHand(this ISelectSource context)
        {
            var c = (SelectionContext) context;
            c.Source = new HandSource(c.Player);
            return c;
        }

        public static IDefineSelection FromRandomHandSelection(this ISelectSource context, int count, Func<Card, bool> selector)
        {
            var c = (SelectionContext)context;
            c.Source = new RandomHandSource(c.Player, count, selector);
            return c;
        }

        public static IDefineSelection FromHall(this ISelectSource context)
        {
            var c = (SelectionContext) context;
            c.Source = new HallSource(c.Player);
            return c;
        }

        public static IDefineSelection FromTopOfHeroDecks(this ISelectSource context)
        {
            var c = (SelectionContext) context;
            c.Source = new TopOfDeckSource(c.Player, CardType.Hero);
            return c;
        }

        public static IDefineSelection FromTopOfVillageDecks(this ISelectSource context)
        {
            var c = (SelectionContext)context;
            c.Source = new TopOfDeckSource(c.Player);
            return c;
        }

        public static IDefineSelection FromHeroDecks(this ISelectSource context)
        {
            var c = (SelectionContext)context;
            c.Source = new HeroDeckSource(c.Player);
            return c;
        }

        public static IDefineSelection HeroToLevel(this ISelectSource source, Func<Card, bool> filter)
        {
            return source
                .FromHand()
                .Filter(filter)
                .Caption("Level Hero")
                .Message("Select a hero card to level up");
        }

        public static IDefineSelection SelectHeroUpgrade(this ISelectSource selectSource, Card hero)
        {
            return selectSource
                .FromHeroDecks()
                .Filter(x=>
                            {
                                if (hero.Level == 0) return x.Level == 1;
                                return x.IsSameTypeAs(hero) && x.Level == hero.Level + 1;
                            })
                .Caption("Level Hero")
                .Message("Select upgraded hero");
        }
    }
}
