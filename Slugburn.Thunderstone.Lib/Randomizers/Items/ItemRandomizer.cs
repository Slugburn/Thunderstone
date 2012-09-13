namespace Slugburn.Thunderstone.Lib.Randomizers.Items
{
    abstract class ItemRandomizer : VillageRandomizer
    {
        protected ItemRandomizer(string name, params string[] additionalTags) : base(CardType.Item, name, additionalTags)
        {
        }
    }
}