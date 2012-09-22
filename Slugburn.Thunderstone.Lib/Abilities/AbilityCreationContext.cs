using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Abilities
{
    public class AbilityCreationContext : ICreateAbilitySyntax, IAbilityCardsSelectedContext, IAbilityCardsSelectedSyntax, IDescriptionDefinedSyntax
    {
        public string Description { get; set; }
        private Action<AbilityUseContext> Action { get; set; }
        private List<CardSelection> CardSelections { get; set; }
        public Func<Player, bool> Condition { get; set; }
        public Action<Player> Continuation { get; set; }
        private SelectOptionArg SelectOption { get; set; }

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

        public void SetSelectOption(SelectOptionArg arg)
        {
            SelectOption = arg;
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
            var ability = CardSelections.Count == 0 
                ? new Ability(phase, Description, Action) {Continuation = Continuation} 
                : CreateSelectCardAbility(phase);
            
            if (SelectOption != null)
            {
                var afterOption = ability.Action;
                ability.Action = x =>
                                     {
                                         x.Player.SelectOptionCallback = s =>
                                                                             {
                                                                                 x.Option = s;
                                                                                 afterOption(x);
                                                                             };
                                         x.Player.View.SelectOption(SelectOptionMessage.From(SelectOption));
                                     };
            }

            ability.Phase = phase;
            ability.IsRequired = IsRequired ?? Ability.GetDefaultIsRequired(phase);
            ability.IsRepeatable = IsRepeatable;
            if (Condition != null)
                ability.Condition = Condition;
            Card.AddAbility(ability);
        }

        private Ability CreateSelectCardAbility(Phase phase)
        {
            // Reverse the order of card selections so that a chain of actions can be created last to first
            CardSelections.Reverse();
            var firstSelectionAction = CardSelections.Aggregate<CardSelection, Action<AbilityUseContext>>(null, (current, selection) => CreateSelectionAction(selection, current));
            var ability = new Ability(phase, Description, player => { })
                          {
                              Action = firstSelectionAction,
                              Continuation = x => { },
                          };

            // The default condition is that there are enough cards in the source to meet the minimum
            // selection criteria
            Func<Player, bool> defaultCondition = player =>
                                                      {
                                                          var selectionContext =
                                                              ((SelectionContext) CardSelections.Last().Select(new AbilityUseContext(player, ability)));
                                                          var selectable = selectionContext.GetSourceCards();
                                                          return selectable.Count() >= selectionContext.Min;
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
            return ability;
        }

        private Action<AbilityUseContext> CreateSelectionAction(CardSelection selection, Action<AbilityUseContext> nextAction)
        {
            Action<AbilityUseContext> action =
                context =>
                    {
                        var selector = selection.Select(context);
                        var requestor = selection.Callbacks
                            .Select(callback => selector.Callback(selectionContext => context.ProcessSelection(selectionContext, callback))).ToList()
                            .Last();
                        var continuation = nextAction ?? (x =>
                                                              {
                                                                  if (Continuation != null)
                                                                      Continuation(context.Player);
                                                                  else
                                                                      context.Player.UseAbilities();
                                                              });
                        requestor.SendRequest(selectionContext => continuation(context));
                    };
            return action;
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