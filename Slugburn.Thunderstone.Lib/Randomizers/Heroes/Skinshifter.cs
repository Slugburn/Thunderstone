using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Skinshifter :HeroRandomizer
    {
        public Skinshifter()
            : base("Skinshifter", "Dwarf", "Cleric")
        {
            Text = "This cleric has Physical Attack. He gains Physical Attack and Strength when you discard cards.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Clawhand(), Mauler(), Terror()};
            }
        }

        private static HeroDef Clawhand()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Clawhand",
                           Strength = 3,
                           Cost = 7,
                           Text = "<b>Physical Attack +1</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Discard a card to add Physical Attack +2 and Strength +1.",
                           PhysicalAttack = 1,
                           Modify = card => CreateSkinshifterAbility(card, 2, 1)
                       };
        }

        private static HeroDef Mauler()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Mauler",
                           Strength = 4,
                           Cost = 10,
                           Text = "<b>Physical Attack +2</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Discard a card to add Physical Attack +3 and Strength +2.",
                           PhysicalAttack = 2,
                           Modify = card => CreateSkinshifterAbility(card, 3, 2)
                       };
        }

        private static HeroDef Terror()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Terror",
                           Strength = 5,
                           Cost = 13,
                           Text = "<small><b>Physical Attack +4</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Discard or destroy a card to add Physical Attack +4 and Strength +3. " 
                                  + "If the card was a disease, you may use this ability a second time this turn.</small>",
                           PhysicalAttack = 4,
                           Modify = card =>
                                        {
                                            CreateSkinshifterAbility(card,4, 3);
                                            card.CreateAbility()
                                                .Description("Destroy a card to add Physical Attack +4 and Strength +3. "
                                                             + "If the card was a disease, you may use this ability a second time this turn.")
                                                .SelectCards(source => source.FromHand().Filter(c => c != card).Caption(card.Name).Message("Destroy a card."))
                                                .OnCardsSelected(x =>
                                                                     {
                                                                         x.Player.DestroyCard(x.Selected.First(), card.Name);
                                                                         card.AddModifier(new PlusMod(card, Attr.PhysicalAttack, 4));
                                                                         card.AddModifier(new PlusMod(card, Attr.Strength, 3));
                                                                     })
                                                .On(Phase.Dungeon);
                                        }
                       };
        }

        private static void CreateSkinshifterAbility(Card card, int attack, int strength)
        {
            card.CreateAbility()
                .Description("Discard a card to add Physical Attack +{0} and Strength +{1}.".Template(attack, strength))
                .SelectCards(source => source.FromHand().Filter(c => c != card).Caption(card.Name).Message("Discard a card."))
                .OnCardsSelected(x =>
                {
                    x.Player.DiscardCard(x.Selected.First());
                    card.AddModifier(new PlusMod(card, Attr.PhysicalAttack, attack));
                    card.AddModifier(new PlusMod(card, Attr.Strength, strength));
                })
                .On(Phase.Dungeon);
        }


    }
}
