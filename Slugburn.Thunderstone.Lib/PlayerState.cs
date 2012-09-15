using System;

namespace Slugburn.Thunderstone.Lib
{
    public class PlayerState
    {
        public string Tag { get; set; }
        public Action<Player> ContinueWith { get; set; }
        public Phase[] AbilityTypes { get; set; }

        static PlayerState()
        {
            SelectingAction = new PlayerState();
            Village = new PlayerState("Village", p => p.BuyCard(), Phase.Village, Phase.Trophy);
            Dungeon = new PlayerState("Dungeon", p => p.SelectMonster(), Phase.Dungeon, Phase.Equip, Phase.Trophy);
            Prepare = new PlayerState("Prepare", p => p.Prepare(), Phase.Trophy);
            Rest = new PlayerState("Rest", p => p.Rest(), Phase.Trophy);
            Battle = new PlayerState("Battle", p => p.DetermineBattleResult(), Phase.Battle);
            Aftermath = new PlayerState("Aftermath", p => p.EndBattle(), Phase.Aftermath);
            Spoils = new PlayerState("Spoils", p=>p.RefillHall(), Phase.Spoils);
        }

        protected PlayerState()
        {
            ContinueWith = player => { };
            AbilityTypes = new Phase[0];
        }

        public PlayerState(string tag, Action<Player> continueWith, params Phase[] abilityTypes)
        {
            Tag = tag;
            ContinueWith = continueWith;
            AbilityTypes = abilityTypes;
        }

        public static PlayerState SelectingAction { get; private set; }
        public static PlayerState Village { get; private set; }
        public static PlayerState Dungeon { get; private set; }
        public static PlayerState Prepare { get; private set; }
        public static PlayerState Rest { get; private set; }
        public static PlayerState Battle { get; private set; }
        public static PlayerState Aftermath { get; private set; }
        public static PlayerState Spoils { get; private set; }
    }
}