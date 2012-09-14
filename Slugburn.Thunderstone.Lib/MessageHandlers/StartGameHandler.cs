using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class StartGameHandler : MessageHandlerBase
    {
        public StartGameHandler() : base("StartGame")
        {
        }

        public override void Handle(Message message)
        {
            var game = new Game();
            game.Initialize(message.Player.Session);
            game.Players.Each(player=>player.View.GameBoard(GameBoardModel.From(player)));
            game.CurrentPlayer.StartTurn();
        }
    }
}
