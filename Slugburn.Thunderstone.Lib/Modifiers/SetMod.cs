namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public class SetMod : IAttributeMod
    {
        public SetMod(Card source, Attr attribute, int to)
        {
            Source = source;
            Attribute = attribute;
            To = to;
        }

        public Card Source { get; set; }
        
        public Attr Attribute { get; set; }

        public int To { get; set; }

        public int Modify(Card card, int startValue)
        {
            return To;
        }
    }
}
