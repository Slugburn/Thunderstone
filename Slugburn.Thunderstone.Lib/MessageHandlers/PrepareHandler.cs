using Slugburn.Thunderstone.Lib.Selectors;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class PrepareHandler : MessageHandlerBase
    {
        public PrepareHandler() : base("Prepare")
        {
        }

        public override void Handle(Message message)
        {
            var player = message.Player;
            player
                .SelectCard()
                .FromHand()
                .Min(0)
                .Max(player.Hand.Count)
                .Caption("Prepare")
                .Message("Select cards to place on top of your deck.")
                .Callback(x =>
                              {
                               x.Player.RemoveFromHand(x.Selected);
                               x.Player.AddToTopOfDeck(x.Selected);
                              } )
                .SendRequest(x => x.Player.EndTurn());
        }
    }
}