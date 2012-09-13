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

    }
}
