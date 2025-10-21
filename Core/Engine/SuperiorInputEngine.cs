using BruteGamingMacros.Core.Utils;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BruteGamingMacros.Core.Engine
{
    /// <summary>
    /// SuperiorInputEngine - Ultra-fast input engine with hardware simulation
    ///
    /// Performance Targets:
    /// - Ultra Mode: 1000+ APS (1ms delay)
    /// - Turbo Mode: 200 APS (5ms delay)
    /// - Standard Mode: 100 APS (10ms delay)
    /// - Sub-millisecond latency with SpinWait
    /// - Hardware-level SendInput API for reliability
    /// </summary>
    public class SuperiorInputEngine
    {
        #region Win32 API Imports

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        // Input type constants
        private const uint INPUT_MOUSE = 0;
        private const uint INPUT_KEYBOARD = 1;
        private const uint INPUT_HARDWARE = 2;

        // Key event flags
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_UNICODE = 0x0004;
        private const uint KEYEVENTF_SCANCODE = 0x0008;

        #endregion

        #region Performance Tracking

        private long totalActionsExecuted = 0;
        private Stopwatch apsTimer = new Stopwatch();
        private double currentAPS = 0;
        private int apsUpdateCounter = 0;
        private const int APS_UPDATE_INTERVAL = 100; // Update APS every 100 actions

        /// <summary>
        /// Current Actions Per Second
        /// </summary>
        public double CurrentAPS
        {
            get
            {
                lock (this)
                {
                    return currentAPS;
                }
            }
        }

        /// <summary>
        /// Total actions executed since engine started
        /// </summary>
        public long TotalActionsExecuted
        {
            get
            {
                lock (this)
                {
                    return totalActionsExecuted;
                }
            }
        }

        #endregion

        #region Speed Modes

        /// <summary>
        /// Speed modes for the input engine
        /// </summary>
        public enum SpeedMode
        {
            /// <summary>Ultra mode: 1ms delay, 1000+ APS</summary>
            Ultra = 1,
            /// <summary>Turbo mode: 5ms delay, 200 APS</summary>
            Turbo = 5,
            /// <summary>Standard mode: 10ms delay, 100 APS</summary>
            Standard = 10
        }

        private SpeedMode currentMode = SpeedMode.Standard;

        /// <summary>
        /// Gets or sets the current speed mode
        /// </summary>
        public SpeedMode CurrentMode
        {
            get { return currentMode; }
            set { currentMode = value; }
        }

        #endregion

        #region High-Precision Timing

        /// <summary>
        /// High-precision delay using SpinWait for sub-millisecond accuracy
        /// Much more accurate than Thread.Sleep() for small delays
        /// </summary>
        private void PrecisionDelay(int milliseconds)
        {
            if (milliseconds <= 0) return;

            // For delays > 15ms, use hybrid approach: Sleep most of it, SpinWait the rest
            if (milliseconds > 15)
            {
                Thread.Sleep(milliseconds - 15);
                milliseconds = 15;
            }

            // Use SpinWait for sub-millisecond precision
            var sw = Stopwatch.StartNew();
            var target = TimeSpan.FromMilliseconds(milliseconds);

            SpinWait spinner = new SpinWait();
            while (sw.Elapsed < target)
            {
                spinner.SpinOnce();
            }
        }

        #endregion

        #region Input Execution Methods

        /// <summary>
        /// Sends a keyboard key press using hardware-level SendInput API
        /// This is MORE reliable than PostMessage and simulates actual hardware input
        /// </summary>
        public bool SendKeyPress(Keys key)
        {
            try
            {
                // Start APS timer on first action
                if (!apsTimer.IsRunning)
                {
                    apsTimer.Start();
                }

                ushort vkCode = (ushort)key;

                // Create input for key down
                INPUT[] inputs = new INPUT[2];

                // Key down
                inputs[0] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = vkCode,
                            wScan = 0,
                            dwFlags = 0,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };

                // Key up
                inputs[1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = vkCode,
                            wScan = 0,
                            dwFlags = KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = GetMessageExtraInfo()
                        }
                    }
                };

                // Send the input
                uint result = SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));

                // Update performance tracking
                UpdatePerformanceMetrics();

                return result == 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperiorInputEngine.SendKeyPress error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sends rapid key presses with configurable speed mode
        /// </summary>
        public void SendRapidKeyPress(Keys key, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SendKeyPress(key);
                PrecisionDelay((int)currentMode);
            }
        }

        /// <summary>
        /// Sends continuous key presses until stopped
        /// Returns actions per second achieved
        /// </summary>
        public void SendContinuousKeyPress(Keys key, Func<bool> shouldContinue)
        {
            while (shouldContinue())
            {
                SendKeyPress(key);
                PrecisionDelay((int)currentMode);
            }
        }

        #endregion

        #region Performance Metrics

        /// <summary>
        /// Updates performance metrics (APS calculation)
        /// </summary>
        private void UpdatePerformanceMetrics()
        {
            lock (this)
            {
                totalActionsExecuted++;
                apsUpdateCounter++;

                // Update APS every N actions to reduce overhead
                if (apsUpdateCounter >= APS_UPDATE_INTERVAL)
                {
                    if (apsTimer.Elapsed.TotalSeconds > 0)
                    {
                        currentAPS = totalActionsExecuted / apsTimer.Elapsed.TotalSeconds;
                    }
                    apsUpdateCounter = 0;
                }
            }
        }

        /// <summary>
        /// Resets performance metrics
        /// </summary>
        public void ResetMetrics()
        {
            lock (this)
            {
                totalActionsExecuted = 0;
                apsUpdateCounter = 0;
                currentAPS = 0;
                apsTimer.Reset();
                apsTimer.Start();
            }
        }

        /// <summary>
        /// Gets detailed performance report
        /// </summary>
        public string GetPerformanceReport()
        {
            lock (this)
            {
                double elapsedSeconds = apsTimer.Elapsed.TotalSeconds;
                return $"Mode: {currentMode} | " +
                       $"APS: {currentAPS:F1} | " +
                       $"Total: {totalActionsExecuted} | " +
                       $"Time: {elapsedSeconds:F1}s | " +
                       $"Target: {1000 / (int)currentMode} APS";
            }
        }

        #endregion

        #region Benchmarking

        /// <summary>
        /// Runs a benchmark test to measure actual APS performance
        /// </summary>
        public BenchmarkResult RunBenchmark(SpeedMode mode, int durationSeconds = 5)
        {
            var oldMode = currentMode;
            currentMode = mode;
            ResetMetrics();

            var sw = Stopwatch.StartNew();
            var testKey = Keys.F12; // Use F12 for testing

            long actionCount = 0;
            while (sw.Elapsed.TotalSeconds < durationSeconds)
            {
                SendKeyPress(testKey);
                PrecisionDelay((int)mode);
                actionCount++;
            }
            sw.Stop();

            var result = new BenchmarkResult
            {
                Mode = mode,
                ActionsExecuted = actionCount,
                ElapsedSeconds = sw.Elapsed.TotalSeconds,
                APS = actionCount / sw.Elapsed.TotalSeconds,
                TargetAPS = 1000.0 / (int)mode,
                LatencyMs = sw.Elapsed.TotalMilliseconds / actionCount
            };

            currentMode = oldMode;
            return result;
        }

        public class BenchmarkResult
        {
            public SpeedMode Mode { get; set; }
            public long ActionsExecuted { get; set; }
            public double ElapsedSeconds { get; set; }
            public double APS { get; set; }
            public double TargetAPS { get; set; }
            public double LatencyMs { get; set; }

            public override string ToString()
            {
                double efficiency = (APS / TargetAPS) * 100;
                return $"[{Mode} Mode Benchmark]\n" +
                       $"Actions: {ActionsExecuted}\n" +
                       $"Duration: {ElapsedSeconds:F2}s\n" +
                       $"APS: {APS:F1} (Target: {TargetAPS:F1})\n" +
                       $"Efficiency: {efficiency:F1}%\n" +
                       $"Avg Latency: {LatencyMs:F3}ms";
            }
        }

        #endregion
    }
}
