namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfShame : CurseRandomizer
    {
        public CurseOfShame() : base("Shame")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village/Dungeon:</b> Destroy this curse. Draw 2 fewer cards when you draw a new hand.";
        }

        protected override void Modify(Card card)
        {
            // TODO: Implement
        }
    }
}