using System;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class HeroDef
    {
        public int Level { get; set; }

        public string Name { get; set; }

        public int Strength { get; set; }

        public int Cost { get; set; }

        public string Text { get; set; }

        public Action<Card> Modify { get; set; }

        public int? PhysicalAttack { get; set; }

        public int? MagicAttack { get; set; }

        public int? Gold { get; set; }

        public int? Light { get; set; }

        public virtual int Count
        {
            get
            {
                switch (Level)
                {
                    case 1:
                        return 6;
                    case 2:
                        return 4;
                    case 3:
                        return 2;
                    default:
                        throw new Exception("Unable to determine count for Level ".Template(Level));
                }
            }
        }

        public virtual int? Vp
        {
            get { return Level == 3 ? 2 : (int?) null; }
        }

        public virtual int? Xp
        {
            get
            {
                switch (Level)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 3;
                    default:
                        return null;
                }
            }
        }
    }
}