using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class StatusModel
    {
        public int? Gold { get; set; }

        public int Xp { get; set; }

        public int Vp { get; set; }

        public int PhysicalAttack { get; set; }

        public int MagicAttack { get; set; }

        public int Light { get; set; }

        public static StatusModel From(Player player)
        {
            return new StatusModel
                       {
                           Gold = player.AvailableGold,
                           Xp = player.Xp,
                           Vp = player.Vp,
                           PhysicalAttack = player.Hand.Sum(x => x.PhysicalAttack ?? 0),
                           MagicAttack = player.Hand.Sum(x => x.MagicAttack ?? 0),
                           Light = player.Hand.Sum(x => x.Light ?? 0)
                       };
        }
    }
}
