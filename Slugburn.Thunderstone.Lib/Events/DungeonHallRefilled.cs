using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Thunderstone.Lib.Events
{
    public class DungeonHallRefilled
    {
        public Game Game { get; set; }

        public DungeonHallRefilled(Game game)
        {
            Game = game;
        }
    }
}
