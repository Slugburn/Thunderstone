using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Rapparee : HeroRandomizer
    {
        public Rapparee() : base("Rapparee", "Elf", "Thief")
        {
            Text = "This thief has Physical Attack. He brings a lot of gold to spend. At higher levels, his Spoils ability lets you buy cards.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Scrounger(), Skimmer(), Looter()};
            }
        }

        private static HeroDef Scrounger()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Scrounger",
                           Gold = 3,
                           Strength = 3,
                           Cost = 5,
                           Text = "<b>Physical Attack +1</b>",
                           PhysicalAttack = 1,
                       };
        }

        private static HeroDef Skimmer()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Skimmer",
                           Gold = 3,
                           Strength = 4,
                           Cost = 9,
                           Text = "<b>Physical Attack +2</b>"
                                  + "<br/><br/>"
                                  + "<b>Spoils:</b> Buy 1 item or weapon.",
                           PhysicalAttack = 2,
                           Modify = card => card.CreateAbility()
                                                .Description("Buy 1 item or weapon.")
                                                .BuyCard(c => c.Type == CardType.Item || c.Type == CardType.Weapon)
                                                .On(Phase.Spoils)
                       };
        }

        private static HeroDef Looter()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Looter",
                           Gold = 4,
                           Strength = 4,
                           Cost = 12,
                           Text = "<b>Physical Attack +3</b>"
                                  + "<br/><br/>"
                                  + "<b>Spoils:</b> Buy 1 village card. Place it on top of your deck.",
                           PhysicalAttack = 3,
                           Modify = card => card.CreateAbility()
                                                .Description("Buy 1 village card. Place it on top of your deck.")
                                                .BuyCard()
                                                .Action(player =>
                                                            {
                                                                var bought = player.Discard.Last();
                                                                player.Discard.Remove(bought);
                                                                player.AddToTopOfDeck(bought);
                                                            })
                                                .On(Phase.Spoils)
                       };
        }

    }
}