using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Randomizers;

namespace Slugburn.Thunderstone.Lib.Test
{
    public static class TestContextExtensions
    {
        public static Card GivenMonsterInFirstRank<TRandomizer>(this TestContext context, string name) where TRandomizer : IRandomizer
        {
            var monster = CreateCard<TRandomizer>(context, name);
            var game = context.Game;
            game.Dungeon.AddToTopOfDeck(monster);
            while (game.Dungeon.Ranks[0].Card == null)
                game.AdvanceDungeon();
            return monster;
        }

        public static Card CreateCard<TRandomizer>(this TestContext context, string name) where TRandomizer : IRandomizer
        {
            var randomizer = Activator.CreateInstance<TRandomizer>();
            return randomizer.CreateCards().First(x => x.Name == name);
        }

        public static void GivenSelectCardsMessageExpected(this TestContext context)
        {
            context.Player.View.Stub(x => x.SelectCards(null)).IgnoreArguments().WhenCalled(inv => context.Set((SelectCardsMessage) inv.Arguments[0]));
        }

        public static Card GivenHeroFromTopOfDeck(this TestContext context, Func<Card, bool> filter)
        {
            return context.Game.Village[CardType.Hero].First(x => filter(x.TopCard)).Draw();
        }

        public static Card GivenHeroFromVillage(this TestContext context, Func<Card, bool> filter)
        {
            var deck = context.Game.Village[CardType.Hero].First(x => x.GetCards().Any(filter));
            var card = deck.GetCards().First(filter);
            deck.Remove(card);
            return card;
        }

        public static void GivenPlayerHand(this TestContext context, params Card[] cards)
        {
            context.Player.DiscardHand();
            context.Player.AddCardsToHand(cards);
        }

        public static void GivenPlayerState(this TestContext context, PlayerState state)
        {
            context.Player.State = state;
        }
    }
}
