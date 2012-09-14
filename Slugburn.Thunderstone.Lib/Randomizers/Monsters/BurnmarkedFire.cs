using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.Randomizers.Monsters
{
    public class BurnmarkedFire : MonsterRandomizer
    {
        public BurnmarkedFire() : base(2, "Burnmarked", "Fire")
        {
            Text = "Burnmarked have Battle effects that often affect all players. Use the curses deck.";
        }

        protected override IEnumerable<MonsterDef> MonsterDefs
        {
            get
            {
                return new[]
                           {
                               BlazingGrimalkin(),
                               CharredBruin(),
                               Phoenix(),
                               Hellhound(),
                               PyreViper()
                           };
            }
        }

        private static MonsterDef PyreViper()
        {
            return new MonsterDef
                       {
                           Name = "Pyre Viper",
                           Count = 2,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 3,
                           Text = "<b>Battle:</b> Each player must either show a curse or gain 2 curses.",
                           Modify = card => card.CreateAbility()
                                                .Description("Each player must either show a curse or gain 2 curses")
                                                .Action(player =>
                                                        player.Game.Players.Each(p =>
                                                                                     {
                                                                                         if (p.Hand.All(x => x.Type != CardType.Curse))
                                                                                             p.GainCurse(2);
                                                                                     }))
                                                .On(Phase.Battle)
                       };
        }

        private static MonsterDef Hellhound()
        {
            return new MonsterDef
                       {
                           Name = "Hellhound",
                           Count = 2,
                           Health = 7,
                           Gold = 2,
                           Xp = 2,
                           Vp = 4,
                           Text = "<b>Battle:</b> Gain 1 curse. Each other player must discard 1 hero, or show a hand with none."
                                  + "<br/><br/>"
                                  + "<b>Aftermath:</b> If you did not defeat this card, gain 1 curse.",
                           Modify = card =>
                                    card.CreateAbility()
                                        .Description("Gain 1 curse. Each other player must discard 1 hero, or show a hand with none.")
                                        .Action(player=>player.GainCurse())
                                        .On(Phase.Battle)
                                        // TODO: Complete implementation of battle ability
                                        .Description("Gain 1 curse")
                                        .Action(player => player.GainCurse())
                                        .Condition(player => !player.Won)
                                        .On(Phase.Aftermath)
                       };
        }

        private static MonsterDef Phoenix()
        {
            return new MonsterDef
                       {
                           Name = "Phoenix",
                           Count = 2,
                           Health = 6,
                           Gold = 2,
                           Xp = 2,
                           Vp = 3,
                           Text = "<b>Battle:</b> Gain a curse."
                                  + "<br/><br/>"
                                  + "<b>Trophy:</b> Gain 3 XP. Place Phoenix on top of the dungeon deck.",
                           Modify = card =>
                                    card.CreateAbility()
                                        .Description("Gain a curse")
                                        .Action(player => player.GainCurse())
                                        .On(Phase.Battle)
                                        .Description("Gain 3 XP. Place Phoenix on top of the dungeon deck.")
                                        .Action(player =>
                                                    {
                                                        player.Xp += 3;
                                                        player.RemoveFromHand(card);
                                                        player.Game.Dungeon.Deck.AddToTop(card);
                                                    })
                                        .On(Phase.Village, Phase.Dungeon)
                       };
        }

        private static MonsterDef CharredBruin()
        {
            return new MonsterDef
                       {
                           Name = "Charred Bruin",
                           Count = 2,
                           Health = 9,
                           Gold = 2,
                           Xp = 2,
                           Vp = 4,
                           Text = "<b>Battle:</b> Each player destroys 1 card.",
                           // TODO: Complete implementation of ability
                           Modify = card =>
                                    card.CreateAbility()
                                        .DestroyCard("Each player destroys 1 card.")
                                        .On(Phase.Battle)
                       };
        }

        private static MonsterDef BlazingGrimalkin()
        {
            return new MonsterDef
                       {
                           Name = "Blazing Grimalkin",
                           Count = 2,
                           Health = 8,
                           Gold = 2,
                           Xp = 3,
                           Vp = 3,
                           Text =
                               "<b>Battle:</b> Each player may choose to discard 3 cards. If a player chooses not to, that player loses 1 XP.",
                           Modify = card =>
                                    card.CreateAbility()
                                        // TODO: Complete implementation of ability
                                        .Description("Each player must discard 3 cards or lose 1 XP")
                                        .SelectCards(source => source.FromHand().Caption("Discard Cards").Message("Discard 3 cards or lose 1 XP").Min(0).Max(3))
                                        .OnCardsSelected(x =>
                                                             {
                                                                 if (x.Selected.Count == 3)
                                                                     x.Source.Discard(x.Selected);
                                                                 else
                                                                     x.Player.Xp = Math.Max(0, x.Player.Xp - 1);
                                                             })
                                        .On(Phase.Battle)
                       };
        }
    }
}
