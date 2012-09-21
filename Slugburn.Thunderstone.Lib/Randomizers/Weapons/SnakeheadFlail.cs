using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    public class SnakeheadFlail : WeaponRandomizer
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
            card.PotentialMagicAttack = ()=>card.Player != null ? 1+card.Player.Hand.Where(c=>c.IsHero()).Max(c=>c.Level) : (int?)null;
            card.CreateAbility()
                .EquipWeapon((player, hero) => hero.AddModifier(new PlusMod(card, Attr.MagicalAttack, 1 + hero.Level)))
                .On(Phase.Equip);
        }
    }
}