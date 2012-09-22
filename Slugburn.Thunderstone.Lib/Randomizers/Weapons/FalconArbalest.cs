using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Modifiers;
using System;

namespace Slugburn.Thunderstone.Lib.Randomizers.Weapons
{
    public class FalconArbalest : WeaponRandomizer
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

            card.PotentialPhysicalAttack = () => 5;

            card.CreateAbility()
                .EquipWeapon((player, hero) =>
                                 {
                                     hero.AddModifier(new SetMod(card, Attr.PhysicalAttack, 0));
                                     hero.AddModifier(new SetMod(card, Attr.MagicalAttack, 0));
                                 }).On(Phase.Equip);

            card.AddEventHandler(events => events.Subscribe<AttackRankSelected>(e =>
                                                              {
                                                                  if (e.AttackedRank.Number >= 2 && card.IsEquipped)
                                                                      card.GetEquipped().First().AddModifier(new PlusMod(card, Attr.PhysicalAttack, 5));
                                                              }));
        }
    }
}
