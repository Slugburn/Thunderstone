using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    class SnakeheadFlail : WeaponRandomizer
    {
        public SnakeheadFlail() : base("Snakehead Flail", "Magic")
        {
            Gold = 1;
            Strength = 3;
            Cost = 3;
            Text = "<b>Magic Attack +1</b><br/><br/>Additional Magic Attack equal to the level of the equipped hero.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.MagicAttack = 1;
            card.CreateAbility()
                .EquipWeapon((player, hero) => hero.AddModifier(new PlusMod(card, Attr.MagicalAttack, 1 + hero.Level)))
                .On(Phase.Equip);
        }
    }
}