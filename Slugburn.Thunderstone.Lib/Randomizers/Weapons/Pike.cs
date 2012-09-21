using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Modifiers;

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
            card.PotentialPhysicalAttack = () => 2;

            card.CreateAbility()
                .EquipWeapon(Attr.PhysicalAttack, 2)
                .On(Phase.Equip);

            card.AddEventHandler(
                events =>
                events.Subscribe<SelectingCards>(
                    message =>
                        {
                            if (message.TriggeringAbility != null && message.TriggeringAbility.Phase == Phase.Battle && card.IsEquipped)
                                message.Selection.Remove(card.GetEquipped());
                        }));
        }
    }
}