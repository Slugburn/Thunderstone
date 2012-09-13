namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    abstract class WeaponRandomizer : VillageRandomizer
    {
        protected WeaponRandomizer(string name, params string[] additionalTags) : base(CardType.Weapon, name, additionalTags)
        {
        }
    }
}