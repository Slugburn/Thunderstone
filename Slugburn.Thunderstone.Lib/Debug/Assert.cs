using System;

namespace Slugburn.Thunderstone.Lib.Debug
{
    public static class Assert
    {
        public static void That(bool condition, string message)
        {
            if (!condition)
                throw new Exception(message);
        }
    }
}
