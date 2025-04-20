using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using _4RTools.Model;

namespace _4RTools.Utils
{
    public static class DebugLogger
    {
        private static readonly object _logLock = new object();
        private static readonly string _logFilePath = AppConfig.DebugLogFilePath;
        private static readonly bool _debugMode;
        private static bool _isInitialized = false;
        private static bool _initializationLogged = false;

        // Deduplication for INFO and STATUS
        private static string _lastInfoMessage = null;
        private static string _lastStatusMessage = null;

        // Deduplication for queued messages (WARNING, ERROR, DEBUG)
        private static string _lastQueuedMessage = null;
        private static LogLevel _lastQueuedLogLevel = LogLevel.INFO;
        private static DateTime _lastQueuedTimestamp = DateTime.MinValue;
        private static int _queuedDuplicateCount = 0;
        private static readonly Queue<LogEntry> _messageQueue = new Queue<LogEntry>();

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

                lock (_logLock)
                {
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

                    _isInitialized = true;

                    if (!_initializationLogged)
                    {
                        Info("DebugLogger initialized successfully");
                        _initializationLogged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize DebugLogger: {ex.Message}");
            }
        }

        public static void Log(LogLevel level, string message)
        {
            if (!_debugMode && level != LogLevel.ERROR)
                return;

            try
            {
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
                            if (_isInitialized && _debugMode)
                            {
                                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                                {
                                    writer.WriteLine(formattedMessage);
                                }
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
                            if (_isInitialized && _debugMode)
                            {
                                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                                {
                                    writer.WriteLine(formattedMessage);
                                }
                            }
                            _lastStatusMessage = message;
                        }
                    }
                    else
                    {
                        // Queue other levels (WARNING, ERROR, DEBUG)
                        if (message == _lastQueuedMessage && level == _lastQueuedLogLevel)
                        {
                            _queuedDuplicateCount++;
                            return;
                        }

                        // Log previous queued message if it exists
                        if (_lastQueuedMessage != null && _isInitialized && _debugMode)
                        {
                            var entry = new LogEntry
                            {
                                Message = _lastQueuedMessage,
                                Level = _lastQueuedLogLevel,
                                Timestamp = _lastQueuedTimestamp,
                                RepeatCount = _queuedDuplicateCount
                            };
                            string formattedMessage = entry.FormatMessage();
                            Console.WriteLine(formattedMessage);
                            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(formattedMessage);
                            }
                            _messageQueue.Enqueue(entry);
                        }

                        // Update deduplication state and enqueue new message
                        _lastQueuedMessage = message;
                        _lastQueuedLogLevel = level;
                        _lastQueuedTimestamp = now;
                        _queuedDuplicateCount = 0;

                        // Enqueue new message without logging immediately
                        var newEntry = new LogEntry
                        {
                            Message = message,
                            Level = level,
                            Timestamp = now,
                            RepeatCount = 0
                        };
                        _messageQueue.Enqueue(newEntry);
                    }
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

        public static void Shutdown()
        {
            if (!_isInitialized || !_debugMode)
                return;

            try
            {
                lock (_logLock)
                {
                    // Log any pending deduplicated message
                    if (_lastQueuedMessage != null)
                    {
                        var entry = new LogEntry
                        {
                            Message = _lastQueuedMessage,
                            Level = _lastQueuedLogLevel,
                            Timestamp = _lastQueuedTimestamp,
                            RepeatCount = _queuedDuplicateCount
                        };
                        string formattedMessage = entry.FormatMessage();
                        Console.WriteLine(formattedMessage);
                        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                        {
                            writer.WriteLine(formattedMessage);
                        }
                    }

                    // Clear queue without re-logging
                    _messageQueue.Clear();

                    // Log shutdown message
                    string shutdownMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [INFO] Shutting down logger...";
                    Console.WriteLine(shutdownMessage);
                    using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                    {
                        writer.WriteLine(shutdownMessage);
                        writer.WriteLine($"=== SESSION ENDED {DateTime.Now} ===");
                    }

                    // Reset deduplication state
                    _lastInfoMessage = null;
                    _lastStatusMessage = null;
                    _lastQueuedMessage = null;
                    _lastQueuedLogLevel = LogLevel.INFO;
                    _lastQueuedTimestamp = DateTime.MinValue;
                    _queuedDuplicateCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during DebugLogger shutdown: {ex.Message}");
            }
        }
    }
}