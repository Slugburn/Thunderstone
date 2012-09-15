using Newtonsoft.Json;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class UseAbilityHandler : MessageHandlerBase
    {
        public UseAbilityHandler() : base("UseAbility")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            var body = JsonConvert.DeserializeObject<UseAbilityResponse>(message.Body);
            if (body.AbilityId==null)
            {
                player.State.ContinueWith(player);
                return;
            }

            player.UseAbility(body.AbilityId.Value);
        }

        public class UseAbilityResponse
        {
            public Phase Phase { get; set; }
            public long? AbilityId { get; set; }
        }
    }

}
