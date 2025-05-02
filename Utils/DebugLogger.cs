using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Utils
{
    public static class DebugLogger
    {
        private static readonly object _logLock = new object();
        private static readonly string _logFilePath = AppConfig.DebugLogFilePath;
        private static bool _debugMode;
        private static bool _isInitialized = false;
        private static bool _initializationLogged = false;

        // Event to notify UI of new log messages
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
            // Initialize _debugMode based on config at startup
            _debugMode = ConfigGlobal.GetConfig().DebugMode;
            InitializeLogger();
        }

        // Added a public method to allow re-initialization if debug mode changes
        public static void InitializeLogger()
        {
            // Ensure this is only run if debug mode is enabled and not already initialized
            if (!_debugMode || _isInitialized)
                return;

            try
            {
                // Use the lock here too, as initialization involves file access
                lock (_logLock)
                {
                    // Ensure directory exists if needed
                    string logDirectory = Path.GetDirectoryName(_logFilePath);
                    if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

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
                        // Append to existing log file
                        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                        {
                            writer.WriteLine();
                            writer.WriteLine($"=== NEW SESSION STARTED {DateTime.Now} ===");
                            writer.WriteLine("============================================");
                        }
                    }

                    _isInitialized = true;

                    // Log successful initialization only once per application run
                    if (!_initializationLogged)
                    {
                        Info("DebugLogger initialized successfully");
                        _initializationLogged = true;
                    }
                } // Release lock
            }
            catch (Exception ex)
            {
                // Log errors that occur during initialization, trying to avoid infinite loops
                string errorMsg = $"Failed to initialize DebugLogger: {ex.Message}";
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {errorMsg}");
                // Even if file logging fails initialization, we can still try to send to UI
                // Format manually here to avoid calling Log() recursively during logger issues
                OnLogMessage?.Invoke($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {errorMsg}", LogLevel.ERROR);
            }
        }

        // Method to update debug mode from ConfigForm
        public static void UpdateDebugMode(bool newDebugMode)
        {
            // Use lock as this accesses shared state (_debugMode, _isInitialized) and calls Initialize/Shutdown
            lock (_logLock)
            {
                bool oldDebugMode = _debugMode;
                _debugMode = newDebugMode;

                if (newDebugMode && !oldDebugMode && !_isInitialized)
                {
                    // If debug mode is turned ON, was previously OFF, and logger was not initialized, initialize it
                    // The check inside InitializeLogger() prevents double initialization if called elsewhere
                    InitializeLogger();
                }
                else if (!newDebugMode && oldDebugMode && _isInitialized)
                {
                    // If debug mode is turned OFF, was previously ON, and logger was initialized, perform shutdown
                    // The check inside Shutdown() prevents double shutdown if called elsewhere
                    Shutdown();
                    // Reset state variables specifically tied to initialization state
                    _isInitialized = false;
                    _initializationLogged = false;
                }
                // If debug mode state didn't change or state transition doesn't require init/shutdown, do nothing.
            }
        }


        public static void Log(LogLevel level, string message)
        {
            // Only process messages if debug mode is ON, or if it's an ERROR message
            if (!_debugMode && level != LogLevel.ERROR)
            {
                return;
            }

            // Format the message with timestamp and level
            DateTime now = DateTime.Now;
            string formattedMessage = $"[{now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";

            try
            {
                // Use the lock for thread safety when writing to file and console
                lock (_logLock)
                {
                    // Write to Console
                    Console.WriteLine(formattedMessage);

                    // Write to File (only if initialized AND debug mode is ON)
                    // Note: ERRORs will ONLY go to the file if debugMode is ON due to this check.
                    // If you need ERRORs always in the file regardless of debugMode, move file writing outside this check
                    // and add a specific check for LogLevel.ERROR.
                    if (_isInitialized && _debugMode)
                    {
                        try
                        {
                            // Use 'true' to append to the file
                            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(formattedMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle file writing errors internally to avoid recursive logging issues
                            // Cannot use Log() here, would cause infinite loop.
                            string fileErrorMsg = $"Failed to write log to file '{_logFilePath}': {ex.Message}";
                            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {fileErrorMsg}");
                            // It's risky to send this file error back to the UI event via OnLogMessage here
                            // as it might cause issues if the UI logging is related to the file problem.
                        }
                    }

                    // Invoke the UI event handler (if any subscribers)
                    // This sends the message to the DebugLogWindow and potentially the Container's lambda
                    OnLogMessage?.Invoke(formattedMessage, level);
                } // Release lock
            }
            catch (Exception ex)
            {
                // This catch block handles errors that occur *within* the logging logic itself (e.g., lock issues)
                // Avoid infinite loops by only writing to Console
                string loggingErrorMsg = $"CRITICAL ERROR in DebugLogger.Log: {ex.Message}";
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {loggingErrorMsg}");
                // Cannot safely send this critical error via OnLogMessage without risk
            }
        }


        public static void Info(string message) => Log(LogLevel.INFO, message);
        public static void Status(string message) => Log(LogLevel.STATUS, message);
        public static void Warning(string message) => Log(LogLevel.WARNING, message);
        public static void Error(string message) => Log(LogLevel.ERROR, message);
        public static void Error(Exception ex, string context = "")
        {
            // Format the error message including exception details
            string message = string.IsNullOrEmpty(context)
                ? $"Exception: {ex.Message}"
                : $"{context}: {ex.Message}";

            Log(LogLevel.ERROR, message);

            // Log the stack trace as DEBUG, but only if DebugMode is ON
            if (_debugMode)
            {
                Log(LogLevel.DEBUG, $"Stack trace: {ex.StackTrace}");
            }
        }

        public static void Debug(string message) => Log(LogLevel.DEBUG, message);

        public static void LogMemoryValue(string description, IntPtr address, object value)
        {
            Log(LogLevel.DEBUG, $"Memory {description}: Address={address.ToInt64():X8}, Value={value}");
        }

        public static void Shutdown()
        {
            // Ensure shutdown logic runs only if the logger was initialized and debug mode was on
            if (!_isInitialized || !_debugMode)
                return;

            try
            {
                lock (_logLock)
                {
                    // Log shutdown message
                    string shutdownMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [INFO] Shutting down logger...";
                    Console.WriteLine(shutdownMessage);

                    // Write shutdown message and session end marker to file
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                        {
                            writer.WriteLine(shutdownMessage);
                            writer.WriteLine($"=== SESSION ENDED {DateTime.Now} ===");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle file writing errors during shutdown
                        string fileErrorMsg = $"Failed to write shutdown log to file '{_logFilePath}': {ex.Message}";
                        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {fileErrorMsg}");
                    }


                    // Invoke the UI event for the shutdown message
                    OnLogMessage?.Invoke(shutdownMessage, LogLevel.INFO);

                } // Release lock
            }
            catch (Exception ex)
            {
                // Handle errors during logger shutdown itself
                string shutdownErrorMsg = $"Error during DebugLogger shutdown: {ex.Message}";
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {shutdownErrorMsg}");
            }
            finally
            {
                // Ensure state is reset even if shutdown fails partially
                lock (_logLock) // Acquire lock one last time for state reset
                {
                    _isInitialized = false; // Mark as uninitialized
                    _initializationLogged = false; // Reset initialization flag
                }
            }
        }
    }
}