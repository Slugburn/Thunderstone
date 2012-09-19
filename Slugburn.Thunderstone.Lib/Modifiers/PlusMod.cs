using System;

namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public class PlusMod : IAttributeMod
    {
        public Func<IAttrSource, int> GetAmount { get; set; }
        public Card Source { get; set; }
        public Attr Attribute { get; set; }
        public int Amount { get; set; }

        public PlusMod(Card source, Attr attribute, int amount)
        {
            Source = source;
            Attribute = attribute;
            GetAmount = x => amount;
        }

        public PlusMod(Card source, Attr attribute, Func<IAttrSource, int> getAmount)
        {
            Source = source;
            Attribute = attribute;
            GetAmount = getAmount;
        }

        public int Modify(IAttrSource target, int startValue)
        {
            return startValue + GetAmount(target);
        }
    }
}