using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public class UndeadSkeleton : MonsterRandomizer
    {
        public UndeadSkeleton() : base(1, "Undead", "Skeleton")
        {
            Text = "Many skeletons give curses as a Battle effect, and some provide Trophy effects. Use the curses deck.";
        }

        protected override IEnumerable<MonsterDef> MonsterDefs
        {
            get
            {
                return new[]
                           {
                               WarriorBones(),
                               DwarvenAncestor(),
                               Ossuous(),
                               Necrophidius(),
                               ShroudedCadaver()
                           };
            }
        }

        private static MonsterDef WarriorBones()
        {
            return new MonsterDef
                       {
                           Name = "Warrior Bones",
                           Count = 2,
                           Health = 5,
                           Gold = 1,
                           Xp = 1,
                           Vp = 1,
                           Text = "<b>Trophy:</b> Physical Attack +1.",
                           Modify = card => card.PhysicalAttack = 1
                       };
        }

        private static MonsterDef DwarvenAncestor()
        {
            return new MonsterDef
                       {
                           Name = "Dwarven Ancestor",
                           Count = 2,
                           Health = 6,
                           Gold = 1,
                           Xp = 1,
                           Vp = 2,
                           Text = "<b>Battle:</b> If there is Darkness, gain 1 curse."
                                  + "<br/><br/>"
                                  + "<b>Battle:</b> If you defeat this card's Health by 4 or more, each other player gains 1 curse.",
                           Modify = card => card
                                                .CreateAbility()
                                                .Description("If there is Darkness, gain 1 curse.")
                                                .Action(x => x.Player.GainCurse())
                                                .Condition(player => (player.TotalLight + card.Darkness) < 0)
                                                .On(Phase.Battle)
                                                .CreateAbility()
                                                .Description("If you defeat this card's Health by 4 or more, each other player gains 1 curse.")
                                                .Action(x => x.Player.Game.Players
                                                                      .Where(p => p != x.Player)
                                                                      .Each(p => p.GainCurse()))
                                                .Condition(player => player.GetBattleMarginVersus(card) >= 4)
                                                .On(Phase.Battle)
                       };
        }

        private static MonsterDef Ossuous()
        {
            return new MonsterDef
                       {
                           Name = "Ossuous",
                           Count = 2,
                           Health = 6,
                           Gold = 1,
                           Xp = 2,
                           Vp = 2,
                           Text = "Cannot be fought unless a hero of level 1 or higher is present."
                                  + "<br/><br/>"
                                  + "<b>Trophy:</b> Magic Attack +1.",
                           Modify = card =>
                                        {
                                            card.MagicAttack = 1;

                                            card.AttackCondition = player => player.Hand.Any(x => x.IsHero() && x.Level >= 1);
                                        }
                       };
        }

        private static MonsterDef Necrophidius()
        {
            return new MonsterDef
                       {
                           Name = "Necrophidius",
                           Count = 2,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 2,
                           Text = "<small>Cannot be fought unless 2 or more heroes are present."
                                  + "<br/><br/>"
                                  + "<b>Trophy:</b> You may destroy this card to level up a Regular or a level 1 hero without paying XP.</small>",
                           Modify = card =>
                                        {
                                            card.AttackCondition = player => player.Hand.Count(x => x.IsHero()) >= 2;

                                            Func<Card, bool> validCardFilter = c => c.IsHero() && c.Level <= 1 && c.Player.AvailableLevelUps(c).Any();
                                            card.CreateAbility()
                                                .Description("Destroy Necrophidius to level up a Regular or level 1 hero without paying XP.")
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
                                                                         x.Player.DestroyCard(card, card.Name);
                                                                         x.Source.Discard(new[] { upgrade });
                                                                         x.Player.DestroyCard(hero, "Upgrading to {0}".Template(upgrade.Name));
                                                                     })
//                                                .Condition(player => player.Hand.Any(validCardFilter))
                                                .Required(false)
                                                .On(Phase.Trophy);
                                        }
                       };
        }

        private static MonsterDef ShroudedCadaver()
        {
            return new MonsterDef
                       {
                           Name = "Shrouded Cadaver",
                           Count = 2,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 3,
                           Text = "<small><b>Battle:</b> Gain 2 curses. Each other player gains 1 curse."
                                  + "<br/><br/>"
                                  + "<b>Trophy:</b> You may discard this card to draw a card.",
                           Modify = card => card.CreateAbility()
                                                .Description("Gain 2 curses. Each other player gains 1 curse.")
                                                .Action(x =>
                                                            {
                                                                x.Player.GainCurse(2);
                                                                x.Game.Players.Where(p => p != x.Player).Each(p => p.GainCurse());
                                                            })
                                                .On(Phase.Battle)
                                                .CreateAbility()
                                                .Description("You may discard this card to draw a card.")
                                                .Action(x =>
                                                            {
                                                                x.Player.DiscardCard(card);
                                                                x.Player.Draw(1);
                                                            })
                                                .Required(false)
                                                .On(Phase.Trophy)
                       };
        }


    }
}