namespace Slugburn.Thunderstone.Lib.Randomizers.Villagers
{
    abstract class VillagerRandomizer : VillageRandomizer
    {
        protected VillagerRandomizer(string name, params string[] additionalTags) : base(CardType.Villager, name, additionalTags)
        {
        }
    }
}