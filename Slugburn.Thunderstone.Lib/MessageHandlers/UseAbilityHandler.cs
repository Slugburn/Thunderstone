using System;
using System.Linq;
using Newtonsoft.Json;
using Slugburn.Thunderstone.Lib.Events;

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
                NextPhase(player, body.Phase);
                return;
            }

            player.UseAbility(body.AbilityId.Value);
        }

        private static void NextPhase(Player player, Phase phase)
        {
            switch (phase)
            {
                case Phase.Village:
                    player.BuyCard();
                    break;
                case Phase.Dungeon:
                    player.SelectMonster();
                    break;
                case Phase.Battle:
                    break;
                case Phase.Spoils:
                    player.RefillHall();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public class UseAbilityResponse
        {
            public Phase Phase { get; set; }
            public long? AbilityId { get; set; }
        }
    }

}
