using System;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public class MonsterDef
    {
        public int Count { get; set; }

        public string Name { get; set; }

        public int Health { get; set; }

        public int? Gold { get; set; }

        public string Text { get; set; }

        public int Xp { get; set; }

        public int Vp { get; set; }

        public Action<Card> Modify { get; set; }

    }
}