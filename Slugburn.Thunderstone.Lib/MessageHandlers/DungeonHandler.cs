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
            player.State = PlayerState.Dungeon;
            player.UseAbilities();
        }
    }
}