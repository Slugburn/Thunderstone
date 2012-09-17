namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public interface IAttributeMod
    {
        Card Source { get; set; }
        Attr Attribute { get; set; }
        int Modify(Card card, int startValue);
    }
}