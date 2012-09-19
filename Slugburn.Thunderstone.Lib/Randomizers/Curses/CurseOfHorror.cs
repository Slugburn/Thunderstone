namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfHorror : CurseRandomizer
    {
        public CurseOfHorror() : base("Horror")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<small><b>Dungeon:</b> Destroy this curse. Reduce your Light to 0. "
                   + "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.</small>";
        }

        protected override void Modify(Card card)
        {
            // TODO: Implement
        }
    }
}