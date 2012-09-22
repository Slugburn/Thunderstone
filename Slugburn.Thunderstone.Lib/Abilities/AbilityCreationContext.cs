using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Selectors;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Abilities
{
    public class AbilityCreationContext : ICreateAbilitySyntax, IAbilityCardsSelectedContext, IAbilityCardsSelectedSyntax
    {
        public string Description { get; set; }
        private Action<AbilityUseContext> Action { get; set; }
        private List<CardSelection> CardSelections { get; set; }
        public Func<Player, bool> Condition { get; set; }
        public Action<Player> Continuation { get; set; }

        public AbilityCreationContext(Card card)
        {
            Card = card;
            CardSelections = new List<CardSelection>();
        }

        public Card On(params Phase[] phases)
        {
            phases.Each(OnPhase);
            return Card;
        }

        public void SetAction(Action<AbilityUseContext> action)
        {
            if (CardSelections.Count == 0)
                Action = action;
            else
                CardSelections.Last().Callbacks.Add(action);
        }

        public void AddSelection(Func<AbilityUseContext, IDefineSelection> cardSelection)
        {
            CardSelections.Add(new CardSelection {Select = cardSelection});
        }

        public void AddCallback(Action<AbilityUseContext> callback)
        {
            CardSelections.Last().Callbacks.Add(callback);
        }

        private void OnPhase(Phase phase)
        {
            Ability ability;
            if (CardSelections.Count == 0)
            {
                ability = new Ability(phase, Description, Action) {Continuation = Continuation};
            }
            else
            {
                Action<AbilityUseContext> nextAction = null;
                CardSelections.Reverse();
                foreach (var selection in CardSelections)
                {
                    var localSelection = selection;
                    var localNextAction = nextAction;
                    Action<AbilityUseContext> action = context =>
                    {
                        var selector = localSelection.Select(context);
                        var requestor = localSelection.Callbacks
                            .Select(callback => selector.Callback(selectionContext => context.ProcessSelection(selectionContext, callback))).ToList().Last();
                        var continuation = localNextAction ?? (x =>
                                                                   {
                                                                       if (Continuation != null) 
                                                                           Continuation(context.Player);
                                                                       else 
                                                                           context.Player.UseAbilities();
                                                                   });
                        requestor.SendRequest(selectionContext => continuation(context));
                    };
                    nextAction = action;
                }
                ability = new Ability(phase, Description, player => { })
                              {
                                  Action = useContext => { nextAction(useContext); },
                                  Continuation = x => { }, 
                              };

                // The default condition is that there are enough cards in the source to meet the minimum
                // selection criteria
                Func<Player, bool> defaultCondition = player =>
                    {
                        var selectionContext = ((SelectionContext) CardSelections.Last().Select(new AbilityUseContext(player, ability)));
                        var selectable = selectionContext.GetSourceCards();
                        return selectable.Count() >= selectionContext.Min ;
                    };
                if (Condition == null)
                {
                    Condition = defaultCondition;
                }
                else
                {
                    var baseCondition = Condition;
                    Condition = player => baseCondition(player) && defaultCondition(player);
                }
            }
            ability.Phase = phase;
            ability.IsRequired = IsRequired ?? Ability.GetDefaultIsRequired(phase);
            ability.IsRepeatable = IsRepeatable;
            if (Condition != null)
                ability.Condition = Condition;
            Card.AddAbility(ability);
        }

        public Card Card { get; private set; }

        public bool? IsRequired { get; set; }

        public bool IsRepeatable { get; set; }

        public class CardSelection
        {
            public CardSelection()
            {
                Callbacks = new List<Action<AbilityUseContext>>();
            }

            public Func<AbilityUseContext, IDefineSelection> Select { get; set; }
            public List<Action<AbilityUseContext>> Callbacks { get; private set; }
        }

    }


}