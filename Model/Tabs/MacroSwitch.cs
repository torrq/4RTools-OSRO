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
    public class MacroKey
    {
        public Keys Key { get; set; }

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
        public MacroKey(Keys key, int delay, int clickMode = 0)
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
        public MacroKey(Keys key, int delay)
        {
            this.Key = key;
            this.Delay = delay;
        }

        public MacroKey() { }  // Default constructor needed for some deserialization scenarios.
    }

    public class ChainConfig
    {
        public int id;

        public List<MacroKey> macroEntries { get; set; } = new List<MacroKey>();

        public ChainConfig() { }

        public ChainConfig(int id)
        {
            this.id = id;
            this.macroEntries = new List<MacroKey>();
            for (int i = 0; i < 7; i++)
            {
                this.macroEntries.Add(new MacroKey(Keys.None, AppConfig.MacroDefaultDelay));
            }
        }

        public ChainConfig(ChainConfig macro)
        {
            this.id = macro.id;
            this.macroEntries = new List<MacroKey>(macro.macroEntries);
        }

        public ChainConfig(int id, Keys trigger)
        {
            this.id = id;
            this.macroEntries = new List<MacroKey>();
            for (int i = 0; i < 7; i++)
            {
                this.macroEntries.Add(new MacroKey(Keys.None, AppConfig.MacroDefaultDelay));
            }
        }

    }


    public class MacroSwitch : IAction
    {
        public static string ACTION_NAME_MACRO_SWITCH = "MacroSwitch";

        public string ActionName { get; set; }
        private ThreadRunner thread;
        public List<ChainConfig> ChainConfigs { get; set; } = new List<ChainConfig>();

        public MacroSwitch(string macroname, int macroLanes)
        {
            this.ActionName = macroname;
            for (int i = 1; i <= macroLanes; i++)
            {
                ChainConfigs.Add(new ChainConfig(i, Keys.None));

            }
        }

        public void ResetMacro(int macroId)
        {
            try
            {
                ChainConfigs[macroId - 1] = new ChainConfig(macroId);
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
            foreach (ChainConfig chainConfig in this.ChainConfigs)
            {
                // Determine the trigger key as the first non-None key in macroEntries
                MacroKey triggerMacroKey = chainConfig.macroEntries.Find(k => k.Key != Keys.None);
                if (triggerMacroKey == null)
                    continue; // no valid keys, skip

                if (Win32Interop.IsKeyPressed(triggerMacroKey.Key))
                {
                    foreach (var macroKey in chainConfig.macroEntries)
                    {
                        if (macroKey.Key != Keys.None)
                        {
                            Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, macroKey.Key, 0);
                            Thread.Sleep(macroKey.Delay); // delay after sending key
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