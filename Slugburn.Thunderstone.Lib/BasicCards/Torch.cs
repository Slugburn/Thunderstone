namespace Slugburn.Thunderstone.Lib.BasicCards
{
    internal class Torch : ICardGen
    {
        #region ICardGen Members

        public Card Create(Game game)
        {
            var card = new Card(game)
                           {
                               Type = CardType.Item,
                               Name = "Torch",
                               Gold = 2,
                               Light = 1,
                               Cost = 3
                           };
            card.SetTags("Item", "Light", "Basic");
            return card;
        }

        #endregion
    }
}