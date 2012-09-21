using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Randomizers;

namespace Slugburn.Thunderstone.Lib.Test
{
    public static class TestContextExtensions
    {
        public static Card SetMonsterInFirstRank<TRandomizer>(this TestContext context, string name) where TRandomizer : IRandomizer
        {
            var monster = CreateCard<TRandomizer>(context, name);
            context.SetTopOfDungeonDeck(monster);
            context.AdvanceMonsterToFirstRank(monster);
            return monster;
        }

        public static void SetTopOfDungeonDeck(this TestContext context, params Card[] monsters)
        {
            monsters.Reverse().Each(monster => context.Game.Dungeon.AddToTopOfDeck(monster));
        }

        public static void AdvanceMonsterToFirstRank(this TestContext context, Card monster)
        {
            while (context.Game.Dungeon.Ranks[0].Card != monster)
                context.Game.AdvanceDungeon();
        }

        public static Card CreateCard<TRandomizer>(this TestContext context, string name = null) where TRandomizer : IRandomizer
        {
            var randomizer = Activator.CreateInstance<TRandomizer>();
            var cards = randomizer.CreateCards(context.Game);
            return name == null ? cards.First() : cards.First(x => x.Name == name);
        }

        public static Card CreateBasicCard<T>(this TestContext context) where T : ICardGen
        {
            var gen = Activator.CreateInstance<T>();
            return gen.Create(context.Game);
        }

        public static Card GetHeroFromTopOfDeck(this TestContext context, Func<Card, bool> filter)
        {
            return context.Game.Village[CardType.Hero].First(x => filter(x.TopCard)).Draw();
        }

        public static Card GetHeroFromVillage(this TestContext context, Func<Card, bool> filter)
        {
            var deck = context.Game.Village[CardType.Hero].First(x => x.GetCards().Any(filter));
            var card = deck.GetCards().First(filter);
            deck.Remove(card);
            return card;
        }

        public static void SetPlayerHand(this TestContext context, params Card[] cards)
        {
            context.Player.DiscardHand();
            context.Player.AddCardsToHand(cards);
        }

        public static void SetPlayerState(this TestContext context, PlayerState state)
        {
            context.Player.State = state;
        }

        public static Ability GetAbility(this Card card)
        {
            return card.GetAbilities().First();
        }

        public static Ability UseAbilityOf(this TestContext context, Card card)
        {
            var ability = context.GetAbilityOf(card);
            context.UseAbility(ability);
            return ability;
        }

        public static Ability GetAbilityOf(this TestContext context, Card card)
        {
            Assert.That(context.Player.State, Is.Not.Null, "Player state has not been set.");
            var ability = card.GetAbilities().FirstOrDefault(x => context.Player.State.AbilityTypes.Contains(x.Phase));
            Assert.That(ability, Is.Not.Null, "No matching ability found");
            return ability;
        }

        public static void UseAbility(this TestContext context, Ability ability)
        {
            Assert.That(context.IsAbilityUsable(ability), Is.True, "Ability is not currently usable: '{0}'".Template(ability.Description));
            context.Player.UseAbility(ability.Id);
        }

        public static void WhenBattling(this TestContext context, Card monster)
        {
            context.Player.OnSelectMonster(monster);
            context.SetTestPlayerState(Phase.Battle);
            if (monster.GetAbilities(Phase.Battle).Any())
                context.UseAbilityOf(monster);
        }

        public static void SetTestPlayerState(this TestContext context, params Phase[] abilityTypes)
        {
            context.Player.State = new PlayerState(null, p => { }, abilityTypes);
        }

        public static Ability AddAbilityStub(this TestContext context, Phase phase)
        {
            var ability = new Ability(phase, "Ability stub", p => { });
            context.Player.ActiveAbilities.Add(ability);
            return ability;
        }

        public static void SetSelectCardsBehavior(this TestContext context, Func<SelectCardsMessage, IEnumerable<long>> behavior)
        {
            context.Set(behavior);
        }

        public static void GivenSelectingFirstMatchingCards(this TestContext context)
        {
            context.SetSelectCardsBehavior(message => message.Cards.Take(message.Min).Select(x => x.Id));
        }

        public static void HeroEquipsWeapon(this TestContext context, Card hero, Card weapon)
        {
            context.SetTestPlayerState(Phase.Equip);
            context.SetSelectCardsBehavior(message => new[] {hero.Id});
            context.UseAbilityOf(weapon);
        }

        public static bool IsAbilityUsable(this TestContext context, Ability ability)
        {
            return ability.IsUsableByOwner() && ability.Condition(context.Player);
        }
    }
}
