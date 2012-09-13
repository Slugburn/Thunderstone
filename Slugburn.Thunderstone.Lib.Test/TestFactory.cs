using System;
using Rhino.Mocks;

namespace Slugburn.Thunderstone.Lib.Test
{
    public static class TestFactory
    {
        public static Game CreateGame()
        {
            var session = new GameSession {Setup = new GameSetup()};
            var player = new Player(Guid.NewGuid(), MockRepository.GenerateStub<IPlayerView>());
            session.Join(player);
            var game = new Game();
            game.Initialize(session);
            return game;
        }
    }
}
