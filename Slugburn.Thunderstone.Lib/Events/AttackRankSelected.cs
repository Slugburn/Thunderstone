using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Thunderstone.Lib.Events
{
    class AttackRankSelected
    {
        public Player Player { get; set; }

        public Rank AttackedRank { get; set; }
    }
}
