﻿using System;
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
                context.Game.Dungeon.Advance();
        }

        public static Card CreateCard<TRandomizer>(this TestContext context, string name = null) where TRandomizer : IRandomizer
        {
            var randomizer = Activator.CreateInstance<TRandomizer>();
            var cards = randomizer.CreateCards(context.Game);
            return name == null ? cards.First() : cards.First(x => x.Name.EndsWith(name));
        }

        public static Card[] CreateCards<TRandomizer>(this TestContext context, int count = int.MaxValue) where TRandomizer : IRandomizer
        {
            var randomizer = Activator.CreateInstance<TRandomizer>();
            return randomizer.CreateCards(context.Game).Take(count).ToArray();
        }


        public static Card CreateBasicCard<T>(this TestContext context) where T : ICardGen
        {
            var gen = Activator.CreateInstance<T>();
            return gen.Create(context.Game);
        }

        public static Card DrawHeroFromTopOfDeck(this TestContext context, Func<Card, bool> filter)
        {
            return DrawVillageCardFromTopOfDeck(context, CardType.Hero, filter);
        }

        public static Card DrawVillageCardFromTopOfDeck(this TestContext context, CardType cardType, Func<Card, bool> filter)
        {
            return GetVillageDeck(context, cardType, filter).Draw();
        }

        public static Deck GetVillageDeck(this TestContext context, CardType cardType, Func<Card, bool> filter)
        {
            return context.Game.Village[cardType].First(x => filter(x.TopCard));
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
            var ability = card.GetAbilities().SingleOrDefault(x => context.Player.State.AbilityTypes.Contains(x.Phase));
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
            context.SetPlayerState(Phase.Battle);
            if (monster.GetAbilities(Phase.Battle).Any())
                context.UseAbilityOf(monster);
        }

        public static void SetPlayerState(this TestContext context, params Phase[] abilityTypes)
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

        public static void WhenSelectingCardsSelectFirst(this TestContext context)
        {
            context.SetSelectCardsBehavior(message => message.Cards.Take(message.Min).Select(x => x.Id));
        }

        public static void WhenSelectingCardsSelect(this TestContext context, params Card[] cards)
        {
            context.SetSelectCardsBehavior(message => cards.Select(c => c.Id));
        }

        public static void WhenSelectingOptionSelect(this TestContext context, string option)
        {
            Func<SelectOptionMessage, string> behavior = message=> option;
            context.Set(behavior);
        }

        public static void HeroEquipsWeapon(this TestContext context, Card hero, Card weapon)
        {
            context.SetPlayerState(Phase.Equip);
            context.SetSelectCardsBehavior(message => new[] {hero.Id});
            context.UseAbilityOf(weapon);
        }

        public static bool IsAbilityUsable(this TestContext context, Ability ability)
        {
            return ability.IsUsableByOwner() && ability.Condition(context.Player);
        }

        public static Card GetMonsterInRank(this TestContext context, int number)
        {
            return context.GetRank(number).Card;
        }

        public static Rank GetRank(this TestContext context, int number)
        {
            return context.Game.Dungeon.Ranks.First(r => r.Number == number);
        }

        public static void SetDungeonHall(this TestContext context, params Card[] monsters)
        {
            context.SetTopOfDungeonDeck(monsters);
            context.AdvanceMonsterToFirstRank(monsters[0]);
        }
    }
}
