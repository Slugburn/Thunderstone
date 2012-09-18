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
            Do(player);
        }

        public static void Do(Player player)
        {
            player.State = PlayerState.Prepare;
            player.Prepare();
        }
    }
}