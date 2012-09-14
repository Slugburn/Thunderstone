using System;
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
            var dungeon = e.Game.Dungeon;

            // switch Stramst with the monster in front of him
            var oldRank = dungeon.GetRankNumber(stramst.Rank ?? 0);
            var newRank = dungeon.GetRankNumber(oldRank.Number + 1);

            // if Stramst is in the last rank, or if there is no card to the right of Stramst then do nothing
            if (newRank == null || newRank.Card == null) return;

            // switch Stramst with the card in the next higher rank
            var switchWith = newRank.Card;
            oldRank.Card = switchWith;
            newRank.Card = stramst;
        }
    }
}
