using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Villagers
{
    class BountyHunter : VillagerRandomizer
    {
        public BountyHunter() : base("Bounty Hunter", "Mercenary")
        {
            Text = "<b>Dungeon:</b> Physical Atack +1.<br/><br/><b>Spoils:</b> Buy 1 village card.";
            Cost = 4;
            Gold = 2;
        }

        protected override void Modify(Card card)
        {
            card.PhysicalAttack = 1;
            card.CreateAbility()
                .Description("Buy 1 village card.")
                .BuyCard()
                .On(Phase.Spoils);
        }
    }
}
