using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Randomizers;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib
{
    public class Curse : IRandomizer
    {
        public CardType Type { get {return CardType.Curse;} }
        public string Name { get { return "Curse"; } }
        
        public CardInfo GetInfo()
        {
            return null;
        }

        public IEnumerable<Card> CreateCards(Game game)
        {
            return CreateCurses(game, Decay, Hostility, Neglect, Discord, Horror, Shame, Sloth, War);
        }

        private IEnumerable<Card> CreateCurses(Game game, params Action<Card>[] modifications)
        {
            // Create 4 of each curse type
            return Enumerable.Range(0, 4)
                .SelectMany(x =>
                            modifications.Select(modify =>
                                                   {
                                                       var card = new Card(game)
                                                                      {
                                                                          PhysicalAttack = -1,
                                                                          Text = "<b>Attack -1</b><br/><br/>",
                                                                          Owner = CardOwner.Game,
                                                                          Type = CardType.Curse
                                                                      };
                                                       card.SetTags("Curse", "Disease");
                                                       modify(card);
                                                       return card;
                                                   })
                );
        }

        private static void War(Card card)
        {
            card.Name = "Curse of War";
            card.Text = "You cannot rest or prepare."
                        + "<br/><br/>"
                        + "<b>Trophy:</b> Attack -1"
                        + "<br/><br/>" + "<b>Village/Dungeon:</b> Select 2 random cards from your hand <i>(excluding this card).</i>  " 
                        + "You may destroy 1 of them to destroy this card.";
            // TODO: Implement
        }

        private static void Sloth(Card card)
        {
            card.Name = "Curse of Sloth";
            card.Text +=
                "<small><b>Dungeon:</b> Destroy this curse. Lower your Total Attack Value by 3. " +
                "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.</small>";
            // TODO: Implement
        }

        private static void Shame(Card card)
        {
            card.Name = "Curse of Shame";
            card.Text +=
                "<b>Village/Dungeon:</b> Destroy this curse. Draw 2 fewer cards when you draw a new hand.";
            // TODO: Implement
        }

        private static void Horror(Card card)
        {
            card.Name = "Curse of Horror";
            card.Text +=
                "<small><b>Dungeon:</b> Destroy this curse. Reduce your Light to 0. " 
                + "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.</small>";
            // TODO: Implement
        }

        private static void Discord(Card card)
        {
            card.Name = "Curse of Discord";
            card.Text +=
                "<b>Village:</b> Lose 2 gold to destroy this curse.";
            // TODO: Implement
        }

        private static void Neglect(Card card)
        {
            card.Name = "Curse of Neglect";
            card.Text +=
                "<b>Village/Dungeon:</b> Discard 2 cards to destroy this curse.";
            card.CreateAbility()
                .Description("Discard 2 cards to destroy this curse.")
                .SelectCards(source=>source.FromHand().Caption("Curse of Neglect").Message("Discard 2 cards.").Min(2).Max(2))
                .OnCardsSelected(context => context.Player.DiscardCards(context.Selected))
                .Action(player => player.DestroyCard(card, card.Name))
                .Condition(player => player.Hand.Count >= 2)
                .On(Phase.Village, Phase.Dungeon);
        }

        private static void Decay(Card card)
        {
            card.Name = "Curse of Decay";
            card.Text +=
                "<b>Village/Dungeon:</b> If this is the first ability you have used this turn, destroy this card. You cannot use any more abilities this turn.";
            
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

        private void Hostility(Card card)
        {
            card.Name = "Curse of Hostility";
            card.Text +=
                "<b>Village/Dungeon:</b> Discard 2 XP to destroy this curse.";
            card.CreateAbility()
                .Description("Discard 2 XP to destroy this curse.")
                .Action(player =>
                            {
                                player.DestroyCard(card, card.Name);
                                player.Xp -= 2;
                            })
                .Condition(player => player.Xp >= 2)
                .On(Phase.Village, Phase.Dungeon);
        }
    }
}
