namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public class PlusMod : IAttributeMod
    {
        public Card Source { get; set; }
        public Attribute Attribute { get; set; }
        public int Amount { get; set; }

        public PlusMod(Card source, Attribute attribute, int amount)
        {
            Source = source;
            Attribute = attribute;
            Amount = amount;
        }

        public int Modify(int startValue)
        {
            return startValue + Amount;
        }
    }
}