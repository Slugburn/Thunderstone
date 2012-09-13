namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public interface IAttributeMod
    {
        Card Source { get; set; }
        Attribute Attribute { get; set; }
        int Modify(int startValue);
    }
}