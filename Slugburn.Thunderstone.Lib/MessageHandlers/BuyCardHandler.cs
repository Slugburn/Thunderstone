namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class BuyCardHandler : MessageHandlerBase
    {
        public BuyCardHandler() : base("BuyCard")
        {
        }

        public override void Handle(Message message)
        {
            var deckId = long.Parse(message.Body);
            var player = message.Player;
            Do(player, deckId);
        }

        public static void Do(Player player, long deckId)
        {
            var card = player.Game.BuyCard(deckId);
            player.AddToDiscard(card);
            player.LevelHeroes();
        }
    }
}
