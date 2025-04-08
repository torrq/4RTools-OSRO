using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace _4RTools.Utils
{
    public static class DebugLogger
    {
        private static readonly object _logLock = new object();
        private static readonly string _logFilePath = "4rtools_debug.txt";
        private static readonly System.Timers.Timer _flushTimer;
        private static readonly Queue<LogEntry> _messageQueue = new Queue<LogEntry>();
        private static bool _isInitialized = false;
        private static readonly int _flushIntervalMs = 1000; // Flush to file every second

        // Duplicate message tracking
        private static string _lastMessage = null;
        private static LogLevel _lastLogLevel = LogLevel.INFO;
        private static int _duplicateCount = 0;
        private static DateTime _lastMessageTime = DateTime.MinValue;

        // Log levels
        public enum LogLevel
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG
        }

        // Structure to hold log messages with potential repeat counts
        private class LogEntry
        {
            public string Message { get; set; }
            public LogLevel Level { get; set; }
            public DateTime Timestamp { get; set; }
            public int RepeatCount { get; set; }

            public string FormatMessage()
            {
                string baseMessage = $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}";
                if (RepeatCount > 0)
                {
                    return $"{baseMessage} (repeated {RepeatCount} times)";
                }
                return baseMessage;
            }
        }

        static DebugLogger()
        {
            try
            {
                // Create log file with header if it doesn't exist
                if (!File.Exists(_logFilePath))
                {
                    using (StreamWriter writer = new StreamWriter(_logFilePath, false, Encoding.UTF8))
                    {
                        writer.WriteLine($"=== 4RTOOLS DEBUG LOG === Started on {DateTime.Now} ===");
                        writer.WriteLine("============================================");
                    }
                }
                else
                {
                    // Add separation for new session
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                    {
                        writer.WriteLine("");
                        writer.WriteLine($"=== NEW SESSION STARTED {DateTime.Now} ===");
                        writer.WriteLine("============================================");
                    }
                }

                // Initialize flush timer
                _flushTimer = new System.Timers.Timer(_flushIntervalMs);
                _flushTimer.Elapsed += (s, e) => FlushQueue();
                _flushTimer.AutoReset = true;
                _flushTimer.Start();

                _isInitialized = true;
                Log(LogLevel.INFO, "DebugLogger initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize DebugLogger: {ex.Message}");
            }
        }

        // Main logging method
        public static void Log(LogLevel level, string message)
        {
            try
            {
                if (level == LogLevel.DEBUG && !AppConfig.DebugMode)
                    return;

                lock (_logLock)
                {
                    DateTime now = DateTime.Now;

                    // Check if this is a duplicate message
                    if (message == _lastMessage && level == _lastLogLevel)
                    {
                        // Just increment counter, don't log yet
                        _duplicateCount++;
                        return;
                    }
                    else
                    {
                        // Process the previous message with potential duplicates
                        if (_lastMessage != null && AppConfig.DebugMode)
                        {
                            var entry = new LogEntry
                            {
                                Message = _lastMessage,
                                Level = _lastLogLevel,
                                Timestamp = _lastMessageTime,
                                RepeatCount = _duplicateCount
                            };

                            // Add to queue and output to console
                            string formattedMessage = entry.FormatMessage();
                            Console.WriteLine(formattedMessage);
                            _messageQueue.Enqueue(entry);
                        }

                        // Set the new message as the current one
                        _lastMessage = message;
                        _lastLogLevel = level;
                        _lastMessageTime = now;
                        _duplicateCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DebugLogger.Log: {ex.Message}");
            }
        }

        // Various helper methods for different log levels
        public static void Info(string message) => Log(LogLevel.INFO, message);
        public static void Warning(string message) => Log(LogLevel.WARNING, message);
        public static void Error(string message) => Log(LogLevel.ERROR, message);
        public static void Error(Exception ex, string context = "")
        {
            string message = string.IsNullOrEmpty(context) ?
                $"Exception: {ex.Message}" :
                $"{context}: {ex.Message}";

            Log(LogLevel.ERROR, message);
            Log(LogLevel.DEBUG, $"Stack trace: {ex.StackTrace}");
        }
        public static void Debug(string message) => Log(LogLevel.DEBUG, message);

        // Method to log memory values with addresses
        public static void LogMemoryValue(string description, IntPtr address, object value)
        {
            Log(LogLevel.DEBUG, $"Memory {description}: Address={address.ToInt64():X8}, Value={value}");
        }

        // Method to flush queue to disk
        private static void FlushQueue()
        {
            if (!_isInitialized)
                return;

            try
            {
                List<LogEntry> messagesToWrite = null;

                lock (_logLock)
                {
                    // Don't flush if there's nothing to write
                    if (_messageQueue.Count == 0)
                        return;

                    messagesToWrite = _messageQueue.ToList();
                    _messageQueue.Clear();
                }

                if (messagesToWrite != null && messagesToWrite.Count > 0)
                {
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                    {
                        foreach (var entry in messagesToWrite)
                        {
                            writer.WriteLine(entry.FormatMessage());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error flushing log queue: {ex.Message}");
            }
        }

        // Call this on application exit to ensure all logs are written
        public static void Shutdown()
        {
            if (!_isInitialized)
                return;

            try
            {
                _flushTimer.Stop();

                // Ensure the current message gets written if it exists
                lock (_logLock)
                {
                    if (_lastMessage != null)
                    {
                        var entry = new LogEntry
                        {
                            Message = _lastMessage,
                            Level = _lastLogLevel,
                            Timestamp = _lastMessageTime,
                            RepeatCount = _duplicateCount
                        };

                        Console.WriteLine(entry.FormatMessage());
                        _messageQueue.Enqueue(entry);
                    }
                }

                FlushQueue();

                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"=== SESSION ENDED {DateTime.Now} ===");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during DebugLogger shutdown: {ex.Message}");
            }
        }
    }
}