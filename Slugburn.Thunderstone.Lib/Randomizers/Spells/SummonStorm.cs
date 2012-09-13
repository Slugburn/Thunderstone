namespace Slugburn.Thunderstone.Lib.Randomizers.Spells
{
    class SummonStorm :SpellRandomizer
    {
        public SummonStorm() : base("Summon Storm", "Attack", "Light")
        {
            Light = 1;
            Cost = 6;
            Text = "<b>Village:</b> Put this card on top of your deck.<br/><br/><b>Dungeon:</b> Magic Attack +2.";
        }

        protected override void Modify(Card card)
        {
            base.Modify(card);
            card.MagicAttack = 2;
            card.AddAbility(new Ability(Phase.Village, "Put this card on top of your deck.",
                player=>
                    {
                        player.RemoveFromHand(card);
                        player.AddToTopOfDeck(card);
                    }));
        }
    }
}
