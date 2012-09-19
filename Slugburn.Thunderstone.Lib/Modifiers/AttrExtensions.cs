using System.Linq;

namespace Slugburn.Thunderstone.Lib.Modifiers
{
    public static class AttrExtensions
    {
        public static int? ApplyModifiers(this IAttrSource target, Attr attr)
        {
            var applicable = target.GetModifiersFor(attr);
            var baseValue = target.GetBaseValue(attr);
            if (applicable.Length == 0)
                return baseValue;

            var modified = applicable.Aggregate(baseValue ?? 0, (i, mod) => mod.Modify(target, i));
            return (modified == 0 && baseValue == null) ? (int?) null : modified;
        }
    }
}
