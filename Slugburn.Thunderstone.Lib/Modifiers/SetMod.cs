namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public class SetMod : IAttributeMod
    {
        public SetMod(Card source, Attribute attribute, int to)
        {
            Source = source;
            Attribute = attribute;
            To = to;
        }

        public Card Source { get; set; }
        
        public Attribute Attribute { get; set; }

        public int To { get; set; }

        public int Modify(int startValue)
        {
            return To;
        }
    }
}
