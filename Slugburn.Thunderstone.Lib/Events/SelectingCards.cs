using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Events
{
    public class SelectingCards
    {
        public SelectingCards(Ability triggeringAbility, List<Card> selection)
        {
            TriggeringAbility = triggeringAbility;
            Selection = selection;
        }

        public Ability TriggeringAbility { get; set; }
        public List<Card> Selection { get; set; }
    }
}