using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    class Bhoidwood : HeroRandomizer
    {
        public Bhoidwood() : base("Bhoidwood", "Elf", "Ranger")
        {
            Text =
                "This ranger has Physical Attack. At higher levels, she can rearrange monster positions in the hall.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[]
                           {
                               Hunter(),
                               Stalker(),
                               Slayer()
                           };
            }
        }

        private static HeroDef Hunter()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Hunter",
                           Gold = -1,
                           Strength = 5,
                           Cost = 6,
                           Text = "<b>Physical Attack +2</b>",
                           PhysicalAttack = 2,
                       };
        }

        private static HeroDef Stalker()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Stalker",
                           Gold = -2,
                           Strength = 5,
                           Cost = 9,
                           Text = "<b>Physical Attack +4</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Switch the positions of two adjacent monsters in the hall.",
                           PhysicalAttack = 4,
                           Modify = card => card.CreateAbility()
                                                .Description("Switch the positions of two adjacent monsters in the hall")
                                                .SelectCards(source => source.FromHall().Caption("Switch Positions").Message("Pick first monster to switch"))
                                                .OnCardsSelected(x => {})
                                                .SelectCards(source => source
                                                                           .FromHall()
                                                                           .Filter(x =>
                                                                                       {
                                                                                           var firstCard = source.SelectionContext.PreviousSelection.First();
                                                                                           return Math.Abs((x.Rank ?? 0) - (firstCard.Rank ?? 0)) == 1;
                                                                                       })
                                                                           .Caption("Switch Positions")
                                                                           .Message("Pick second monster to switch"))
                                                .OnCardsSelected(x =>
                                                                     {
                                                                         var monster1 = x.PreviousSelection.First();
                                                                         var monster2 = x.Selected.First();
                                                                         var dungeon = x.Game.Dungeon;
                                                                         var rank1 = dungeon.GetRankOf(monster1);
                                                                         var rank2 = dungeon.GetRankOf(monster2);
                                                                         rank1.Card = monster2;
                                                                         rank2.Card = monster1;
                                                                         x.Game.SendUpdateDungeon();
                                                                     } )
                                                .On(Phase.Dungeon)
                       };
        }

        private HeroDef Slayer()
        {
            return new HeroDef
            {
                Level = 3,
                Name = "Slayer",
                Gold = -3,
                Strength = 6,
                Cost = 12,
                Text = "<b>Physical Attack +6</b>"
                        + "<br/><br/>"
                        + "<b>Dungeon:</b> Rearrange the hall.",
                PhysicalAttack = 6,
            };

        }


    }
}