using System.Linq;
using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Selectors;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Randomizers.Items
{
    class KingCaelansWrit : ItemRandomizer
    {
        public KingCaelansWrit() : base("King Caelan's Writ")
        {
            Cost = 5;
            Text = "<b>Village:</b> Destroy this card to place the top card of any hero stack on top of your deck. End your turn.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Destroy this card to place the top card of any hero stack on top of your deck. End your turn.")
                .SelectCards(select => select.FromTopOfHeroDecks().Caption(card.Name).Message("Select one hero card to add to your deck"))
                .OnCardsSelected(x =>
                                     {
                                         var hero = x.Selected.First();
                                         x.Player.AddToTopOfDeck(hero);
                                         x.Player.DestroyCard(card, card.Name);
                                     })
//                .Condition(player => new TopOfDeckSource(player, CardType.Hero).GetCards().Any())
                .ContinueWith(player => player.EndTurn())
                .On(Phase.Village);

        }
    }
}
