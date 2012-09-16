using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Abilities
{
    public class AbilityCreationContext : ICreateAbilitySyntax, IAbilityCardsSelectedContext, IAbilityCardsSelectedSyntax
    {
        public string Description { get; set; }
        private Action<Player> Action { get; set; }
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

        public void SetAction(Action<Player> action)
        {
            if (CardSelections.Count == 0)
                Action = action;
            else
                CardSelections.Last().Callbacks.Add(context => action(context.Player));
        }

        public void AddSelection(Func<ISelectSource, IDefineSelection> cardSelection)
        {
            CardSelections.Add(new CardSelection {Select = cardSelection});
        }

        public void AddCallback(Action<ISelectionContext> callback)
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
                Action<SelectionContext> nextAction = null;
                CardSelections.Reverse();
                foreach (var selection in CardSelections)
                {
                    var localSelection = selection;
                    var localNextAction = nextAction;
                    Action<SelectionContext> action = context =>
                    {
                        var selector = localSelection.Select(context);
                        var requestor = localSelection.Callbacks.Select(selector.Callback).ToList().Last();
                        var continuation = localNextAction ?? (selectionContext =>
                                                                   {
                                                                       if (Continuation != null) 
                                                                           Continuation(context.Player);
                                                                       else 
                                                                           context.Player.UseAbilities();
                                                                   });
                        requestor.SendRequest(continuation);
                    };
                    nextAction = action;
                }
                Action<Player> firstAction = player => nextAction((SelectionContext) player.SelectCard(Card));
                ability = new Ability(phase, Description, firstAction) {Continuation = x => { }};
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
                Callbacks = new List<Action<ISelectionContext>>();
            }

            public Func<ISelectSource, IDefineSelection> Select { get; set; }
            public List<Action<ISelectionContext>> Callbacks { get; private set; }
        }

    }


}