using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Disowned : HeroRandomizer
    {
        public Disowned() : base("Disowned", "Dwarf", "Fighter")
        {
            Text = "This fighter has Physical Attack. In her rage, she often discards or destroys other heroes in your hand as Aftermath effects.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Berserker(), Reaver(), Bloodrager()};
            }
        }

        private static HeroDef Berserker()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Berserker",
                           Strength = 6,
                           Cost = 6,
                           Text = "<b>Physical Attack +2</b>"
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> Destroy 1 other hero.",
                           PhysicalAttack = 2,
                           Modify = card => CreateDestroyOtherHeroAbility(card).On(Phase.Aftermath)
                       };
        }

        private static IAbilityCardsSelectedSyntax CreateDestroyOtherHeroAbility(Card card)
        {
            return card.CreateAbility()
                .Description("Destroy 1 other hero.")
                .SelectCards(x => x.Select().FromHand()
                                      .Filter(c => c != card && c.IsHero())
                                      .Caption(card.Name).Message("Destroy 1 hero."))
                .OnCardsSelected(x => x.Player.DestroyCard(x.Selected.First(), card.Name));
        }

        private static HeroDef Reaver()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Reaver",
                           Strength = 6,
                           Cost = 8,
                           Text = "<b>Physical Attack +5</b>"
                                  + "<br/><br/>"
                                  + "<b>Dungeon:</b> Destroy 1 hero."
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> If equipped with a weapon, destroy 1 other hero.",
                           PhysicalAttack = 5,
                           Modify = card =>
                                        {
                                            CreateDestroyOtherHeroAbility(card).On(Phase.Dungeon);
                                            CreateDestroyOtherHeroAbility(card)
                                                .Condition(p => card.IsEquipped)
                                                .On(Phase.Aftermath);
                                        }
                       };
        }

        private static HeroDef Bloodrager()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Bloodrager",
                           Strength = 7,
                           Cost = 11,
                           Text = "<b>Physical Attack +8</b>"
                                  + "<br/><br/>"
                                  + "This hero can equip two weapons."
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> Destroy all other heroes. Place this card on top of your deck.",
                           PhysicalAttack = 8,
                           Modify = card =>
                               {
                                   card.CanEquip = () => card.GetEquipped() == null || card.GetEquipped().Count() < 2;
                                   card.CreateAbility()
                                       .Description("Destroy all other heroes. Place Bloodrager on top of your deck.")
                                       .Action(x =>
                                           {
                                               x.Player.SelectCard()
                                                   .FromHand()
                                                   .Filter(c => c != card && c.IsHero())
                                                   .Matches()
                                                   .Each(c => x.Player.DestroyCard(c, card.Name));
                                               x.Player.DiscardToTopOfDeck(card);
                                           })
                                       .On(Phase.Aftermath);
                               }
                       };
        }

    }
}