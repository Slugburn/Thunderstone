using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Randomizers.Curses
{
    public abstract class CurseRandomizer : RandomizerBase
    {
        protected CurseRandomizer(string name)
            : base(CardType.Curse, "Curse of {0}".Template(name), new[] { "Curse", "Disease" })
        {
        }

        public override IEnumerable<Card> CreateCards(Game game)
        {
            return Enumerable.Range(0, 4)
                .Select(x =>
                    {
                        var card = new Card(game)
                            {
                                Name = Name,
                                PhysicalAttack = -1,
                                Text = "<b>Attack -1</b><br/><br/>" + GetAdditionalText(),
                                Owner = CardOwner.Game,
                                Type = CardType.Curse
                            };
                        card.SetTags(Tags);
                        Modify(card);
                        return card;
                    });
        }

        protected abstract string GetAdditionalText();

        protected abstract void Modify(Card card);
    }
}
