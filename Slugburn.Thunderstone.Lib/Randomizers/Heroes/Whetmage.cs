using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Heroes
{
    public class Whetmage : HeroRandomizer
    {
        public Whetmage() : base("Whetmage", "Human", "Wizard")
        {
            Text =
                "This wizard has Magic Attack. He also levels up other heroes in the dungeon before combat.";
        }

        protected override IEnumerable<HeroDef> HeroDefs
        {
            get
            {
                return new[] { Honer(), Finisher(), Polisher() };
            }
        }

        private static HeroDef Honer()
        {
            return new HeroDef
                       {
                           Level = 1,
                           Name = "Honer",
                           Strength = 2,
                           Cost = 5,
                           Text = "<b>Magic Attack +1</b>"
                           + "<br/><br/>"
                           + "<b>Dungeon:</b> Level up a Regular or another level 1 hero. Add the newly leveled hero to your hand.",
                           MagicAttack = 1,
                           Modify = card=>
                                        {
                                            Func<Card, bool> validCardFilter =
                                                c => c != card && c.IsHero() && c.Xp <= c.Player.Xp && c.Level <= 1 && c.Player.AvailableLevelUps(c).Any();
                                            CreateLevelUpHeroAbility(card, validCardFilter, "Level up a Regular or another level 1 hero.").On(Phase.Dungeon);
                                        }
                       };
        }

        private static HeroDef Finisher()
        {
            return new HeroDef
                       {
                           Level = 2,
                           Name = "Finisher",
                           Strength = 2,
                           Cost = 8,
                           Text = "<b>Magic Attack +3</b>"
                           + "<br/><br/>"
                           + "<b>Dungeon:</b> Level up another hero. Add the newly leveled hero to your hand.",
                           MagicAttack = 3,
                           Modify = card=>
                                        {
                                            Func<Card, bool> validCardFilter =
                                                c => c != card && c.IsHero() && c.Xp <= c.Player.Xp && c.Player.AvailableLevelUps(c).Any();
                                            CreateLevelUpHeroAbility(card, validCardFilter, "Level up another hero.").On(Phase.Dungeon);
                                        }
                       };
        }

        private static HeroDef Polisher()
        {
            return new HeroDef
                       {
                           Level = 3,
                           Name = "Polisher",
                           Strength = 3,
                           Cost = 11,
                           Text = "<small><b>Magic Attack +5</b>"
                           + "<br/><br/>"
                           + "<b>Dungeon:</b> Level up any number of heroes. Add the newly leveled heroes to your hand. You cannot level a hero that has already been leveled up this turn.</small>",
                           MagicAttack = 5,
                           Modify = card =>
                           {
                               Func<Card, bool> validCardFilter =
                                   c => 
                                       c != card 
                                       && c.IsHero() 
                                       && c.Xp <= c.Player.Xp 
                                       && c.Player.AvailableLevelUps(c).Any()
                                       && (card.GetData<List<Card>>() == null || !card.GetData<List<Card>>().Contains(c));
                               CreateLevelUpHeroAbility(card, validCardFilter, "Level up any number of heroes.")
                                   .Repeatable()
                                   .On(Phase.Dungeon);
                           }
                       };
        }

        private static IAbilityDefinedSyntax CreateLevelUpHeroAbility(Card card, Func<Card, bool> validCardFilter, string abilityDescription)
        {
            return card.CreateAbility()
                .Description(abilityDescription)
                .SelectCards(x => x.Select().HeroToLevel(validCardFilter))
                .OnCardsSelected(x => { })
                .SelectCards(x =>
                    {
                        var hero = x.Select().SelectionContext.Selected.First();
                        return x.Select().SelectHeroUpgrade(hero);
                    })
                .OnCardsSelected(x =>
                    {
                        var hero = x.Selections[0].First();
                        var upgrade = x.Selected.First();
                        x.Source.Draw(new[] {upgrade});
                        x.Player.Xp -= (hero.Xp ?? 0);
                        x.Player.DestroyCard(hero, "Upgrading to {0}".Template(upgrade.Name));
                        var leveledUp = card.GetData<List<Card>>() ?? new List<Card>();
                        leveledUp.Add(upgrade);
                        card.SetData(leveledUp);
                    });
        }

    }
}