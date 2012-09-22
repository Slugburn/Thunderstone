using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public class CurseOfHostility : CurseRandomizer
    {
        public CurseOfHostility() : base("Hostility")
        {
        }

        protected override string GetAdditionalText()
        {
            return "<b>Village/Dungeon:</b> Discard 2 XP to destroy this curse.";
        }

        protected override void Modify(Card card)
        {
            card.CreateAbility()
                .Description("Discard 2 XP to destroy this curse.")
                .Action(x =>
                {
                    x.Player.DestroyCard(card, card.Name);
                    x.Player.Xp -= 2;
                })
                .Condition(player => player.Xp >= 2)
                .On(Phase.Village, Phase.Dungeon);
        }
    }
}