using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Modifiers;
using Attribute = Slugburn.Thunderstone.Lib.Modifiers.Attribute;

namespace Slugburn.Thunderstone.Lib
{
    public class Card
    {
        public Card()
        {
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
        private readonly List<IDisposable>  _subscriptions = new List<IDisposable>();
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
            get { return _mods.ApplyTo(Attribute.Strength, _strength); }
            set { _strength = value; }
        }

        public int? Light { get; set; }

        public int? Cost { get; set; }

        public string Text { get; set; }

        public int? PhysicalAttack
        {
            get { return Type == CardType.Weapon ? null : _mods.ApplyTo(Attribute.PhysicalAttack, _physicalAttack); }
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
            get { return Type == CardType.Weapon ? null : _mods.ApplyTo(Attribute.MagicalAttack, _magicAttack); }
            set { _magicAttack = value; }
        }

        public int? PotentialMagicAttack
        {
            get { return Type == CardType.Weapon && !IsEquipped ? _magicAttack : null; }
        }

        public int? Health { get; set; }

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
            Player.Log("{0} {1} {2} of {3} to {4}".Template(mod.Source.Name, change, mod.Attribute, Name, after));
        }

        private int? GetAttributeValue(Attribute attr)
        {
            switch (attr)
            {
                case Attribute.Strength:
                    return Strength;
                case Attribute.PhysicalAttack:
                    return PhysicalAttack;
                case Attribute.MagicalAttack:
                    return MagicAttack;
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

        public int Darkness
        {
            get { return Rank.Darkness; }
        }

        public Func<Player,bool> AttackCondition { get; set; }

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

    }
}
