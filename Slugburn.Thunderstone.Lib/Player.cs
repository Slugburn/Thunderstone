﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.BasicCards;
using Slugburn.Thunderstone.Lib.Debug;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib
{
    public class Player : IAttrSource
    {
        public IPlayerView View { get; private set; }
        private readonly IEventAggregator _events;
        private readonly List<IAttributeMod> _mods;
        private Rank _attackedMonsterRank;

        public Player(string id, IPlayerView view)
        {
            Id = id;
            View = view;
            Discard = new List<Card>();
            Hand = new List<Card>();
            ActiveAbilities = new List<Ability>();
            _mods = new List<IAttributeMod>();
            _events = new SimpleAggregator();
            ValidActions = GetAllActions();
        }

        private static IEnumerable<PlayerAction> GetAllActions()
        {
            return new[] {PlayerAction.Village, PlayerAction.Dungeon, PlayerAction.Prepare, PlayerAction.Rest};
        }

        public PlayerState State { get; set; }

        public string Name { get; set; }

        public int Vp { get; set; }

        public Deck Deck { get; set; }

        public List<Card> Hand { get; private set; }

        public List<Card> Discard { get; private set; }

        public int HandSize
        {
            get { return this.ApplyModifiers(Attr.HandSize) ?? 0; }
        }

        public void StartGame(Game game)
        {
            Game = game;
            Deck = CreateStartingDeck();
            Vp = Deck.GetCards().Sum(x => x.Vp ?? 0);
            DrawHand();
        }

        public void DrawHand()
        {
            Hand = new List<Card>();
            Draw(HandSize);
        }

        public IEnumerable<Card> Draw(int count)
        {
            List<Card> drawn;
            var overdraw = Math.Max(count - Deck.Count, 0);
            if (overdraw == 0)
            {
                drawn = Deck.Draw(count);
            }
            else
            {
                drawn = Deck.Draw(Deck.Count);
                Deck.Add(Discard);
                Discard = new List<Card>();
                Deck.Shuffle();
                drawn.AddRange(Deck.Draw(Math.Min(overdraw, Deck.Count)));
            }
            AddCardsToHand(drawn);
            return drawn;
        }

        public void AddCardsToHand(IEnumerable<Card> cards)
        {
            var cardList = cards.ToList();
            cardList.Each(card=>
                {
                    card.SetPlayer(this);
                    card.Subscribe(_events);
                });
            Hand.AddRange(cardList);
            ActiveAbilities.AddRange(cardList.SelectMany(x => x.GetAbilities()));

            Assert.That(Hand.Select(x => x.Id).Distinct().Count() == Hand.Count, "Duplicate card ID found in Hand");
            Assert.That(ActiveAbilities.Select(x => x.Id).Distinct().Count() == ActiveAbilities.Count, "Duplicate ability ID found in ActiveAbilities");

            Hand.Each(c => PublishEvent(new CardDrawnToHand(c)));
            SendUpdateHand();
        }

        public void AddCardToHand(Card card)
        {
            AddCardsToHand(new[] {card});
        }

        public int AvailableGold
        {
            get { return this.ApplyModifiers(Attr.Gold) ?? 0; }
        }

        public Game Game { get; set; }
        
        public IAttributeMod[] GetModifiersFor(Attr attr)
        {
            return _mods.Where(x=>x.Attribute==attr).ToArray();
        }

        public int? GetBaseValue(Attr attr)
        {
            switch (attr)
            {
                case Attr.Light:
                    return Hand.Sum(x => x.Light ?? 0);
                case Attr.PhysicalAttack:
                    return Hand.Sum(x => x.PhysicalAttack ?? 0);
                case Attr.MagicalAttack:
                    return Hand.Sum(x => x.MagicAttack ?? 0);
                case Attr.TotalAttack:
                    return PhysicalAttack + MagicAttack;
                case Attr.Gold:
                    return Hand.Sum(x => x.Gold ?? 0);
                case Attr.HandSize:
                    return 6;
                default:
                    throw new NotImplementedException();
            }
        }

        public GameSession Session { get; set; }

        public List<Ability> ActiveAbilities { get; private set; }

        public void StartTurn()
        {
            State = PlayerState.SelectingAction;
            Won = false;
            View.StartTurn(StartTurnMessage.From(ValidActions));
        }

        public IEnumerable<PlayerAction> ValidActions { get; set; }

        public void SelectMonster()
        {
            var validTargets = Game.Dungeon.Ranks
                .Select(x => x.Card)
                .Where(c => c != null)
                .Where(c => c.AttackCondition == null || c.AttackCondition(this))
                .ToList();

            if (!validTargets.Any())
            {
                EndTurn();
                return;
            }

            this.SelectCard()
                .FromHall()
                .Filter(card => validTargets.Any(x => x == card))
                .Caption("Select Monster").Message("Select a monster to fight")
                .Callback(x => OnSelectMonster(x.Selected.First()))
                .SendRequest(player => UseBattleAbilities());
        }

        public void UseBattleAbilities()
        {
            State = PlayerState.Battle;
            UseAbilities();
        }

        public void DetermineBattleResult()
        {
            var monster = AttackedCard;
            Won = GetBattleMarginVersus(monster) >= 0;
            var outcome = Won ? "defeated" : "triumphed";
            Log("{0} {1}.".Template(monster.Name, outcome));

            if (Won)
                Game.Publish(new MonsterDefeated(this, monster));

            UseAftermathAbilities();
        }

        public int GetBattleMarginVersus(Card monster)
        {
            return TotalAttack + Math.Min(0, TotalLight + (monster.Darkness ?? 0)) - (monster.Health ?? 0);
        }

        public int TotalAttack
        {
            get { return this.ApplyModifiers(Attr.TotalAttack) ?? 0; }
        }

        private int MagicAttack
        {
            get { return this.ApplyModifiers(Attr.MagicalAttack) ?? 0; }
        }

        private int PhysicalAttack
        {
            get { return this.ApplyModifiers(Attr.PhysicalAttack) ?? 0; }
        }

        public void UseAftermathAbilities()
        {
            State = PlayerState.Aftermath;
            UseAbilities();
        }

        public void UseSpoilsAbilities()
        {
            State = PlayerState.Spoils;
            UseAbilities();
        }

        public void EndBattle()
        {
            var monster = AttackedCard;
             _attackedMonsterRank = AttackedCard.Rank;
            if (Won)
            {
                _attackedMonsterRank.Card = null;
                Xp += monster.Xp ?? 0;
                AddToDiscard(monster);
                UseSpoilsAbilities();
            }
            else
            {
//                // Place undefeated monster on the bottom of the dungeon deck
//                Game.Dungeon.Deck.Add(monster);
//                RefillHall();
                EndTurn();
            }
        }

        public void Log(string message)
        {
            Game.Players.Each(p => p.View.Log(message));
        }

        public void RefillHall()
        {
            Game.Dungeon.RefillHall();
            ResolveRaidAndBreachEffects();
        }

        private void ResolveRaidAndBreachEffects()
        {
            // TODO: Resolve raid and breach effects
            EndTurn();
        }

        public int Xp { get; set; }

        public bool Won { get; set; }

        public void OnSelectMonster(Card monster)
        {
            AttackedCard = monster;
            ActiveAbilities.AddRange(AttackedCard.GetAbilities());

            Assert.That(ActiveAbilities.Select(x => x.Id).Distinct().Count() == ActiveAbilities.Count, "Duplicate ability found in ActiveAbilities");

            PublishEvent(new AttackRankSelected { Player = this, AttackedRank = AttackedCard.Rank });
        }

        public void PublishEvent<T>(T e)
        {
            _events.Publish(e);
        }

        public Card AttackedCard { get; set; }

        public void UseAbilities()
        {
            SendUpdateHand();
            var phaseAbilities = ActiveAbilities.Where(a => State.AbilityTypes.Contains(a.Phase) && a.Condition(this)).ToList();
            if (phaseAbilities.Count > 0)
            {
                var message = UseAbilityMessage.Create(State.Tag, phaseAbilities);
                View.UseAbility(message);
            }
            else
            {
                View.HideUseAbility();
                State.ContinueWith(this);
            }
        }

        public void BuyCard()
        {
            var availableDecks = Game.GetBuyableDecks(AvailableGold).ToList();
            if (availableDecks.Count > 0)
                View.BuyCard(BuyCardMessage.From(availableDecks));
            else
                LevelHeroes();
        }

        public void LevelHeroes()
        {
            Func<Card, bool> filter = x => x.IsHero() && x.Xp <= Xp && AvailableLevelUps(x).Length > 0;
            if (!Hand.Any(filter))
            {
                EndTurn();
                return;
            }

            this.SelectCard()
                .HeroToLevel(filter)
                .Min(0)
                .Callback( x => LevelHero(x.Selected.FirstOrDefault()))
                .SendRequest(x => { });
        }

        private void LevelHero(Card hero)
        {
            if (hero == null)
            {
                EndTurn();
                return;
            }
            this.SelectCard()
                .SelectHeroUpgrade(hero)
                .Callback(x =>
                              {
                                  var upgrade = x.Selected.First();
                                  x.Source.Discard(new[] {upgrade});
                                  x.Player.Xp -= hero.Xp ?? 0;
                                  DestroyCard(hero, "Upgrading to {0}".Template(upgrade.Name));
                              })
                .SendRequest(player => LevelHeroes());
        }

        public Card[] AvailableLevelUps(Card card)
        {
            var heroDecks = Game.Village[CardType.Hero];
            if (card.Level == 0)
            {
                return heroDecks.Select(x => x.TopCard).Where(c => c != null && c.Level == 1).ToArray();
            }
            else
            {
                var deck = heroDecks.SingleOrDefault(x => x.TopCard != null && card.IsSameTypeAs(x.TopCard));
                if (deck==null) return new Card[0];
                var upgrade = deck.GetCards().FirstOrDefault(x => x.Level == card.Level + 1);
                return upgrade==null ? new Card[0] : new[] {upgrade};
            }
        }

        public void EndTurn()
        {
            View.HideUseAbility();
            DiscardHand();
            ValidActions = GetAllActions();
            DrawHand();
            _mods.Clear();
            Game.EndTurn();
        }

        public void SendUpdateHand()
        {
            View.UpdateHand(UpdateHandMessage.From(Hand));
            View.UpdateStatus(StatusModel.From(this));
        }

        public void DiscardHand()
        {
            ActiveAbilities = new List<Ability>();
            Hand.Each(x=>x.Reset());
            Discard.AddRange(Hand);
            Hand = new List<Card>();
        }

        public void AddToDiscard(Card card)
        {
            AddToDiscard(new[] {card});
        }

        public void AddToDiscard(IEnumerable<Card> cards)
        {
            cards.Each(card =>
                           {
                               card.SetPlayer(this);
                               Vp += card.Vp ?? 0;
                               Discard.Add(card);
                           });
        }

        public Action<IEnumerable<long>> SelectCardsCallback { get; set; }

        public Action<string> SelectOptionCallback { get; set; }

        public object CardSelectionContext { get; set; }

        public int TotalLight
        {
            get { return this.ApplyModifiers(Attr.Light) ?? 0; }
        }

        public string Id { get; private set; }

        public void DestroyCard(Card card, string destructionSource)
        {
            DestroyCards(new[] {card}, destructionSource);
        }

        public void DestroyCards(IEnumerable<Card> cards, string source)
        {
            var cardList = cards.ToArray();
            Log("{0} destroys {1}.".Template(source, String.Join(", ", cardList.Select(c => c.Name))));
            Vp -= cardList.Sum(card => card.Vp ?? 0);
            RemoveFromHand(cardList);

            // Add destroyed curses back to the curse deck
            Game.Curses.Add(cardList.Where(c => c.Type == CardType.Curse));
        }

        public void DiscardCard(Card card)
        {
            DiscardCards(new[] {card});
        }

        public void DiscardCards(IEnumerable<Card> cards)
        {
            var cardList = cards.ToList();
            RemoveFromHand(cardList);
            Discard.AddRange(cardList);
        }

        public void RemoveFromHand(IEnumerable<Card> cards)
        {
            cards.Each(_RemoveFromHand);
            SendUpdateHand();
        }

        private void _RemoveFromHand(Card card)
        {
            // Unequip any related card
            if (card.IsEquipped)
            {
                card.GetEquipped().Each(equipped =>
                    {
                        equipped.RemoveEquipped(card);
                        if (equipped.IsHero())
                            Log("{0} no longer has {1} equipped.".Template(equipped.Name, card.Name));
                    });
            }
            card.Reset();
            Hand.Remove(card);
            ActiveAbilities.RemoveAll(x => x.Card == card);
        }

        public void RemoveFromHand(Card card)
        {
            _RemoveFromHand(card);
            SendUpdateHand();
        }

        public void Equip(Card hero, Card weapon, Action<Player, Card> onEquip)
        {
            hero.AddEquipped(weapon);
            weapon.AddEquipped(hero);
            onEquip(this, hero);
        }

        public void AddToTopOfDeck(Card card)
        {
            card.SetPlayer(this);
            Deck.AddToTop(card);
        }

        public void AddToTopOfDeck(IEnumerable<Card> cards)
        {
            cards.Each(AddToTopOfDeck);
        }

        public Deck CreateStartingDeck()
        {
            var deck = new Deck();
            deck.Add(new Regular().Create(Game, 6));
            deck.Add(new Longspear().Create(Game, 2));
            deck.Add(new Torch().Create(Game, 2));
            deck.Add(new ThunderstoneShard().Create(Game, 2));
            deck.Shuffle();
            deck.GetCards().Each(x => x.SetPlayer(this));
            return deck;
        }

        public void GainCurse(int count = 1)
        {
            Enumerable.Range(0, count).Each(x => Game.GiveCurseTo(this));
        }

        public void UseAbility(long abilityId)
        {
            var ability = ActiveAbilities.Single(x => x.Id == abilityId);
            if (!ability.IsRepeatable)
                ActiveAbilities.Remove(ability);

            var context = new AbilityUseContext(this, ability);
            ability.Action(context);
            PublishEvent(new CardAbilityUsed(ability));
            SendUpdateHand();
            
            // If ability overrides the default continuation, use that
            // Otherwise, just continue to try and use more abilities
            if (ability.Continuation != null)
                ability.Continuation(this);
            else
                UseAbilities();
        }

        public void Prepare()
        {
            this.SelectCard()
                .FromHand()
                .Min(0)
                .Max(Hand.Count)
                .Caption("Prepare")
                .Message("Select cards to place on top of your deck.")
                .Callback(x =>
                              {
                                  x.Player.RemoveFromHand(x.Selected);
                                  x.Player.AddToTopOfDeck(x.Selected);
                              })
                .SendRequest(x => x.Player.EndTurn());
        }

        public void Rest()
        {
            this.SelectCard()
                .FromHand()
                .Caption("Rest")
                .Destroy("Resting")
                .SendRequest(x => x.Player.EndTurn());
        }

        public void AddModifier(IAttributeMod mod)
        {
            _mods.Add(mod);
        }

        public void DiscardToTopOfDeck(Card card)
        {
            RemoveFromHand(card);
            AddToTopOfDeck(card);
        }
    }
}
