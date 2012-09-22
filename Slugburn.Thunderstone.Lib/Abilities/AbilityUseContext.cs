using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Selectors;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Abilities
{
    public class AbilityUseContext
    {

        private ISelectSource _select;
        public Player Player { get; set; }
        public Ability Ability { get; set; }

        public List<ICardSource> Sources { get; private set; }

        public ICardSource Source
        {
            get { return Sources.Last(); }
        }

        public List<Card> Selected
        {
            get { return Selections.Last(); }
        }

        public ISelectionContext SelectionContext { get; set; }

        public List<List<Card>> Selections { get; private set; }

        public AbilityUseContext(Player player, Ability ability)
        {
            Player = player;
            Ability = ability;
            Sources = new List<ICardSource>();
            Selections = new List<List<Card>>();
        }

        public Game Game
        {
            get { return Player.Game; }
        }

        public ISelectSource Select()
        {
            return _select ?? (_select = Player.SelectCard(Ability));
        }

        public void ProcessSelection(ISelectionContext selectionContext, Action<AbilityUseContext> callback)
        {
            Sources.Add(selectionContext.Source);
            Selections.Add(selectionContext.Selected.ToList());
            callback(this);
        }
    }
}
