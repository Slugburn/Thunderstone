using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfNeglect : CurseRandomizer
    {
        public CurseOfNeglect() : base("Neglect")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village:</b> Lose 2 gold to destroy this curse.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Lose 2 gold to destroy this curse.")
                .Action(x =>
                    {
                        x.Player.AddModifier(new PlusMod(card, Attr.Gold, -2));
                        x.Player.DestroyCard(card, card.Name);
                    })
                .On(Phase.Village);
        }
    }
}