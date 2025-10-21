using BruteGamingMacros.Core.Model;
using System;
using System.IO;
using System.Text;

namespace BruteGamingMacros.Core.Utils
{
    public static class DebugLogger
    {
        private static readonly object _logLock = new object();
        private static readonly string _logFilePath = AppConfig.DebugLogFile;
        private static bool _debugMode;
        private static bool _isInitialized = false;
        private static bool _initializationLogged = false;
        private static string _sessionDate;

        public delegate void LogMessageHandler(string message, LogLevel level);

        public static event LogMessageHandler OnLogMessage;

        public enum LogLevel
        {
            INFO,
            WARNING,
            ERROR,
            DEBUG,
            STATUS
        }

        static DebugLogger()
        {
            _debugMode = ConfigGlobal.GetConfig().DebugMode;
            InitializeLogger();
        }

        public static void InitializeLogger()
        {
            if (!_debugMode || _isInitialized) return;

            try
            {
                lock (_logLock)
                {
                    string dir = Path.GetDirectoryName(_logFilePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    _sessionDate = DateTime.Now.ToString("yyyy-MM-dd");

                    if (!File.Exists(_logFilePath))
                    {
                        using (StreamWriter writer = new StreamWriter(_logFilePath, false, Encoding.UTF8))
                        {
                            writer.WriteLine("=== " + AppConfig.Name.ToUpper() + " DEBUG LOG ===");
                            writer.WriteLine("=== SESSION DATE " + _sessionDate + " ===");
                            writer.WriteLine("============================================");
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                        {
                            writer.WriteLine();
                            writer.WriteLine("=== NEW SESSION STARTED " + _sessionDate + " " + DateTime.Now.ToString("HH:mm:ss") + " ===");
                            writer.WriteLine("============================================");
                        }
                    }

                    _isInitialized = true;

                    if (!_initializationLogged)
                    {
                        Info("DebugLogger initialized");
                        _initializationLogged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Failed to initialize DebugLogger: " + ex.Message;
                Console.WriteLine($"[{AppConfig.ERROR}] " + msg);
                OnLogMessage?.Invoke($"[{AppConfig.ERROR}] " + msg, LogLevel.ERROR);
            }
        }

        public static void UpdateDebugMode(bool newDebugMode)
        {
            lock (_logLock)
            {
                bool old = _debugMode;
                _debugMode = newDebugMode;

                if (newDebugMode && !old && !_isInitialized)
                    InitializeLogger();
                else if (!newDebugMode && old && _isInitialized)
                {
                    Shutdown();
                    _isInitialized = false;
                    _initializationLogged = false;
                }
            }
        }

        public static void Log(LogLevel level, string message)
        {
            if (!_debugMode && level != LogLevel.ERROR) return;

            string time = DateTime.Now.ToString("HH:mm:ss.fff");
            string logLevelName = GetLogLevelName(level);
            string formatted = time + " [" + logLevelName + "] " + message;

            try
            {
                lock (_logLock)
                {
                    Console.WriteLine(formatted);

                    if (_isInitialized && _debugMode)
                    {
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(formatted);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[{AppConfig.ERROR}] Failed to write log: " + ex.Message);
                        }
                    }

                    OnLogMessage?.Invoke(formatted, level);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{AppConfig.ERROR}] Logging failure: " + ex.Message);
            }
        }

        private static string GetLogLevelName(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.INFO:
                    return AppConfig.INFO;

                case LogLevel.WARNING:
                    return AppConfig.WARNING;

                case LogLevel.ERROR:
                    return AppConfig.ERROR;

                case LogLevel.DEBUG:
                    return AppConfig.DEBUG;

                case LogLevel.STATUS:
                    return AppConfig.STATUS;

                default:
                    return level.ToString();
            }
        }

        public static void Info(string message) => Log(LogLevel.INFO, message);

        public static void Status(string message) => Log(LogLevel.STATUS, message);

        public static void Warning(string message) => Log(LogLevel.WARNING, message);

        public static void Error(string message) => Log(LogLevel.ERROR, message);

        public static void Error(Exception ex, string context = "")
        {
            string msg = string.IsNullOrEmpty(context)
                ? "Exception: " + ex.Message
                : context + ": " + ex.Message;
            Log(LogLevel.ERROR, msg);

            /*
            if (_debugMode)
                Log(LogLevel.DEBUG, "Stack trace: " + ex.StackTrace);
            */
        }

        public static void Debug(string message) => Log(LogLevel.DEBUG, message);

        public static void LogMemoryValue(string desc, IntPtr addr, object val)
        {
            Log(LogLevel.DEBUG, "Memory " + desc + ": Addr=" + addr.ToInt64().ToString("X8") + ", Val=" + val);
        }

        public static void Shutdown()
        {
            if (!_isInitialized || !_debugMode) return;

            try
            {
                lock (_logLock)
                {
                    string time = DateTime.Now.ToString("HH:mm:ss.fff");
                    string msg = time + $" [{AppConfig.INFO}] Shutting down logger...";
                    Console.WriteLine(msg);

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                        {
                            writer.WriteLine(msg);
                            writer.WriteLine("=== SESSION ENDED " + _sessionDate + " " + time + " ===");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{AppConfig.ERROR}] Failed to write shutdown log: " + ex.Message);
                    }

                    OnLogMessage?.Invoke(msg, LogLevel.INFO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{AppConfig.ERROR}] Shutdown failure: " + ex.Message);
            }
            finally
            {
                lock (_logLock)
                {
                    _isInitialized = false;
                    _initializationLogged = false;
                }
            }
        }
    }
}