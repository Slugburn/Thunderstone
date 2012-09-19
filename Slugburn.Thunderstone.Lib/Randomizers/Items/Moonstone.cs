using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Items
{
    public class Moonstone : ItemRandomizer
    {
        public Moonstone() : base("Moonstone", "Light")
        {
            Gold = 2;
            Light = 2;
            Cost = 5;
            Text = "<b>Dungeon:</b> Draw 1 card.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.CreateAbility().DrawCards(1).On(Phase.Dungeon);
        }
    }
}
