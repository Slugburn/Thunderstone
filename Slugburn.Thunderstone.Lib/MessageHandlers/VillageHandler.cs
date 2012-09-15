namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class VillageHandler : MessageHandlerBase
    {
        public VillageHandler() : base("Village")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            player.State = PlayerState.Village;
            player.UseAbilities();
        }
    }
}
