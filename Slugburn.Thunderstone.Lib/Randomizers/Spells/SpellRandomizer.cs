namespace Slugburn.Thunderstone.Lib.Randomizers.Spells
{
    abstract class SpellRandomizer : VillageRandomizer
    {
        protected SpellRandomizer(string name, params string[] additionalTags) : base(CardType.Spell, name, additionalTags)
        {
        }
    }
}