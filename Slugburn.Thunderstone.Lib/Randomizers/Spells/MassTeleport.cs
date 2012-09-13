using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Spells
{
    class MassTeleport : SpellRandomizer
    {
        public MassTeleport() : base("Mass Teleport")
        {
            Cost = 5;
            Text =
                "<b>Dungeon</b> Draw 3 cards. You cannot use any more Dungeon abilities this turn <i>(you may still equip weapons).</i>";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.CreateAbility()
                .Description("Draw 3 cards. You cannot use any more Dungeon abilities this turn <i>(you may still equip weapons).</i>")
                .Action(player =>
                            {
                                player.Draw(3);
                                player.ActiveAbilities.RemoveAll(x => x.Phase == Phase.Dungeon);
                            })
                .On(Phase.Dungeon);
        }
    }
}
