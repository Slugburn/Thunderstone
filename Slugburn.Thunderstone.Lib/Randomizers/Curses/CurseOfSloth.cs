namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfSloth : CurseRandomizer
    {
        public CurseOfSloth() : base("Sloth")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<small><b>Dungeon:</b> Destroy this curse. Lower your Total Attack Value by 3. " +
                   "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.</small>";
        }

        protected override void Modify(Card card)
        {
            // TODO: Implement
        }
    }
}
