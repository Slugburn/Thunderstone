using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Criochan : HeroRandomizer
    {
        public Criochan() : base("Criochan", "Human", "Fighter")
        {
            Text = "This fighter has Physical Attack. At higher levels his attack increases significantly.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Sergeant(), Knight(), Captain()};
            }
        }

        private static HeroDef Sergeant()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Sergeant",
                           Strength = 7,
                           Cost = 7,
                           Text = "<b>Physical Attack +2</b>",
                           PhysicalAttack = 2
                       };
        }

        private static HeroDef Knight()
        {
            return new HeroDef
            {
                Level = 2,
                Name = "Knight",
                Strength = 7,
                Cost = 10,
                Text = "<b>Physical Attack +5</b>",
                PhysicalAttack = 5
            };
        }

        private static HeroDef Captain()
        {
            return new HeroDef
            {
                Level = 3,
                Name = "Captain",
                Strength = 8,
                Cost = 13,
                Text = "<b>Physical Attack +8</b>",
                PhysicalAttack = 8
            };
        }

    }
}
