using System;
using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public class OgreHumanoid : MonsterRandomizer
    {
        public OgreHumanoid() : base(2, "Ogre", "Humanoid")
        {
            Text = "Ogres discard and destroy heroes and weapons as Battle and Aftermath effects.";
        }

        protected override IEnumerable<MonsterDef> MonsterDefs
        {
            get
            {
                return new[]
                           {
                               Ogrillon(),
                               Ogre(),
                               OgreMage(),
                               Ettin()
                           };
            }
        }

        private static MonsterDef Ogrillon()
        {
            Func<Card, bool> isEquippedWeapon = x => x.IsWeapon() && x.IsEquipped;
            return new MonsterDef
                       {
                           Name = "Ogrillon",
                           Count = 3,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 3,
                           Text = "<b>Battle:</b> Discard 1 equipped weapon.",
                           Modify = card => card.CreateAbility().DiscardCard("Discard 1 equipped weapon.", isEquippedWeapon).On(Phase.Battle)
                       };
        }

        private static MonsterDef Ogre()
        {
            Func<Card, bool> filter = x => x.IsHero() && x.Strength <= 3;
            return new MonsterDef
                       {
                           Name = "Ogre",
                           Count = 3,
                           Health = 7,
                           Gold = 1,
                           Xp = 2,
                           Vp = 2,
                           Text = "<b>Aftermath:</b> Destroy 1 hero with Strength 3 or less.",
                           Modify = card=>card.CreateAbility().DestroyCard("Destroy 1 hero with Strength 3 or less", x => x.IsHero() && x.Strength <= 3).On(Phase.Aftermath)
                       };
        }

        private static MonsterDef OgreMage()
        {
            Func<Card, bool> filter = x => x.IsHero() && x.Strength <= 3;
            return new MonsterDef
                       {
                           Name = "Ogre Mage",
                           Count = 2,
                           Health = 8,
                           Gold = 2,
                           Xp = 2,
                           Vp = 4,
                           Text = "<b>Battle:</b> Discard 1 card with Magic Attack."
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> Destroy 1 hero with Strength 4 or less.",
                        Modify = card => card.CreateAbility()
                                             .DiscardCard("Discard 1 card with Magic Attack.", x => x.HasMagicAttack()).On(Phase.Battle)
                                             .DestroyCard("Destroy 1 hero with Strength 4 or less", x => x.IsHero() && x.Strength <= 4).On(Phase.Aftermath)
                       };
        }

        private static MonsterDef Ettin()
        {
            return new MonsterDef
                       {
                           Name = "Ettin",
                           Count = 2,
                           Health = 9,
                           Gold = 3,
                           Xp = 2,
                           Vp = 5,
                           Text = "<b>Battle:</b> Destroy 1 hero without Magic Attack."
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> Destroy 1 hero without Physical Attack.",
                           Modify = card => card.CreateAbility()
                                                .DestroyCard("Destroy 1 hero without Magic Attack", x => x.IsHero() && !x.HasMagicAttack())
                                                .On(Phase.Battle)
                                                .DestroyCard("Destroy 1 hero without Physical Attack", x => x.IsHero() && !x.HasPhysicalAttack())
                                                .On(Phase.Aftermath)
                       };
        }



    }
}
