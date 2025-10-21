namespace BruteGamingMacros.Core.Utils
{
    public static class StatusUtils
    {
        private const uint INVALID_STATUS = 0xFFFFFFFF;

        public static bool IsValidStatus(uint statusId)
        {
            return statusId != INVALID_STATUS;
        }
    }
}