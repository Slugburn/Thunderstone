namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class VillageHandler : MessageHandlerBase
    {
        public VillageHandler() : base("Village")
        {
        }

        public override void Handle(Message message)
        {
            message.Player.DoVillage();
        }
    }
}
