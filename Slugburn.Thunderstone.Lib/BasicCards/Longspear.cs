﻿using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.BasicCards
{
    internal class Longspear : ICardGen
    {
        #region ICardGen Members

        public Card Create()
        {
            var card = new Card
                           {
                               Type = CardType.Weapon,
                               Name = "Longspear",
                               Gold = 2,
                               Strength = 3,
                               Cost = 3,
                               Text = "<b>Physical Attack +1</b>",
                               PhysicalAttack = 1,
                           };
            card.SetTags("Weapon", "Polearm", "Basic");
            card.CreateAbility().EquipWeapon(Attribute.PhysicalAttack, 1).On(Phase.Equip);
            return card;
        }

        #endregion
    }
}