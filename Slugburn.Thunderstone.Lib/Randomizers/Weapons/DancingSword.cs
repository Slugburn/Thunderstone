namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    public class DancingSword : WeaponRandomizer
    {
        public DancingSword() : base("Dancing Sword", "Edged", "Magic", "Light")
        {
            Gold = 2;
            Strength = 5;
            Cost = 8;
            Light = 1;
            Text = "<b>Physical Attack +2</b><br/><br/>Even if not equipped, Magic Attack +2 <i>(and still provides light).</i>.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.PhysicalAttack = 2;
            card.MagicAttack = 2;

            // TODO: Implement dancing
        }
    }
}