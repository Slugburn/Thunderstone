using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    class CurseOfWar : CurseRandomizer
    {
        public CurseOfWar() : base("War")
        {
        }

        protected override string GetAdditionalText()
        {
            return "{ this is overridden by Modify() }";
        }

        protected override void Modify(Card card)
        {
            card.Text = "You cannot rest or prepare."
                   + "<br/><br/>"
                   + "<b>Trophy:</b> Attack -1"
                   + "<br/><br/>" + "<b>Village/Dungeon:</b> Select 2 random cards from your hand <i>(excluding this card).</i>  "
                   + "You may destroy 1 of them to destroy this card.";
            // TODO: Implement
        }
    }
}
