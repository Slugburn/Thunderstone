namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    public class Pike : WeaponRandomizer
    {
        public Pike() : base("Pike", "Polearm")
        {
            Gold = 2;
            Strength = 3;
            Cost = 4;
            Text = "<b>Physical Attack +2</b><br/><br/>This hero is not affected by Battle effects.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.PhysicalAttack = 2;

            // TODO: Implement Battle effect immunity
        }
    }
}