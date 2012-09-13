using System.Threading;

namespace Slugburn.Thunderstone.Lib
{
    public static class UniqueId
    {
        private static long _id;

        public static long Next()
        {
            return Interlocked.Increment(ref _id);
        }
    }
}
