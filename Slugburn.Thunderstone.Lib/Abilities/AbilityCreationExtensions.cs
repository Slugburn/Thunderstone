using System;
using System.Linq;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Abilities
{
    public static class AbilityCreationExtensions
    {
        public static ICreateAbilitySyntax CreateAbility(this Card card)
        {
            return new AbilityCreationContext(card);
        }

        public static IAbilityDefinedSyntax Required(this IAbilityDefinedSyntax syntax, bool isRequired = true)
        {
            ((AbilityCreationContext)syntax).IsRequired = isRequired;
            return syntax;
        }

        public static IAbilityDefinedSyntax Repeatable(this IAbilityDefinedSyntax syntax)
        {
            ((AbilityCreationContext)syntax).IsRepeatable = true;
            return syntax;
        }

        public static IAbilityCardsSelectedSyntax Destroy(this IAbilitySelectCardsSyntax syntax, string destructionSource)
        {
            return OnCardsSelected(syntax, x => x.Source.Destroy(x.Selected, destructionSource));
        }

        public static IAbilityDescriptionCompleteSyntax Description(this ICreateAbilitySyntax context, string description)
        {
            var c = (AbilityCreationContext)context;
            c.Description = description;
            return c;
        }

        public static IAbilityDefinedSyntax Action(this IAbilityDescriptionCompleteSyntax context, Action<AbilityUseContext> action)
        {
            var c = (AbilityCreationContext)context;
            c.SetAction(action);
            return c;
        }

        public static IAbilitySelectCardsSyntax SelectCards(this IAbilityDescriptionCompleteSyntax context, Func<AbilityUseContext, IDefineSelection> cardSelection)
        {
            var c = (AbilityCreationContext)context;
            c.AddSelection(cardSelection);
            return c;
        }

        public static IAbilityCardsSelectedSyntax OnCardsSelected(this IAbilitySelectCardsSyntax syntax, Action<AbilityUseContext> callback)
        {
            var c = (AbilityCreationContext)syntax;
            c.AddCallback(callback);
            return c;
        }

        public static IAbilityDefinedSyntax Condition(this IAbilityDefinedSyntax syntax, Func<Player, bool> condition)
        {
            ((AbilityCreationContext) syntax).Condition = condition;
            return syntax;
        }

        public static IAbilityDefinedSyntax ContinueWith(this IAbilityDefinedSyntax syntax, Action<Player> continuation)
        {
            ((AbilityCreationContext) syntax).Continuation = continuation;
            return syntax;
        }

        public static IAbilityDefinedSyntax DrawCards(this ICreateAbilitySyntax syntax, int count)
        {
            return syntax
                .Description("Draw {0} {1}.".Template(count, GetCountText(count)))
                .DrawCards(count);
        }

        public static IAbilityDefinedSyntax DrawCards(this IAbilityDescriptionCompleteSyntax syntax, int count)
        {
            return syntax.Action(x => x.Player.Draw(count));
        }

        public static IAbilityDefinedSyntax EquipWeapon(this ICreateAbilitySyntax syntax, Action<Player, Card> onEquip)
        {
            Func<Card, bool> heroCanEquip = hero => hero.IsHero() && hero.CanEquip() && hero.Strength >= syntax.Card.Strength;
            return syntax.Description("Equip {0}.".Template(syntax.Card.Name))
                .SelectCards(context => context.Select().FromHand().Filter(heroCanEquip))
                .OnCardsSelected(x => x.Player.Equip(x.Selected.First(), syntax.Card, onEquip))
                .Condition(player => !syntax.Card.IsEquipped && player.Hand.Any(heroCanEquip));
        }

        public static IAbilityDefinedSyntax EquipWeapon(this ICreateAbilitySyntax syntax, Attr attribute, int amount)
        {
            return syntax.EquipWeapon((player, hero) => hero.AddModifier(new PlusMod(syntax.Card, attribute, amount)));
        }

        public static IAbilityDefinedSyntax DestroyCard(this ICreateAbilitySyntax syntax, string description = "Destroy 1 card in your hand.", Func<Card, bool> filter = null)
        {
            filter = filter ?? (card => true);
            return syntax
                .Description(description)
                .SelectCards(x => x.Select().FromHand().Filter(filter).Caption("Destroy Card").Message(description))
                .Destroy(syntax.Card.Name)
                .Condition( player => player.Hand.Any(filter));
        }

        public static IAbilityDefinedSyntax DestroyDiseaseToDrawCard(this ICreateAbilitySyntax syntax, int drawCount = 1)
        {
            Func<Card, bool> isDisease = x => x.Type == CardType.Curse;
            return syntax
                .Description("Destroy 1 disease to draw {0} {1}.".Template(drawCount, GetCountText(drawCount)))
                .SelectCards(x => x.Select().FromHand().Filter(isDisease).Caption("Destroy Disease").Message("Select 1 card."))
                .Destroy(syntax.Card.Name)
                .DrawCards(1)
                .Condition(player => player.Hand.Any(isDisease));
        }

        public static IAbilityDefinedSyntax DiscardMonster(this ICreateAbilitySyntax context, string description, Func<Card, bool> selector)
        {
            return context
                .Description(description)
                .SelectCards(x => x.Select().FromHall().Filter(selector).Caption("Discard Monster").Message("Pick a monster to discard."))
                .OnCardsSelected(x => x.Source.Discard(x.Selected))
                .Condition(player => player.Game.Dungeon.Ranks.Select(r => r.Card).Any(selector));
        }

        public static IAbilityDefinedSyntax DiscardCard(this ICreateAbilitySyntax context, string description, Func<Card, bool> filter)
        {
            return context
                .Description(description)
                .SelectCards(x => x.Select().FromHand().Filter(filter).Caption("Discard a card").Message(description))
                .OnCardsSelected(x => x.Source.Discard(x.Selected))
                .Condition(player => player.Hand.Any(filter));
        }


        private static string GetCountText(int count)
        {
            return count == 1 ? "card" : "cards";
        }

        public static IAbilityCardsSelectedSyntax BuyCard(this IAbilityDescriptionCompleteSyntax syntax, Func<Card,bool> filter = null)
        {
            return syntax
                .SelectCards(
                    context => context.Select().FromTopOfVillageDecks()
                                  .Min(0)
                                  .Filter(x =>
                                              {
                                                  Func<Card, bool> cardFilter = filter ?? (c => true);
                                                  Func<Card, bool> costsLessThanAvailableGold = c => c.Cost <= context.Player.AvailableGold;
                                                  return cardFilter(x) && costsLessThanAvailableGold(x);
                                              })
                                  .Caption("Buy Card")
                                  .Message("Buy 1 card ({0} gold available).".Template(context.Player.AvailableGold)))
                .OnCardsSelected(x => x.Source.Discard(x.Selected));
        }
    }
}
