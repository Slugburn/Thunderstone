using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfWar : CurseRandomizer
    {
        public CurseOfWar() : base("War")
        {
        }

        protected override string GetAdditionalText()
        {
            return "{ this is overridden by Modify() }";
        }

        protected override void Modify(Card card)
        {
            card.Text = "You cannot rest or prepare."
                   + "<br/><br/>"
                   + "<b>Trophy:</b> Attack -1"
                   + "<br/><br/>" + "<b>Village/Dungeon:</b> Select 2 random cards from your hand <i>(excluding this card).</i>  "
                   + "You may destroy 1 of them to destroy this card.";

            card.AddEventHandler(
                events => events.Subscribe<CardDrawnToHand>(
                    ev =>
                        {
                            if (ev.Card == card)
                                card.Player.ValidActions = card.Player.ValidActions.Except(new[] {PlayerAction.Prepare, PlayerAction.Rest});
                        }));
            
            card.CreateAbility()
                .Description("Select 2 random cards from your hand. You may destroy 1 of them to destroy this card.")
                .SelectCards(x => x.Select().FromRandomHandSelection(2, c => c != card)
                                           .Caption("Curse of War").Message("Destroy 1 card."))
                .OnCardsSelected(x =>
                    {
                        x.Player.DestroyCard(x.Selected.First(), card.Name);
                        x.Player.DestroyCard(card, card.Name);
                    })
                .Condition(p => p.Hand.Count >= 3)
                .On(Phase.Village, Phase.Dungeon);
        }
    }
}
