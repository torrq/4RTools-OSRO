using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Engine
{
    /// <summary>
    /// SuperiorSkillSpammer - Advanced skill spamming with multiple modes
    ///
    /// Features:
    /// - Burst Mode: Maximum speed spam
    /// - Adaptive Mode: Adjusts speed based on SP/HP
    /// - Smart Mode: Pauses when conditions not met
    /// - Uses SuperiorInputEngine for sub-millisecond timing
    /// </summary>
    public class SuperiorSkillSpammer
    {
        private SuperiorInputEngine inputEngine;
        private ThreadRunner thread;
        private bool isRunning = false;

        /// <summary>
        /// Spam execution modes
        /// </summary>
        public enum SpamMode
        {
            /// <summary>Continuous max-speed spam</summary>
            Burst,
            /// <summary>Adapts speed based on resources</summary>
            Adaptive,
            /// <summary>Smart pausing based on conditions</summary>
            Smart
        }

        private SpamMode currentSpamMode = SpamMode.Burst;

        /// <summary>
        /// Gets or sets the current spam mode
        /// </summary>
        public SpamMode CurrentSpamMode
        {
            get { return currentSpamMode; }
            set { currentSpamMode = value; }
        }

        /// <summary>
        /// Configuration for skill spamming
        /// </summary>
        public class SpamConfiguration
        {
            /// <summary>Key to spam</summary>
            public Keys Key { get; set; }

            /// <summary>Speed mode (Ultra/Turbo/Standard)</summary>
            public SuperiorInputEngine.SpeedMode SpeedMode { get; set; } = SuperiorInputEngine.SpeedMode.Standard;

            /// <summary>Minimum HP percentage to continue spamming</summary>
            public int MinHpPercent { get; set; } = 20;

            /// <summary>Minimum SP percentage to continue spamming</summary>
            public int MinSpPercent { get; set; } = 5;

            /// <summary>Enable adaptive speed based on SP</summary>
            public bool EnableAdaptiveSpeed { get; set; } = false;

            /// <summary>Enable smart pausing</summary>
            public bool EnableSmartPause { get; set; } = true;
        }

        public SuperiorSkillSpammer()
        {
            inputEngine = new SuperiorInputEngine();
        }

        /// <summary>
        /// Gets the input engine for metrics access
        /// </summary>
        public SuperiorInputEngine InputEngine
        {
            get { return inputEngine; }
        }

        /// <summary>
        /// Starts skill spamming with given configuration
        /// </summary>
        public void Start(SpamConfiguration config)
        {
            Stop();

            Client roClient = ClientSingleton.GetClient();
            if (roClient == null)
            {
                Console.WriteLine("SuperiorSkillSpammer: No client available");
                return;
            }

            isRunning = true;
            inputEngine.CurrentMode = config.SpeedMode;
            inputEngine.ResetMetrics();

            thread = new ThreadRunner((_) => SpamExecutionThread(roClient, config));
            ThreadRunner.Start(thread);

            Console.WriteLine($"SuperiorSkillSpammer started - Mode: {currentSpamMode}, Speed: {config.SpeedMode}");
        }

        /// <summary>
        /// Stops skill spamming
        /// </summary>
        public void Stop()
        {
            isRunning = false;

            if (thread != null)
            {
                ThreadRunner.Stop(thread);
                thread.Terminate();
                thread = null;
            }

            Console.WriteLine($"SuperiorSkillSpammer stopped - {inputEngine.GetPerformanceReport()}");
        }

        /// <summary>
        /// Main spam execution thread
        /// </summary>
        private int SpamExecutionThread(Client client, SpamConfiguration config)
        {
            try
            {
                // Check if we should continue spamming
                if (!ShouldContinueSpamming(client, config))
                {
                    Thread.Sleep(100); // Pause briefly before rechecking
                    return 0;
                }

                // Execute spam based on current mode
                switch (currentSpamMode)
                {
                    case SpamMode.Burst:
                        ExecuteBurstMode(client, config);
                        break;

                    case SpamMode.Adaptive:
                        ExecuteAdaptiveMode(client, config);
                        break;

                    case SpamMode.Smart:
                        ExecuteSmartMode(client, config);
                        break;
                }

                // Small sleep to prevent 100% CPU usage
                Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperiorSkillSpammer error: {ex.Message}");
                Thread.Sleep(100);
            }

            return 0;
        }

        /// <summary>
        /// Checks if spamming should continue based on client state
        /// </summary>
        private bool ShouldContinueSpamming(Client client, SpamConfiguration config)
        {
            try
            {
                // Check if client is still online
                if (!client.IsOnline())
                {
                    return false;
                }

                // Check HP threshold
                if (config.MinHpPercent > 0)
                {
                    uint currentHp = client.ReadCurrentHp();
                    uint maxHp = client.ReadMaxHp();
                    if (maxHp > 0)
                    {
                        int hpPercent = (int)((currentHp * 100) / maxHp);
                        if (hpPercent < config.MinHpPercent)
                        {
                            return false;
                        }
                    }
                }

                // Check SP threshold
                if (config.MinSpPercent > 0)
                {
                    uint currentSp = client.ReadCurrentSp();
                    uint maxSp = client.ReadMaxSp();
                    if (maxSp > 0)
                    {
                        int spPercent = (int)((currentSp * 100) / maxSp);
                        if (spPercent < config.MinSpPercent)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Burst Mode: Maximum speed continuous spam
        /// </summary>
        private void ExecuteBurstMode(Client client, SpamConfiguration config)
        {
            inputEngine.SendKeyPress(config.Key);
        }

        /// <summary>
        /// Adaptive Mode: Adjusts speed based on SP levels
        /// </summary>
        private void ExecuteAdaptiveMode(Client client, SpamConfiguration config)
        {
            try
            {
                // Calculate current SP percentage
                uint currentSp = client.ReadCurrentSp();
                uint maxSp = client.ReadMaxSp();

                if (maxSp == 0)
                {
                    // Fallback to burst mode if can't read SP
                    ExecuteBurstMode(client, config);
                    return;
                }

                int spPercent = (int)((currentSp * 100) / maxSp);

                // Adaptive speed based on SP levels:
                // >70% SP = Ultra mode (fastest)
                // 40-70% SP = Turbo mode (medium)
                // <40% SP = Standard mode (slower to conserve SP)
                SuperiorInputEngine.SpeedMode adaptiveMode;
                if (spPercent > 70)
                {
                    adaptiveMode = SuperiorInputEngine.SpeedMode.Ultra;
                }
                else if (spPercent > 40)
                {
                    adaptiveMode = SuperiorInputEngine.SpeedMode.Turbo;
                }
                else
                {
                    adaptiveMode = SuperiorInputEngine.SpeedMode.Standard;
                }

                // Temporarily switch to adaptive mode
                var originalMode = inputEngine.CurrentMode;
                inputEngine.CurrentMode = adaptiveMode;
                inputEngine.SendKeyPress(config.Key);
                inputEngine.CurrentMode = originalMode;
            }
            catch
            {
                // Fallback to burst mode on error
                ExecuteBurstMode(client, config);
            }
        }

        /// <summary>
        /// Smart Mode: Pauses spam intelligently based on game state
        /// </summary>
        private void ExecuteSmartMode(Client client, SpamConfiguration config)
        {
            try
            {
                // Check for debuffs that should pause spamming
                bool shouldPause = false;

                // Check for Silence (can't cast skills)
                if (HasDebuff(client, EffectStatusIDs.SILENCE))
                {
                    shouldPause = true;
                }

                // Check for Stun (can't do anything)
                if (HasDebuff(client, EffectStatusIDs.STUN))
                {
                    shouldPause = true;
                }

                // Check for Frozen (can't do anything)
                if (HasDebuff(client, EffectStatusIDs.FREEZING))
                {
                    shouldPause = true;
                }

                if (shouldPause)
                {
                    Thread.Sleep(100);
                    return;
                }

                // Otherwise execute normally
                ExecuteBurstMode(client, config);
            }
            catch
            {
                // Fallback to burst mode on error
                ExecuteBurstMode(client, config);
            }
        }

        /// <summary>
        /// Checks if client has a specific debuff
        /// </summary>
        private bool HasDebuff(Client client, EffectStatusIDs debuffId)
        {
            try
            {
                for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                {
                    uint statusCode = client.CurrentBuffStatusCode(i);
                    if (statusCode == (uint)debuffId)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets current performance metrics
        /// </summary>
        public string GetPerformanceMetrics()
        {
            return inputEngine.GetPerformanceReport();
        }

        /// <summary>
        /// Gets current APS (Actions Per Second)
        /// </summary>
        public double GetCurrentAPS()
        {
            return inputEngine.CurrentAPS;
        }

        /// <summary>
        /// Runs a benchmark test
        /// </summary>
        public SuperiorInputEngine.BenchmarkResult RunBenchmark(
            SuperiorInputEngine.SpeedMode mode,
            int durationSeconds = 5)
        {
            return inputEngine.RunBenchmark(mode, durationSeconds);
        }
    }
}
