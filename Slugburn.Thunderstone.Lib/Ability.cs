using System;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public class Ability
    {
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

        public AbilityModel CreateMessage()
        {
            return new AbilityModel { Id = Id, CardName = Card.Name, Description =  Description};
        }

        public Action<Player> Continuation { get; set; }

        public bool IsRequired { get; set; }

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

        public override string ToString()
        {
            return Description;
        }

        public static bool GetDefaultIsRequired(Phase phase)
        {
            switch (phase)
            {
                case Phase.Battle:
                case Phase.Aftermath:
                case Phase.Trophy:
                    return true;
                default:
                    return false;

            };
        }
    }
}
