using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib
{
    public class Card
    {
        public Card(Game game)
        {
            Game = game;
            Id = UniqueId.Next();
            _abilities = new List<Ability>();
            _tags = new List<string>();
            _mods = new List<IAttributeMod>();
        }

        public long Id { get; private set; }

        List<string> _tags;
        readonly List<Ability> _abilities;
        private Card _equipped;
        private int? _physicalAttack;
        private readonly List<IAttributeMod> _mods;
        private readonly List<Func<IEventAggregator, IDisposable>> _eventHandlers = new List<Func<IEventAggregator, IDisposable>>();
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
        private int? _magicAttack;
        private int? _strength;

        public Player Player { get; private set; }

        public void SetPlayer(Player player)
        {
            Player = player;
            Owner = CardOwner.Player;
        }

        public CardType Type { get; set; }

        public string Name { get; set; }

        public int? Gold { get; set; }

        public int? Strength
        {
            get { return ApplyModifiers(Attr.Strength); }
            set { _strength = value; }
        }

        public int? Light { get; set; }

        public int? Cost { get; set; }

        public string Text { get; set; }

        public int? PhysicalAttack
        {
            get { return Type == CardType.Weapon ? null : ApplyModifiers(Attr.PhysicalAttack); }
            set { _physicalAttack = value; }
        }

        public int? PotentialPhysicalAttack
        {
            get { return Type == CardType.Weapon && !IsEquipped ? _physicalAttack : null; }
        }

        public int? Xp { get; set; }

        public int? Vp { get; set; }

        public int? MagicAttack
        {
            get { return Type == CardType.Weapon ? null : ApplyModifiers(Attr.MagicalAttack); }
            set { _magicAttack = value; }
        }

        public int? PotentialMagicAttack
        {
            get { return Type == CardType.Weapon && !IsEquipped ? _magicAttack : null; }
        }

        public int? Health
        {
            get { return ApplyModifiers(Attr.Health); }
            set { _health = value; }
        }

        public string Tags
        {
            get { return _tags.ConcatTags(); }
        }

        public void SetTags(params string[] tags)
        {
            _tags = _tags.Union(tags).ToList();
        }

        public IEnumerable<Ability> GetAbilities()
        {
            return _abilities.Where(x=>x.IsUsableByOwner()).ToList();
        }

        public IEnumerable<Ability> GetAbilities(Phase phase)
        {
            return _abilities.Where(x=>x.Phase==phase).ToList();
        }

        public void AddAbility(Ability ability)
        {
            ability.Card = this;
            _abilities.Add(ability);
        }

        public void AddModifier(IAttributeMod mod)
        {
            var before = GetAttributeValue(mod.Attribute) ?? 0;
            _mods.Add(mod);
            var after = GetAttributeValue(mod.Attribute) ?? 0;
            if (before == after)
                return;
            var change = after < before ? "decreases" : "increases";
            Game.Log("{0} {1} {2} of {3} to {4}".Template(mod.Source.Name, change, mod.Attribute, Name, after));
        }

        private int? GetAttributeValue(Attr attr)
        {
            switch (attr)
            {
                case Attr.Strength:
                    return Strength;
                case Attr.PhysicalAttack:
                    return PhysicalAttack;
                case Attr.MagicalAttack:
                    return MagicAttack;
                case Attr.Health:
                    return Health;
                default:
                    throw new NotImplementedException();
            }
        }

        public Rank Rank { get; set; }

        public bool IsEquipped 
        {
            get { return _equipped != null; }
        }

        public int Level { get; set; }

        public CardOwner Owner { get; set; }

        public int? Darkness
        {
            get { return Rank != null ? Rank.Darkness : (int?) null; }
        }

        public Func<Player, bool> AttackCondition { get; set; }

        public Game Game { get; private set; }

        internal void SetEquipped(Card card)
        {
            // Remove any modifiers from the previous equipped card
            if (_equipped != null)
            {
                _mods.RemoveAll(x => x.Source == _equipped);
            }
            _equipped = card;
        }

        internal Card GetEquipped()
        {
            return _equipped;
        }

        public bool HasTag(string tag)
        {
            return _tags.Contains(tag);
        }

        public void Reset()
        {
            // Remove any rank
            Rank = null;
            // Unequip
            _equipped = null;
            // Remove modifiers
            _mods.Clear();
            // Unsubscribe
            _subscriptions.Each(x=>x.Dispose());
            _subscriptions.Clear();
        }

        public void Subscribe(IEventAggregator events)
        {
            _subscriptions.AddRange(_eventHandlers.Select(x => x(events)));
        }

        public void AddEventHandler(Func<IEventAggregator, IDisposable> func)
        {
            _eventHandlers.Add(func);
        }

        private object _data;
        private int? _health;

        public T GetData<T>()
        {
            return (T) _data;
        }

        public void SetData<T>(T data)
        {
            _data = data;
        }

        public int? GetBaseValue(Attr attr)
        {
            switch (attr)
            {
                case Attr.Health:
                    return _health;
                case Attr.MagicalAttack:
                    return _magicAttack;
                case Attr.PhysicalAttack:
                    return _physicalAttack;
                case Attr.Strength:
                    return _strength;
                default:
                    throw new NotImplementedException();
            }
        }

        public int? ApplyModifiers(Attr attr)
        {
            var applicable = _mods.Where(x => x.Attribute == attr).ToArray();
            var baseValue = GetBaseValue(attr);
            if (applicable.Length == 0)
                return baseValue;

            var modified = applicable.Aggregate(baseValue ?? 0, (i, mod) => mod.Modify(this, i));
            return (modified == 0 && baseValue == null) ? (int?) null : modified;
        }
    }
}
