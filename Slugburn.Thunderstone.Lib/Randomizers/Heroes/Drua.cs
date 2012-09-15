using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Drua : HeroRandomizer
    {
        public Drua() : base("Drua", "Elf", "Cleric")
        {
            Text =
                "This cleric has Physical Attack. She also destroys cards in your hand, and draws cards at higher levels.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get { return new[] {Sacrist(), Cursesworn(), Purifier()}; }
        }

        private static HeroDef Sacrist()
        {
            return new HeroDef
                       {
                           Level=1,
                           Name="Sacrist",
                           Strength = 4,
                           Cost = 6,
                           Text = "<b>Physical Attack +2</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Destroy 1 card in your hand.",
                           PhysicalAttack = 2,
                           Modify = card=>card
                               .CreateAbility()
                               .DestroyCard()
                               .On(Phase.Dungeon)
                       };
        }

        private static HeroDef Cursesworn()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Cursesworn",
                           Strength = 4,
                           Cost = 9,
                           Text = "<b>Physical Attack +3</b>"
                                  + "<br/><br/>"
                                  + "<b>Village/Dungeon:</b> Destroy 1 card in your hand."
                                  + "<br/><br/>"
                                  + "<b>Village/Dungeon:</b> Destroy 1 disease to draw 1 card.",
                           PhysicalAttack = 3,
                           Modify = card => card.CreateAbility()
                               .DestroyCard()
                               .On(Phase.Village, Phase.Dungeon)
                               .DestroyDiseaseToDrawCard()
                               .On(Phase.Village, Phase.Dungeon)
                       };
        }

        private static HeroDef Purifier()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Purifier",
                           Strength = 5,
                           Cost = 12,
                           Text = "<b>Physical Attack +4</b>"
                                  + "<br/><br/>"
                                  + "<b>Village/Dungeon:</b> Destroy 1 card in your hand."
                                  + "<br/><br/>"
                                  + "<b>Village/Dungeon:</b> Destroy 1 disease to draw 2 cards.",
                           PhysicalAttack = 4,
                           Modify = card => card.CreateAbility()
                                                .DestroyCard()
                                                .On(Phase.Village, Phase.Dungeon)
                                                .DestroyDiseaseToDrawCard(2)
                                                .On(Phase.Village, Phase.Dungeon)
                       };
        }
    }
}