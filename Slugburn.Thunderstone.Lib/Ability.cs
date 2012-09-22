using System;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public class Ability
    {
        public long Id { get; set; }
        public Card Card { get; set; }
        public Phase Phase { get; set; }
        public string Description { get; set; }
        public Action<AbilityUseContext> Action { get; set; }
        public Func<Player, bool> Condition { get; set; }

        public Ability(Phase type, string description, Action<AbilityUseContext> action) :this(description,action)
        {
            Phase = type;
        }

        public Ability(string description, Action<AbilityUseContext> action)
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

        public bool IsRepeatable { get; set; }

        public bool IsUsableByOwner()
        {
            switch (Card.Owner)
            {
                case CardOwner.Player:
                    return Phase == Phase.Village 
                        || Phase == Phase.Dungeon 
                        || Phase == Phase.Equip 
                        || Phase == Phase.Trophy 
                        || Phase == Phase.Spoils && Card.Type != CardType.Monster
                        || Phase == Phase.Aftermath && Card.Type != CardType.Monster;
                case CardOwner.Dungeon:
                    return Phase == Phase.Battle || Phase == Phase.Aftermath || Phase == Phase.Trophy || Phase == Phase.Spoils && Card.Type == CardType.Monster;
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
