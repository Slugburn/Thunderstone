namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public interface IAttrSource
    {
        Game Game { get; }

        IAttributeMod[] GetModifiersFor(Attr attr);
        int? GetBaseValue(Attr attr);
    }
}