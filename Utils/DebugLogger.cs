using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using _4RTools.Model;

namespace _4RTools.Utils
{
    public static class DebugLogger
    {
        private static readonly object _logLock = new object();
        private static readonly string _logFilePath = AppConfig.DebugLogFilePath;
        private static readonly System.Timers.Timer _flushTimer;
        private static readonly Queue<LogEntry> _messageQueue = new Queue<LogEntry>();
        private static bool _isInitialized = false;
        private static readonly int _flushIntervalMs = 1000;

        private static readonly bool _debugMode;

        private static string _lastInfoMessage = null;
        private static string _lastStatusMessage = null;
        private static string _lastQueuedMessage = null;
        private static LogLevel _lastQueuedLogLevel = LogLevel.INFO;
        private static int _queuedDuplicateCount = 0;
        private static DateTime _lastQueuedMessageTime = DateTime.MinValue;

        public enum LogLevel
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG,
            STATUS
        }

        private class LogEntry
        {
            public string Message { get; set; }
            public LogLevel Level { get; set; }
            public DateTime Timestamp { get; set; }
            public int RepeatCount { get; set; }

            public string FormatMessage()
            {
                string baseMessage = $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}";
                return RepeatCount > 0 ? $"{baseMessage} (repeated {RepeatCount} times)" : baseMessage;
            }
        }

        static DebugLogger()
        {
            try
            {
                _debugMode = ConfigManager.GetConfig().DebugMode;

                if (!_debugMode)
                    return;

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
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                    {
                        writer.WriteLine();
                        writer.WriteLine($"=== NEW SESSION STARTED {DateTime.Now} ===");
                        writer.WriteLine("============================================");
                    }
                }

                _flushTimer = new System.Timers.Timer(_flushIntervalMs);
                _flushTimer.Elapsed += (s, e) => FlushQueue();
                _flushTimer.AutoReset = true;
                _flushTimer.Start();

                _isInitialized = true;
                Info("DebugLogger initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize DebugLogger: {ex.Message}");
            }
        }

        public static void Log(LogLevel level, string message)
        {
            try
            {
                if (!_debugMode && level != LogLevel.ERROR)
                    return;

                lock (_logLock)
                {
                    DateTime now = DateTime.Now;

                    if (level == LogLevel.INFO)
                    {
                        // Direct write for INFO
                        if (message != _lastInfoMessage)
                        {
                            string formattedMessage = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [INFO] {message}";
                            Console.WriteLine(formattedMessage);
                            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(formattedMessage);
                            }
                            _lastInfoMessage = message;
                        }
                    }
                    else if (level == LogLevel.STATUS)
                    {
                        // Direct write for STATUS
                        if (message != _lastStatusMessage)
                        {
                            string formattedMessage = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [STATUS] {message}";
                            Console.WriteLine(formattedMessage);
                            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(formattedMessage);
                            }
                            _lastStatusMessage = message;
                        }
                    }
                    else
                    {
                        // Queue other levels
                        if (message == _lastQueuedMessage && level == _lastQueuedLogLevel)
                        {
                            _queuedDuplicateCount++;
                            return;
                        }
                        else
                        {
                            if (_lastQueuedMessage != null && _debugMode)
                            {
                                var entry = new LogEntry
                                {
                                    Message = _lastQueuedMessage,
                                    Level = _lastQueuedLogLevel,
                                    Timestamp = _lastQueuedMessageTime,
                                    RepeatCount = _queuedDuplicateCount
                                };

                                Console.WriteLine(entry.FormatMessage());
                                _messageQueue.Enqueue(entry);
                            }

                            _lastQueuedMessage = message;
                            _lastQueuedLogLevel = level;
                            _lastQueuedMessageTime = now;
                            _queuedDuplicateCount = 0;

                            var newEntry = new LogEntry
                            {
                                Message = message,
                                Level = level,
                                Timestamp = now,
                                RepeatCount = 0
                            };

                            Console.WriteLine(newEntry.FormatMessage());
                            _messageQueue.Enqueue(newEntry);
                        }
                    }

                    if (!_debugMode && level == LogLevel.ERROR)
                        Console.WriteLine($"[ERROR] {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DebugLogger.Log: {ex.Message}");
            }
        }

        public static void Info(string message) => Log(LogLevel.INFO, message);
        public static void Status(string message) => Log(LogLevel.STATUS, message);
        public static void Warning(string message) => Log(LogLevel.WARNING, message);
        public static void Error(string message) => Log(LogLevel.ERROR, message);
        public static void Error(Exception ex, string context = "")
        {
            string message = string.IsNullOrEmpty(context)
                ? $"Exception: {ex.Message}"
                : $"{context}: {ex.Message}";

            Log(LogLevel.ERROR, message);
            Log(LogLevel.DEBUG, $"Stack trace: {ex.StackTrace}");
        }

        public static void Debug(string message) => Log(LogLevel.DEBUG, message);

        public static void LogMemoryValue(string description, IntPtr address, object value)
        {
            Log(LogLevel.DEBUG, $"Memory {description}: Address={address.ToInt64():X8}, Value={value}");
        }

        private static void FlushQueue()
        {
            if (!_isInitialized || !_debugMode)
                return;

            try
            {
                List<LogEntry> messagesToWrite;

                lock (_logLock)
                {
                    if (_messageQueue.Count == 0)
                        return;

                    messagesToWrite = _messageQueue.ToList();
                    _messageQueue.Clear();
                }

                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    foreach (var entry in messagesToWrite)
                    {
                        writer.WriteLine(entry.FormatMessage());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error flushing log queue: {ex.Message}");
            }
        }

        public static void Shutdown()
        {
            if (!_isInitialized || !_debugMode)
                return;

            try
            {
                _flushTimer.Stop();

                lock (_logLock)
                {
                    if (_lastQueuedMessage != null)
                    {
                        var entry = new LogEntry
                        {
                            Message = _lastQueuedMessage,
                            Level = _lastQueuedLogLevel,
                            Timestamp = _lastQueuedMessageTime,
                            RepeatCount = _queuedDuplicateCount
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