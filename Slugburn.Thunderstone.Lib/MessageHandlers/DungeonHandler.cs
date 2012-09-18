namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class DungeonHandler : MessageHandlerBase
    {
        public DungeonHandler() : base("Dungeon")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            Do(player);
        }

        public static void Do(Player player)
        {
            player.State = PlayerState.Dungeon;
            player.UseAbilities();
        }
    }
}