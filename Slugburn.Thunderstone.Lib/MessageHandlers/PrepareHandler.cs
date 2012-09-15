using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class PrepareHandler : MessageHandlerBase
    {
        public PrepareHandler() : base("Prepare")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            DoPrepare(player);
        }

        public static void DoPrepare(Player player)
        {
            player.State = PlayerState.Prepare;
            player.Prepare();
        }
    }
}