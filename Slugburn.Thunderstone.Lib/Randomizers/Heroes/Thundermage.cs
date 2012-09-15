using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Thundermage : HeroRandomizer
    {
        public Thundermage() : base("Thundermage", "Human", "Wizard")
        {
            Text =
                "This wizard has Magic Attack and provides Light. At her top level, she can defeat any monster in rank 1.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Summoner(), Evoker(), Bolter() };
            }
        }

        private static HeroDef Summoner()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Summoner",
                           Strength = 3,
                           Light = 1,
                           Cost = 7,
                           Text = "<b>Magic Attack +2</b>",
                           MagicAttack = 2,
                       };
        }

        private static HeroDef Evoker()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Evoker",
                           Strength = 3,
                           Light = 2,
                           Cost = 10,
                           Text = "<b>Magic Attack +3</b>",
                           MagicAttack = 3,
                       };
        }

        private static HeroDef Bolter()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Bolter",
                           Strength = 4,
                           Light = 2,
                           Cost = 13,
                           Text = "<b>Magic Attack +4</b>"
                                  + "<br/><br/>"
                                  +
                                  "<b>Dungeon:</b> Place the monster from rank 1 into your discard pile <i>(you do not collect XP)</i>. End your turn.",
                           MagicAttack = 4,
                           Modify = card => card.CreateAbility()
                                                .DiscardMonster("Place the monster from rank 1 into your discard pile", x => x.Rank.Number == 1)
                                                .On(Phase.Dungeon)
                       };
        }
    }
}