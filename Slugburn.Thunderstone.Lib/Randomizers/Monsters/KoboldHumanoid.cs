using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public class KoboldHumanoid : MonsterRandomizer
    {
        public KoboldHumanoid() : base(1, "Kobold", "Humanoid")
        {
            Text =
                "Kobolds are easy to kill but worth little XP or VP. You can often put them in another player's discard pile after defeating them.";
        }

        protected override IEnumerable<MonsterDef> MonsterDefs
        {
            get
            {
                return new[]
                           {
                               DrakeclanAmbusher(),
                               DrakeclanCutter(),
                               DrakeclanShaman(),
                               DrakeclanLaird()
                           };
            }
        }

        private static MonsterDef DrakeclanAmbusher()
        {
            return new MonsterDef
                       {
                           Name = "Drakeclan Ambusher",
                           Count = 3,
                           Health = 4,
                           Gold = 0,
                           Xp = 1,
                           Vp = 1,
                           Text = "When you defeat this monster, you may place it into any player's discard pile."
                       };
        }

        private static MonsterDef DrakeclanCutter()
        {
            return new MonsterDef
                       {
                           Name = "Drakeclan Cutter",
                           Count = 3,
                           Health = 5,
                           Gold = 2,
                           Xp = 2,
                           Vp = -1,
                           Text = "When you defeat this monster, you may place it into any player's discard pile."
                       };
        }

        private static MonsterDef DrakeclanShaman()
        {
            return new MonsterDef
                       {
                           Name = "Drakeclan Shaman",
                           Count = 2,
                           Health = 6,
                           Gold = 3,
                           Xp = 1,
                           Vp = -2,
                           Text = "When you defeat this monster, you may place it into any player's discard pile."
                       };
        }

        private static MonsterDef DrakeclanLaird()
        {
            return new MonsterDef
                       {
                           Name = "Drakeclan Laird",
                           Count = 2,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 3,
                           Text =
                               "<b>Battle:</b> This card gains Health equal to the total Health of all other kobolds in the hall. "
                               + "If you defeat this card, you also defeat each other kobold in the hall. "
                               + "You do not get XP for those kobolds."
                       };
        }

    }

}
