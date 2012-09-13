namespace Slugburn.Thunderstone.Lib.Randomizers.ThunderstoneBearers
{
    public class Stramst : ThunderstoneBearer
    {
        public Stramst() : base("Stramst")
        {
            Health = 12;
            Gold = 3;
            Xp = 3;
            Vp = 4;
            Text =
                "<small><b>Global:</b> When refilling the hall, Stramst does not move forward to an empty rank until the dungeon deck is depleted. " +
                "Instead, refill the empty rank in front of him.<br/><br/><b>Battle:</b> Destroy a hero.</small>";
        }
    }
}
