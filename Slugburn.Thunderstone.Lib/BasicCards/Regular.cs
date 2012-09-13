using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.BasicCards
{
    internal class Regular : ICardGen
    {
        public Card Create()
        {
            var card = new Card
                           {
                               Type = CardType.Hero,
                               Name = "Regular",
                               Gold = 0,
                               Strength = 3,
                               Cost = 0,
                               Text = "<b>Physical Attack +1</b>"
                                      + "<br/><br/>"
                                      + "<b>Dungeon:</b> If equipped with a polearm, draw a card.",
                               PhysicalAttack = 1,
                               Xp = 2,
                               Owner = CardOwner.Village
                           };
            card.SetTags("Level 0", "Basic");
            card.CreateAbility()
                .DrawCards(1)
                .Condition(player => card.IsEquipped && card.GetEquipped().HasTag("Polearm"))
                .On(Phase.Dungeon);

            return card;
        }
    }
}