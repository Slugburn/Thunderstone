using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public static class AttributeModExtensions
    {
        public static int? ApplyTo(this IEnumerable<IAttributeMod> mods, Attribute attribute, int? baseValue)
        {
            var applicable = mods.Where(x => x.Attribute == attribute).ToArray();
            if (applicable.Length == 0)
                return baseValue;

            var modified = applicable.Aggregate(baseValue ?? 0, (i, mod) => mod.Modify(i));
            return (modified == 0 && baseValue == null) ? (int?) null : modified;
        }
    }
}
