using System;

namespace Slugburn.Thunderstone.Lib
{
    public class Ability
    {
        private Action<Player> _continuation;

        public long Id { get; set; }
        public Card Card { get; set; }
        public Phase Phase { get; set; }
        public string Description { get; set; }
        public Action<Player> Action { get; set; }
        public Func<Player, bool> Condition { get; set; }

        public Ability(Phase type, string description, Action<Player> action) :this(description,action)
        {
            Phase = type;
        }

        public Ability(string description, Action<Player> action)
        {
            Id = UniqueId.Next();
            Description = description;
            Action = action;
            Condition = player => true;
        }

        public object CreateMessage()
        {
            return new {Id, CardName = Card.Name, Description};
        }

        public Action<Player> Continuation
        {
            get { return _continuation ?? GetDefaultContinuation(Phase); }
            set { _continuation = value; }
        }

        public static Action<Player> GetDefaultContinuation(Phase phase)
        {
            switch (phase)
            {
                case Phase.Village:
                    return (player => player.UseVillageAbilities());
                case Phase.Dungeon:
                case Phase.Equip:
                    return (player => player.UseDungeonAbilities());
                case Phase.Battle:
                    return (player => player.UseBattleAbilities());
                case Phase.Aftermath:
                    return (player => player.UseAftermathAbilities());
                case Phase.Spoils:
                    return (player => player.UseSpoilsAbilities());
                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsUsableByOwner()
        {
            switch (Card.Owner)
            {
                case CardOwner.Player:
                    return Phase == Phase.Village || Phase == Phase.Dungeon || Phase == Phase.Equip || Phase == Phase.Spoils && Card.Type != CardType.Monster;
                case CardOwner.Dungeon:
                    return Phase == Phase.Battle || Phase == Phase.Aftermath || Phase == Phase.Spoils && Card.Type == CardType.Monster;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
