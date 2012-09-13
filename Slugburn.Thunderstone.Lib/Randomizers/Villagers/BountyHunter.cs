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
                .SelectCards(
                    source => source.FromTopOfVillageDecks()
                                  .Min(0)
                                  .Filter(x => x.Cost <= card.Player.AvailableGold)
                                  .Caption("Buy Card")
                                  .Message("Buy 1 village card ({0} gold available).".Template(source.Player.AvailableGold)))
                .OnCardsSelected(x => x.Source.Discard(x.Selected))
                .Condition(player => player.Game.GetBuyableDecks(player.AvailableGold).Any())
                .On(Phase.Spoils);
        }
    }
}
