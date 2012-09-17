using System;
using System.Linq;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class GameSetupHandler : MessageHandlerBase
    {
        public GameSetupHandler() : base("GameSetup")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            Do(player);
        }

        public static void Do(Player player)
        {
            var setup = new GameSetup();
            player.Session.Setup = setup;
            player.Session.SendAll(x => x.View.GameSetup(GameSetupModel.From(setup)));
        }
    }
}
