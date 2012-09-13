using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Thunderstone.Lib.Test
{
    public static class TestFactory
    {
        public static Game CreateGame()
        {
            var session = new GameSession {Setup = new GameSetup()};
            var player = new Player(Guid.NewGuid(), (s, o) => { });
            session.Join(player);
            var game = new Game();
            game.Initialize(session);
            return game;
        }
    }
}
