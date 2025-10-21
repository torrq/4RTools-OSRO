using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

public static class StatusIdLogger
{
    private static Dictionary<uint, string> knownStatusIds = null;
    private static string lastStatusesLog = null;

    static StatusIdLogger()
    {
        InitializeStatusDictionaries();
    }

    private static void InitializeStatusDictionaries()
    {
        if (knownStatusIds != null)
            return;

        knownStatusIds = new Dictionary<uint, string>();

        foreach (EffectStatusIDs statusId in Enum.GetValues(typeof(EffectStatusIDs)))
        {
            uint id = (uint)statusId;
            string name = Enum.GetName(typeof(EffectStatusIDs), statusId);
            knownStatusIds[id] = name;
        }
    }

    public static void LogAllStatuses(IEnumerable<(int index, uint statusId)> statuses)
    {
        if (knownStatusIds == null)
            InitializeStatusDictionaries();

        var unknownStatuses = new List<uint>();
        var knownStatuses = new List<uint>();

        foreach (var (index, statusId) in statuses)
        {
            if (statusId != uint.MaxValue)
            {
                if (knownStatusIds.ContainsKey(statusId))
                {
                    knownStatuses.Add(statusId);
                }
                else
                {
                    unknownStatuses.Add(statusId);
                }
            }
        }

        if (unknownStatuses.Any() || knownStatuses.Any())
        {
            var unknownLog = unknownStatuses.OrderBy(id => id).Select(id => $"{id}:**UNKNOWN**");
            var knownLog = knownStatuses.OrderBy(id => id).Select(id => $"{id}:{knownStatusIds[id]}");
            var allStatuses = unknownLog.Concat(knownLog);
            string currentLog = $"{string.Join(" ", allStatuses)}";

            if (currentLog != lastStatusesLog)
            {
                DebugLogger.Status(currentLog);
                lastStatusesLog = currentLog;
            }
        }
    }
}