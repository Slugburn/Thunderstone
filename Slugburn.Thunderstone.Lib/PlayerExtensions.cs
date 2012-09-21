using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib
{
    public static class PlayerExtensions
    {
        public static ISelectSource SelectCard(this Player player, Ability triggerAbility = null)
        {
            return new SelectionContext(player, triggerAbility);
        }
    }
}
