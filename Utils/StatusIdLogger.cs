using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Reflection;
using System.ComponentModel;
using _4RTools.Utils;

public static class StatusIdLogger
{
    private static Dictionary<uint, string> knownStatusIds = null;
    private static readonly HashSet<uint> reportedNewStatusIds = new HashSet<uint>();
    private static readonly HashSet<uint> currentKnownStatuses = new HashSet<uint>();
    private static readonly HashSet<uint> currentUnknownStatuses = new HashSet<uint>();
    private static readonly Timer logTimer;
    private static readonly int logIntervalMs = 1000; // 1 second interval

    static StatusIdLogger()
    {
        InitializeStatusDictionaries();
        logTimer = new Timer(logIntervalMs);
        logTimer.Elapsed += LogStatuses;
        logTimer.AutoReset = true;
        logTimer.Start();
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

    public static void LogStatusId(int index, uint statusId)
    {
        if (knownStatusIds == null)
            InitializeStatusDictionaries();

        if (knownStatusIds.TryGetValue(statusId, out string statusName))
        {
            currentKnownStatuses.Add(statusId);
        }
        else
        {
            if (!reportedNewStatusIds.Contains(statusId))
            {
                reportedNewStatusIds.Add(statusId);
            }
            currentUnknownStatuses.Add(statusId);
        }
    }

    private static string lastKnownStatusesLog = null;
    private static string lastUnknownStatusesLog = null;

    private static void LogStatuses(object sender, ElapsedEventArgs e)
    {
        if (currentKnownStatuses.Any())
        {
            // Sort the known statuses by ID number before joining them
            string currentLog = $"Known Statuses: {string.Join(" ", currentKnownStatuses.OrderBy(id => id).Select(id => $"{id}:{knownStatusIds[id]}"))}";

            if (currentLog != lastKnownStatusesLog)
            {
                DebugLogger.Info(currentLog);
                lastKnownStatusesLog = currentLog;
            }
        }

        if (currentUnknownStatuses.Any())
        {
            // Sort the unknown statuses numerically as well
            string currentLog = $"Unknown Statuses: {string.Join(", ", currentUnknownStatuses.OrderBy(id => id))}";

            if (currentLog != lastUnknownStatusesLog)
            {
                DebugLogger.Info(currentLog);
                lastUnknownStatusesLog = currentLog;
            }
        }

        currentKnownStatuses.Clear();
        currentUnknownStatuses.Clear();
    }

}
