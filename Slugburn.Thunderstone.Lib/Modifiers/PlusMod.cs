using System;

namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public class PlusMod : IAttributeMod
    {
        public Func<Card, int> GetAmount { get; set; }
        public Card Source { get; set; }
        public Attr Attribute { get; set; }
        public int Amount { get; set; }

        public PlusMod(Card source, Attr attribute, int amount)
        {
            Source = source;
            Attribute = attribute;
            GetAmount = x => amount;
        }

        public PlusMod(Card source, Attr attribute, Func<Card, int> getAmount)
        {
            Source = source;
            Attribute = attribute;
            GetAmount = getAmount;
        }

        public int Modify(Card card, int startValue)
        {
            return startValue + GetAmount(card);
        }
    }
}