using Slugburn.Thunderstone.Lib.Abilities;
using Slugburn.Thunderstone.Lib.Modifiers;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfHorror : CurseRandomizer
    {
        public CurseOfHorror() : base("Horror")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<small><b>Dungeon:</b> Destroy this curse. Reduce your Light to 0. "
                   + "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.</small>";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Destroy this curse. Reduce your Light to 0. "
                             + "You cannot equip any more weapons, use any other Dungeon abilities, or use any other Trophy effects this turn.")
                .Action(x =>
                    {
                        x.Player.AddModifier(new SetMod(card, Attr.Light, 0));
                        x.Player.ActiveAbilities.RemoveAll(a => a.Phase == Phase.Equip || a.Phase == Phase.Dungeon || a.Phase == Phase.Trophy);
                        x.Player.DestroyCard(card, card.Name);
                    })
                .On(Phase.Dungeon);
        }
    }
}