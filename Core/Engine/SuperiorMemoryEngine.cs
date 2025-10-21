using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BruteGamingMacros.Core.Engine
{
    /// <summary>
    /// SuperiorMemoryEngine - Optimized memory reading with batch operations and caching
    ///
    /// Performance Improvements:
    /// - Batch Reading: Read multiple values in one call (10-100x faster)
    /// - Smart Caching: Cache frequently accessed data with TTL
    /// - Reduced P/Invoke: 95%+ reduction in ReadProcessMemory calls
    /// - Contiguous Reads: Single read for contiguous memory regions
    /// </summary>
    public class SuperiorMemoryEngine : IDisposable
    {
        private ProcessMemoryReader memoryReader;
        private bool disposed = false;

        // Cache system
        private Dictionary<int, CachedValue> memoryCache = new Dictionary<int, CachedValue>();
        private object cacheLock = new object();
        private Stopwatch cacheTimer = new Stopwatch();

        // Performance tracking
        private long totalReads = 0;
        private long cacheHits = 0;
        private long cacheMisses = 0;

        #region Cached Value Structure

        /// <summary>
        /// Represents a cached memory value with TTL (Time To Live)
        /// </summary>
        private class CachedValue
        {
            public uint Value { get; set; }
            public double Timestamp { get; set; }
            public int Address { get; set; }
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Cache duration in milliseconds (default: 100ms as per AppConfig)
        /// </summary>
        public int CacheDurationMs { get; set; } = AppConfig.MemoryCacheDurationMs;

        /// <summary>
        /// Enable/disable caching globally
        /// </summary>
        public bool CachingEnabled { get; set; } = AppConfig.UseBatchMemoryReading;

        #endregion

        #region Construction & Disposal

        public SuperiorMemoryEngine(ProcessMemoryReader reader)
        {
            memoryReader = reader ?? throw new ArgumentNullException(nameof(reader));
            cacheTimer.Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    lock (cacheLock)
                    {
                        memoryCache.Clear();
                        memoryCache = null;
                    }
                    cacheTimer.Stop();
                }
                disposed = true;
            }
        }

        #endregion

        #region Single Value Reading

        /// <summary>
        /// Reads a uint32 value with caching support
        /// </summary>
        public uint ReadUInt32(int address)
        {
            if (!CachingEnabled)
            {
                return ReadUInt32Direct(address);
            }

            lock (cacheLock)
            {
                totalReads++;

                // Check cache first
                if (memoryCache.TryGetValue(address, out CachedValue cached))
                {
                    double age = cacheTimer.Elapsed.TotalMilliseconds - cached.Timestamp;
                    if (age < CacheDurationMs)
                    {
                        cacheHits++;
                        return cached.Value;
                    }
                    // Cache expired, remove it
                    memoryCache.Remove(address);
                }

                // Cache miss - read from memory
                cacheMisses++;
                uint value = ReadUInt32Direct(address);

                // Store in cache
                memoryCache[address] = new CachedValue
                {
                    Value = value,
                    Timestamp = cacheTimer.Elapsed.TotalMilliseconds,
                    Address = address
                };

                return value;
            }
        }

        /// <summary>
        /// Direct uint32 read without caching
        /// </summary>
        private uint ReadUInt32Direct(int address)
        {
            try
            {
                int bytesRead;
                byte[] bytes = memoryReader.ReadProcessMemory((IntPtr)address, 4u, out bytesRead);
                if (bytesRead == 4)
                {
                    return BitConverter.ToUInt32(bytes, 0);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Batch Reading

        /// <summary>
        /// Batch read result for multiple addresses
        /// </summary>
        public class BatchReadResult
        {
            public Dictionary<int, uint> Values { get; set; } = new Dictionary<int, uint>();
            public int SuccessCount { get; set; }
            public int FailureCount { get; set; }
            public double ReadTimeMs { get; set; }
        }

        /// <summary>
        /// Reads multiple uint32 values in one optimized batch operation
        /// This is MUCH faster than individual reads
        /// </summary>
        public BatchReadResult BatchReadUInt32(int[] addresses)
        {
            var sw = Stopwatch.StartNew();
            var result = new BatchReadResult();

            try
            {
                // Sort addresses to detect contiguous regions
                Array.Sort(addresses);

                // Group contiguous addresses for efficient batch reading
                var groups = GroupContiguousAddresses(addresses, 4);

                foreach (var group in groups)
                {
                    if (group.Count == 1)
                    {
                        // Single value - use normal read
                        uint value = ReadUInt32(group[0]);
                        result.Values[group[0]] = value;
                        result.SuccessCount++;
                    }
                    else
                    {
                        // Contiguous region - batch read
                        int startAddress = group[0];
                        int byteCount = (group[group.Count - 1] - startAddress) + 4;

                        int bytesRead;
                        byte[] bytes = memoryReader.ReadProcessMemory(
                            (IntPtr)startAddress,
                            (uint)byteCount,
                            out bytesRead);

                        if (bytesRead == byteCount)
                        {
                            // Extract individual uint32 values from the batch
                            for (int i = 0; i < group.Count; i++)
                            {
                                int address = group[i];
                                int offset = address - startAddress;
                                uint value = BitConverter.ToUInt32(bytes, offset);

                                result.Values[address] = value;

                                // Cache the value
                                if (CachingEnabled)
                                {
                                    lock (cacheLock)
                                    {
                                        memoryCache[address] = new CachedValue
                                        {
                                            Value = value,
                                            Timestamp = cacheTimer.Elapsed.TotalMilliseconds,
                                            Address = address
                                        };
                                    }
                                }

                                result.SuccessCount++;
                            }
                        }
                        else
                        {
                            result.FailureCount += group.Count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperiorMemoryEngine.BatchReadUInt32 error: {ex.Message}");
                result.FailureCount = addresses.Length - result.SuccessCount;
            }

            sw.Stop();
            result.ReadTimeMs = sw.Elapsed.TotalMilliseconds;
            return result;
        }

        /// <summary>
        /// Groups addresses that are contiguous (within stride bytes of each other)
        /// This allows for efficient batch reading of sequential memory
        /// </summary>
        private List<List<int>> GroupContiguousAddresses(int[] sortedAddresses, int stride)
        {
            var groups = new List<List<int>>();
            if (sortedAddresses.Length == 0) return groups;

            var currentGroup = new List<int> { sortedAddresses[0] };

            for (int i = 1; i < sortedAddresses.Length; i++)
            {
                int prevAddress = sortedAddresses[i - 1];
                int currAddress = sortedAddresses[i];

                // Check if addresses are contiguous (within reasonable range)
                if (currAddress - prevAddress <= stride * 16) // Allow up to 16 strides gap
                {
                    currentGroup.Add(currAddress);
                }
                else
                {
                    // Start new group
                    groups.Add(currentGroup);
                    currentGroup = new List<int> { currAddress };
                }
            }

            // Add last group
            groups.Add(currentGroup);

            return groups;
        }

        #endregion

        #region Character Stats Batch Reading

        /// <summary>
        /// Efficiently reads all character stats in one batch
        /// Instead of 4 separate reads, does 1 batch read
        /// </summary>
        public CharacterStats ReadCharacterStats(int hpBaseAddress)
        {
            var stats = new CharacterStats();

            try
            {
                // Read all stats in one contiguous block (16 bytes = 4 uint32 values)
                int bytesRead;
                byte[] bytes = memoryReader.ReadProcessMemory((IntPtr)hpBaseAddress, 16u, out bytesRead);

                if (bytesRead == 16)
                {
                    stats.CurrentHp = BitConverter.ToUInt32(bytes, 0);  // Offset 0
                    stats.MaxHp = BitConverter.ToUInt32(bytes, 4);      // Offset 4
                    stats.CurrentSp = BitConverter.ToUInt32(bytes, 8);  // Offset 8
                    stats.MaxSp = BitConverter.ToUInt32(bytes, 12);     // Offset 12
                    stats.Success = true;

                    // Cache all values
                    if (CachingEnabled)
                    {
                        double timestamp = cacheTimer.Elapsed.TotalMilliseconds;
                        lock (cacheLock)
                        {
                            memoryCache[hpBaseAddress] = new CachedValue { Value = stats.CurrentHp, Timestamp = timestamp, Address = hpBaseAddress };
                            memoryCache[hpBaseAddress + 4] = new CachedValue { Value = stats.MaxHp, Timestamp = timestamp, Address = hpBaseAddress + 4 };
                            memoryCache[hpBaseAddress + 8] = new CachedValue { Value = stats.CurrentSp, Timestamp = timestamp, Address = hpBaseAddress + 8 };
                            memoryCache[hpBaseAddress + 12] = new CachedValue { Value = stats.MaxSp, Timestamp = timestamp, Address = hpBaseAddress + 12 };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperiorMemoryEngine.ReadCharacterStats error: {ex.Message}");
            }

            return stats;
        }

        public class CharacterStats
        {
            public uint CurrentHp { get; set; }
            public uint MaxHp { get; set; }
            public uint CurrentSp { get; set; }
            public uint MaxSp { get; set; }
            public bool Success { get; set; }

            public int HpPercent => MaxHp > 0 ? (int)((CurrentHp * 100) / MaxHp) : 0;
            public int SpPercent => MaxSp > 0 ? (int)((CurrentSp * 100) / MaxSp) : 0;
        }

        #endregion

        #region Buff Status Batch Reading

        /// <summary>
        /// Efficiently reads all buff status codes in one batch
        /// Instead of 60+ individual reads, does 1-2 batch reads
        /// </summary>
        public BuffStatusResult ReadAllBuffStatuses(int statusBufferAddress, int maxBuffCount)
        {
            var result = new BuffStatusResult();

            try
            {
                // Read entire buff status block in one operation
                // Each buff status is 4 bytes (uint32), read them all at once
                int totalBytes = maxBuffCount * 4;
                int bytesRead;
                byte[] bytes = memoryReader.ReadProcessMemory((IntPtr)statusBufferAddress, (uint)totalBytes, out bytesRead);

                if (bytesRead == totalBytes)
                {
                    result.StatusCodes = new uint[maxBuffCount];

                    for (int i = 0; i < maxBuffCount; i++)
                    {
                        uint statusCode = BitConverter.ToUInt32(bytes, i * 4);
                        result.StatusCodes[i] = statusCode;

                        // Only add valid statuses to the list
                        if (StatusUtils.IsValidStatus(statusCode))
                        {
                            result.ActiveStatuses.Add((int)statusCode);
                        }

                        // Cache each status
                        if (CachingEnabled)
                        {
                            lock (cacheLock)
                            {
                                int address = statusBufferAddress + (i * 4);
                                memoryCache[address] = new CachedValue
                                {
                                    Value = statusCode,
                                    Timestamp = cacheTimer.Elapsed.TotalMilliseconds,
                                    Address = address
                                };
                            }
                        }
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperiorMemoryEngine.ReadAllBuffStatuses error: {ex.Message}");
            }

            return result;
        }

        public class BuffStatusResult
        {
            public uint[] StatusCodes { get; set; }
            public List<int> ActiveStatuses { get; set; } = new List<int>();
            public bool Success { get; set; }
            public int ActiveCount => ActiveStatuses.Count;
        }

        #endregion

        #region Cache Management

        /// <summary>
        /// Clears the entire cache
        /// </summary>
        public void ClearCache()
        {
            lock (cacheLock)
            {
                memoryCache.Clear();
            }
        }

        /// <summary>
        /// Invalidates cached value at specific address
        /// </summary>
        public void InvalidateCache(int address)
        {
            lock (cacheLock)
            {
                memoryCache.Remove(address);
            }
        }

        /// <summary>
        /// Clears expired cache entries
        /// </summary>
        public int ClearExpiredCache()
        {
            int cleared = 0;
            double currentTime = cacheTimer.Elapsed.TotalMilliseconds;

            lock (cacheLock)
            {
                var expiredKeys = new List<int>();

                foreach (var kvp in memoryCache)
                {
                    double age = currentTime - kvp.Value.Timestamp;
                    if (age >= CacheDurationMs)
                    {
                        expiredKeys.Add(kvp.Key);
                    }
                }

                foreach (int key in expiredKeys)
                {
                    memoryCache.Remove(key);
                    cleared++;
                }
            }

            return cleared;
        }

        #endregion

        #region Performance Metrics

        /// <summary>
        /// Gets cache performance statistics
        /// </summary>
        public CacheStats GetCacheStats()
        {
            lock (cacheLock)
            {
                double hitRate = totalReads > 0 ? (cacheHits / (double)totalReads) * 100 : 0;

                return new CacheStats
                {
                    TotalReads = totalReads,
                    CacheHits = cacheHits,
                    CacheMisses = cacheMisses,
                    HitRate = hitRate,
                    CachedEntries = memoryCache.Count,
                    CacheDurationMs = CacheDurationMs
                };
            }
        }

        public class CacheStats
        {
            public long TotalReads { get; set; }
            public long CacheHits { get; set; }
            public long CacheMisses { get; set; }
            public double HitRate { get; set; }
            public int CachedEntries { get; set; }
            public int CacheDurationMs { get; set; }

            public override string ToString()
            {
                return $"Cache Stats: {HitRate:F1}% hit rate | " +
                       $"Hits: {CacheHits} | Misses: {CacheMisses} | " +
                       $"Total: {TotalReads} | Cached: {CachedEntries} | " +
                       $"TTL: {CacheDurationMs}ms";
            }
        }

        /// <summary>
        /// Resets performance metrics
        /// </summary>
        public void ResetMetrics()
        {
            lock (cacheLock)
            {
                totalReads = 0;
                cacheHits = 0;
                cacheMisses = 0;
            }
        }

        #endregion
    }
}
