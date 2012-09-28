using System;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;

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

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .DestroyCard("Destroy a hero.", c => c.IsHero())
                .On(Phase.Battle);

            card.AddEventHandler(events => events.Subscribe<DungeonHallRefilled>(e => KeepStramstFromAdvancing(card, e)));
        }

        private static void KeepStramstFromAdvancing(Card stramst, DungeonHallRefilled e)
        {
            // if any monster is in a higher rank than Stramst then
            // move Stramst right one and move the monster into his old place
            var dungeon = e.Game.Dungeon;
            var monsterRank = dungeon.GetRanksAfter(stramst.Rank).FirstOrDefault(r => r.Card != null);
            if (monsterRank == null)
                return;
            var monster = monsterRank.Card;

            var currentRank = stramst.Rank;
            monsterRank.Card = null;
            currentRank.PreviousRank.Card = stramst;
            currentRank.Card = monster;
        }
    }
}
