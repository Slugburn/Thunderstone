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
            var cardIds = JsonConvert.DeserializeObject<long[]>(message.Body);
            message.Player.SelectCardsCallback(cardIds);
        }
    }
}
