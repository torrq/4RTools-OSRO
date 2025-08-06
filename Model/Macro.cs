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
        public bool AltKey { get; set; }
        public bool Enabled { get; set; }

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

        // This property is for backward compatibility. It catches the old "ClickActive" boolean
        // during JSON deserialization and converts it to the new "ClickMode" integer value.
        [JsonProperty("ClickActive", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private bool ObsoleteClickActive { set { ClickMode = value ? 1 : 0; } }


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
        public Keys Trigger { get; set; }
        public Keys DaggerKey { get; set; }
        public Keys InstrumentKey { get; set; }
        public int Delay { get; set; } = AppConfig.MacroDefaultDelay;
        public Dictionary<string, MacroKey> macroEntries { get; set; } = new Dictionary<string, MacroKey>();

        public ChainConfig() { }
        public ChainConfig(int id)
        {
            this.id = id;
            this.macroEntries = new Dictionary<string, MacroKey>();
        }

        public ChainConfig(ChainConfig macro)
        {
            this.id = macro.id;
            this.Delay = macro.Delay;
            this.Trigger = macro.Trigger;
            this.DaggerKey = macro.DaggerKey;
            this.InstrumentKey = macro.InstrumentKey;
            this.macroEntries = new Dictionary<string, MacroKey>(macro.macroEntries);
        }
        public ChainConfig(int id, Keys trigger)
        {
            this.id = id;
            this.Trigger = trigger;
            this.macroEntries = new Dictionary<string, MacroKey>();
        }
    }

    public class Macro : IAction
    {
        public static string ACTION_NAME_SONG_MACRO = "SongMacro2.0";
        public static string ACTION_NAME_MACRO_SWITCH = "MacroSwitch";

        public string ActionName { get; set; }
        private ThreadRunner thread;
        public List<ChainConfig> ChainConfigs { get; set; } = new List<ChainConfig>();

        public Macro(string macroname, int macroLanes)
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
                if (chainConfig.Trigger != Keys.None && Win32Interop.IsKeyPressed(chainConfig.Trigger))
                {
                    Dictionary<string, MacroKey> macro = chainConfig.macroEntries;
                    for (int i = 1; i <= macro.Count; i++)//Ensure to execute keys in Order
                    {
                        MacroKey macroKey = macro["in" + i + "mac" + chainConfig.id];
                        if (macroKey.Key != Keys.None)
                        {
                            if (chainConfig.InstrumentKey != Keys.None)
                            {
                                //Press instrument key if exists.
                                Keys instrumentKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.InstrumentKey.ToString());
                                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, instrumentKey, 0);
                                Thread.Sleep(30);
                            }

                            Keys thisk = (Keys)Enum.Parse(typeof(Keys), macroKey.Key.ToString());
                            Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                            Thread.Sleep(macroKey.Delay); // Delay after key rather than before

                            if (chainConfig.DaggerKey != Keys.None)
                            {
                                //Press instrument key if exists.
                                Keys daggerKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.DaggerKey.ToString());
                                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, daggerKey, 0);
                                Thread.Sleep(30);
                            }

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
                this.thread = new ThreadRunner((_) => MacroThread(roClient), "Macro");
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