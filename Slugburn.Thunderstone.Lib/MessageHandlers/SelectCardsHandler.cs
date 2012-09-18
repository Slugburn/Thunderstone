using Newtonsoft.Json;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class SelectCardsHandler : MessageHandlerBase
    {
        public SelectCardsHandler() : base("SelectCards")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            var cardIds = JsonConvert.DeserializeObject<long[]>(message.Body);
            Do(player, cardIds);
        }

        public static void Do(Player player, long[] cardIds)
        {
            player.SelectCardsCallback(cardIds);
        }
    }
}
