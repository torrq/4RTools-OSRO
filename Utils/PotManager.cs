using System;
using System.Threading;

namespace _4RTools.Utils
{
    public static class PotManager
    {
        private static long _lastPotTicks = 0;
        private static readonly long _cooldownTicks = TimeSpan.FromMilliseconds(10).Ticks; // Slightly increased for safety

        /// <summary>
        /// Checks if enough time has passed since the last pot was used.
        /// Uses high-resolution timing for maximum accuracy.
        /// </summary>
        public static bool CanUsePot()
        {
            long currentTicks = DateTime.UtcNow.Ticks;
            return currentTicks - Interlocked.Read(ref _lastPotTicks) >= _cooldownTicks;
        }

        /// <summary>
        /// Records the timestamp of a pot usage using atomic operations.
        /// </summary>
        public static void RecordPotUsage()
        {
            Interlocked.Exchange(ref _lastPotTicks, DateTime.UtcNow.Ticks);
        }
    }
}