using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class StatusModel
    {
        public int? Gold { get; set; }

        public int Xp { get; set; }

        public int Vp { get; set; }

        public static StatusModel From(Player player)
        {
            return new StatusModel { Gold = player.Hand.Sum(x => x.Gold), Xp = player.Xp, Vp = player.Vp };
        }
    }
}
