using System;
using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib
{
    public static class UtilExtensions
    {
        public static string Template(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var e in enumerable)
                action(e);
        }
    }
}
