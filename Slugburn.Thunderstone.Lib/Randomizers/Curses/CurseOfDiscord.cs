using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfDiscord : CurseRandomizer
    {
        public CurseOfDiscord() : base("Discord")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village/Dungeon:</b> Discard 2 cards to destroy this curse.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Discard 2 cards to destroy this curse.")
                .SelectCards(x => x.Select().FromHand()
                                           .Filter(c => c != card)
                                           .Caption("Curse of Neglect").Message("Discard 2 cards.").Min(2).Max(2))
                .OnCardsSelected(context => context.Player.DiscardCards(context.Selected))
                .Action(x => x.Player.DestroyCard(card, card.Name))
//                .Condition(player => player.Hand.Count >= 3)
                .On(Phase.Village, Phase.Dungeon);
        }
    }
}