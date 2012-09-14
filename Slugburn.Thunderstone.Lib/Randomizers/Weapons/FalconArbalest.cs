using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Modifiers;
using System;
using Attribute = Slugburn.Thunderstone.Lib.Modifiers.Attribute;

namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    class FalconArbalest : WeaponRandomizer
    {
        public FalconArbalest() : base("Falcon Arbalest")
        {
            Gold = 2;
            Strength = 3;
            Cost = 7;
            Text = "Reduce the equipped hero's Attack to 0.<br/><br/>Physical Attack +5 against rank 2 or higher.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);

            card.PhysicalAttack = 0;

            card.CreateAbility()
                .EquipWeapon((player, hero) =>
                                 {
                                     hero.AddModifier(new SetMod(card, Attribute.PhysicalAttack, 0));
                                     hero.AddModifier(new SetMod(card, Attribute.MagicalAttack, 0));
                                 }).On(Phase.Equip);

            card.AddEventHandler(events => events.Subscribe<AttackRankSelected>(e =>
                                                              {
                                                                  if (e.AttackedRank.Number >= 2 && card.IsEquipped)
                                                                      card.GetEquipped().AddModifier(new PlusMod(card, Attribute.PhysicalAttack, 5));
                                                              }));
        }
    }
}
