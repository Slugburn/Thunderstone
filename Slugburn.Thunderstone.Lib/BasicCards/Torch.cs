namespace Slugburn.Thunderstone.Lib.BasicCards
{
    internal class Torch : ICardGen
    {
        #region ICardGen Members

        public Card Create()
        {
            var card = new Card
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