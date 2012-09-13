namespace Slugburn.Thunderstone.Lib.Events
{
    class CardAbilityUsed
    {
        public CardAbilityUsed(Ability ability)
        {
            Ability = ability;
        }

        public Ability Ability { get; set; }
    }
}
