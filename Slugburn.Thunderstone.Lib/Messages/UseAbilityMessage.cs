using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class UseAbilityMessage
    {
        public string Phase { get; set; }

        public bool Required { get; set; }

        public IEnumerable<AbilityModel> Abilities { get; set; }

        public static UseAbilityMessage Create(string phaseTag, bool required, IEnumerable<Ability> phaseAbilities)
        {
            var message = new UseAbilityMessage
                {
                    Phase = phaseTag,
                    Required = required,
                    Abilities = phaseAbilities.Select(a => a.CreateMessage())
                };
            return message;
        }
    }
}