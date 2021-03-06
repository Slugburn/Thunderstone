﻿using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;
using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.BasicCards
{
    public class ThunderstoneShard : ICardGen
    {
        #region ICardGen Members

        public Card Create(Game game)
        {
            var card = new Card(game)
                           {
                               Type = CardType.Item,
                               Name = "Thunderstone Shard",
                               Gold = 1,
                               Cost = 0,
                               Text = "<b>Dungeon:</b> One hero gains Strength +2."
                                      + "<br/><br/>"
                                      + "<b>Spoils:</b> Gain 1 XP.",
                               Vp = 1
                           };
            card.SetTags("Item", "Thunderstone");
            card.CreateAbility()
                .Description("One hero gains Strength +2")
                .SelectCards(x => x.Select().FromHand().Filter(c => c.IsHero()).Caption("Select Hero").Message("Select hero to use Thunderstone Shard"))
                .OnCardsSelected(x => x.Selected.First().AddModifier(new PlusMod(card, Attr.Strength, 2)))
//                .Condition(player => player.Hand.Any(c => c.IsHero()))
                .On(Phase.Dungeon);
            card.CreateAbility()
                .Description("Gain 1 XP")
                .Action(x => x.Player.Xp++)
                .On(Phase.Spoils);
            return card;
        }

        #endregion
    }
}