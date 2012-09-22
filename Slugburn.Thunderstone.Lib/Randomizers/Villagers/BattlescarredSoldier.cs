using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Villagers
{
    class BattlescarredSoldier : VillagerRandomizer
    {
        public BattlescarredSoldier() : base("Battle-scarred Soldier", "Mercenary")
        {
            Text =
                "<b>Village:</b> Draw 1 card.<br/><br/><b>Dungeon:</b> Draw 1 card. If it is a hero it gains Physical Attack +2.";
            Cost = 3;
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.CreateAbility()
                .DrawCards(1)
                .On(Phase.Village)
                .CreateAbility()
                .Description("Draw 1 card. If it is a hero it gains Physical Attack +2.")
                .Action(x =>
                            {
                                var drawn = x.Player.Draw(1).FirstOrDefault();
                                if (drawn != null && drawn.Type == CardType.Hero)
                                    drawn.AddModifier(new PlusMod(card, Attr.PhysicalAttack, 2));
                            })
                .On(Phase.Dungeon);
        }
    }
}
