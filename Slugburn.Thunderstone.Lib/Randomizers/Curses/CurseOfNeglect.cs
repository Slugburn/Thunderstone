namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfNeglect : CurseRandomizer
    {
        public CurseOfNeglect() : base("Neglect")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village:</b> Lose 2 gold to destroy this curse.";
        }

        protected override void Modify(Card card)
        {
            // TODO: Implement
        }
    }
}