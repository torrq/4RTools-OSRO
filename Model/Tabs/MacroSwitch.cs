using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Cursor = System.Windows.Forms.Cursor;

namespace _ORTools.Model
{
    public class MacroSwitchKey
    {
        public Keys Key { get; set; }

        public static int TOTAL_MACRO_LANES = ConfigGlobal.GetConfig().MacroSwitchRows;
        public static int TOTAL_MACRO_KEYS = 7;

        private int _delay = AppConfig.MacroDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.MacroDefaultDelay : _delay;
            set => _delay = value;
        }

        /// <summary>
        /// Represents the click behavior for the skill timer.
        /// 0: No Click
        /// 1: Click at current mouse position
        /// 2: Click at the center of the game window
        /// </summary>
        public int ClickMode { get; set; } = 0;

        /// <summary>
        /// Constructor for creating new instances programmatically.
        /// </summary>
        public MacroSwitchKey(Keys key, int delay, int clickMode = 0)
        {
            this.Key = key;
            this.Delay = delay;
            this.ClickMode = clickMode;
        }

        /// <summary>
        /// Constructor used by Newtonsoft.Json for deserialization.
        /// This allows loading profiles that may or may not contain the click-related properties.
        /// </summary>
        [JsonConstructor]
        public MacroSwitchKey(Keys key, int delay)
        {
            this.Key = key;
            this.Delay = delay;
        }

        public MacroSwitchKey() { }  // Default constructor needed for some deserialization scenarios.
    }

    public class MacroSwitchChainConfig
    {
        public int id;

        /// <summary>
        /// The trigger key that activates this macro chain
        /// </summary>
        public Keys TriggerKey { get; set; } = Keys.None;

        public List<MacroSwitchKey> macroEntries { get; set; } = new List<MacroSwitchKey>();

        public MacroSwitchChainConfig() { }

        public MacroSwitchChainConfig(int id)
        {
            this.id = id;
            this.TriggerKey = Keys.None;
            this.macroEntries = new List<MacroSwitchKey>();
            for (int i = 0; i < MacroSwitchKey.TOTAL_MACRO_KEYS; i++)
            {
                this.macroEntries.Add(new MacroSwitchKey(Keys.None, AppConfig.MacroDefaultDelay));
            }
        }

        public MacroSwitchChainConfig(MacroSwitchChainConfig macro)
        {
            this.id = macro.id;
            this.TriggerKey = macro.TriggerKey;
            this.macroEntries = new List<MacroSwitchKey>(macro.macroEntries);
        }

        public MacroSwitchChainConfig(int id, Keys trigger)
        {
            this.id = id;
            this.TriggerKey = trigger;
            this.macroEntries = new List<MacroSwitchKey>();
            for (int i = 0; i < MacroSwitchKey.TOTAL_MACRO_KEYS; i++)
            {
                this.macroEntries.Add(new MacroSwitchKey(Keys.None, AppConfig.MacroDefaultDelay));
            }
        }
    }

    public class MacroSwitch : IAction
    {
        public static string ACTION_NAME_MACRO_SWITCH = "MacroSwitch";

        public string ActionName { get; set; }
        private ThreadRunner thread;
        public List<MacroSwitchChainConfig> ChainConfigs { get; set; } = new List<MacroSwitchChainConfig>();

        public MacroSwitch(string macroname, int macroLanes)
        {
            this.ActionName = macroname;
            for (int i = 1; i <= macroLanes; i++)
            {
                ChainConfigs.Add(new MacroSwitchChainConfig(i, Keys.None));
            }
        }

        public void ResetMacro(int macroId)
        {
            try
            {
                ChainConfigs[macroId - 1] = new MacroSwitchChainConfig(macroId);
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        private int MacroThread(Client roClient)
        {
            foreach (MacroSwitchChainConfig chainConfig in this.ChainConfigs)
            {
                // Use the dedicated TriggerKey instead of the first non-None key
                if (chainConfig.TriggerKey == Keys.None)
                    continue; // no trigger key set, skip this chain

                if (Win32Interop.IsKeyPressed(chainConfig.TriggerKey))
                {
                    foreach (var macroKey in chainConfig.macroEntries)
                    {
                        if (macroKey.Key != Keys.None)
                        {
                            // Send the key
                            Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, macroKey.Key, 0);

                            // Handle click behavior
                            if (macroKey.ClickMode == 1)
                            {
                                // Click at current mouse position
                                MouseHelper.TryClickAtCurrentPosition(roClient.Process.MainWindowHandle);
                            }
                            else if (macroKey.ClickMode == 2)
                            {
                                // Click at center of game window
                                MouseHelper.TryClickAtWindowCenter(roClient.Process.MainWindowHandle);
                            }

                            Thread.Sleep(macroKey.Delay); // delay after sending key and/or click
                        }
                    }
                }
            }
            Thread.Sleep(100);
            return 0;
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                }
                this.thread = new ThreadRunner((_) => MacroThread(roClient), "MacroSwitch");
                ThreadRunner.Start(this.thread);
            }
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                ThreadRunner.Stop(this.thread);
                this.thread.Terminate();
                this.thread = null;
            }
        }
    }
}