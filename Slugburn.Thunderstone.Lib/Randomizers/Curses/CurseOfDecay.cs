using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfDecay : CurseRandomizer
    {
        public CurseOfDecay() : base("Decay")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village/Dungeon:</b> If this is the first ability you have used this turn, destroy this card. You cannot use any more abilities this turn.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("If this is the first ability you use this turn, destroy this card. You cannot use any more abilities this turn.")
                .Action(player =>
                {
                    player.DestroyCard(card, card.Name);
                    player.ActiveAbilities.RemoveAll(x => x.Card.Owner == CardOwner.Player);
                })
                .On(Phase.Village, Phase.Dungeon);

            // Add event handler that removes this card's abilities when player uses another card's ability (except equipping)
            card.AddEventHandler(events => events.Subscribe<CardAbilityUsed>(e =>
            {
                var usedCard = e.Ability.Card;
                if (e.Ability.Phase != Phase.Equip && usedCard.Owner == CardOwner.Player && usedCard != card)
                    usedCard.Player.ActiveAbilities.RemoveAll(x => x.Card == card);
            }));
        }
    }
}