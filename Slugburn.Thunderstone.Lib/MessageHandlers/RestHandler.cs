using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class RestHandler : MessageHandlerBase
    {
        public RestHandler() : base("Rest")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            player
                .SelectCard()
                .FromHand()
                .Caption("Rest")
                .Destroy()
                .SendRequest(p=>p.EndTurn());
        }
    }
}
