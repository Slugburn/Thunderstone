using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.Test
{
    public class TestContext
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public TestContext()
        {
            var game = TestFactory.CreateGame();
            Set(game);
            Set(game.CurrentPlayer);
            Player.View.Stub(x => x.SelectCards(null))
                .IgnoreArguments()
                .WhenCalled(inv =>
                    {
                        var message = (SelectCardsMessage) inv.Arguments[0];
                        Set(message);
                        var selectCardBehavior = Get<Func<SelectCardsMessage, IEnumerable<long>>>();
                        if (selectCardBehavior != null)
                            Player.SelectCardsCallback(selectCardBehavior(message));
                    });
            Player.View.Stub(x => x.SelectOption(null))
                .IgnoreArguments()
                .WhenCalled(inv =>
                                {
                                    var message = (SelectOptionMessage) inv.Arguments[0];
                                    Set(message);
                                    var selectOptionBehavior = Get<Func<SelectOptionMessage, string>>();
                                    if (selectOptionBehavior != null)
                                        Player.SelectOptionCallback(selectOptionBehavior(message));
                                });
            Player.View.Stub(x => x.StartTurn(null))
                .IgnoreArguments()
                .WhenCalled(inv => Set((StartTurnMessage) inv.Arguments[0]));
            // Fill up the dungeon hall
            while (game.Dungeon.Ranks[0].Card == null)
                game.AdvanceDungeon();
        }

        public void Set<T>(T value, string key = null)
        {
            _values[GetKey<T>(key)] = value;
        }

        public T Get<T>(string key = null)
        {
            object stored;
            return (T) (_values.TryGetValue(GetKey<T>(key), out stored) ? stored : null);
        }

        private static string GetKey<T>(string key)
        {
            key = key ?? typeof (T).FullName;
            if (key==null)
                throw new InvalidOperationException("Unable to determine key.");
            return key;
        }

        public Game Game { get { return Get<Game>(); } }

        public Player Player { get { return Get<Player>(); } }

        public IEnumerable<long> SelectCardsIds { get { return Get<SelectCardsMessage>().Cards.Select(x=>x.Id); } }

    }
}
