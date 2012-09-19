namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public interface IAttributeMod
    {
        Card Source { get; set; }
        Attr Attribute { get; set; }
        int Modify(IAttrSource target, int startValue);
    }
}