using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfShame : CurseRandomizer
    {
        public CurseOfShame() : base("Shame")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village/Dungeon:</b> Destroy this curse. Draw 2 fewer cards when you draw a new hand.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Destroy this curse. Draw 2 fewer cards when you draw a new hand.")
                .Action(player =>
                {
                    player.DestroyCard(card, card.Name);
                })
                .On(Phase.Village, Phase.Dungeon);
        }
    }
}