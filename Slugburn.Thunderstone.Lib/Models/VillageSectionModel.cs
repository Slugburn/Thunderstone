using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Models
{
    public class VillageSectionModel
    {
        public string SectionName { get; set; }

        public IEnumerable<DeckModel> Decks { get; set; }

        public static VillageSectionModel Create(Game game, string sectionName, CardType type)
        {
            return new VillageSectionModel
                {
                    SectionName = sectionName,
                    Decks = game.Village[type].Select(DeckModel.From)
                };
        }
    }
}