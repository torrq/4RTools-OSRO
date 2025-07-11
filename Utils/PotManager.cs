using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4RTools.Utils
{
    public static class PotManager
    {
        private static readonly object _lock = new object();
        private static DateTime _lastPotTime = DateTime.MinValue;
        private static readonly TimeSpan _cooldown = TimeSpan.FromMilliseconds(5); // ms gap between any pot usage

        /// <summary>
        /// Checks if enough time has passed since the last pot was used.
        /// </summary>
        public static bool CanUsePot()
        {
            lock (_lock)
            {
                return DateTime.UtcNow - _lastPotTime > _cooldown;
            }
        }

        /// <summary>
        /// Records the timestamp of a pot usage.
        /// </summary>
        public static void RecordPotUsage()
        {
            lock (_lock)
            {
                _lastPotTime = DateTime.UtcNow;
            }
        }
    }
}