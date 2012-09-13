namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class DungeonHandler : MessageHandlerBase
    {
        public DungeonHandler() : base("Dungeon")
        {
        }

        public override void Handle(Message message)
        {
            message.Player.DoDungeon();
        }
    }
}