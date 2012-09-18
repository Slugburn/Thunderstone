using Newtonsoft.Json;
using Slugburn.Thunderstone.Lib.Messages;

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
            Do(player, body);
        }

        public static void Do(Player player, UseAbilityResponse body)
        {
            if (body.AbilityId == null)
            {
                player.State.ContinueWith(player);
                return;
            }

            player.UseAbility(body.AbilityId.Value);
        }
    }
}
