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
            var player = message.Player;
            Do(player);
        }

        public static void Do(Player player)
        {
            var game = new Game();
            game.Initialize(player.Session);
            game.Players.Each(p => p.View.GameBoard(GameBoardModel.From(p)));
            game.CurrentPlayer.StartTurn();
        }
    }
}
