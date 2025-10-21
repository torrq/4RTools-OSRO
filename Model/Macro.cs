using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class MacroKey
    {
        public Key Key { get; set; }
        private int _delay = AppConfig.MacroDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.MacroDefaultDelay : _delay;
            set => _delay = value;
        }
        public MacroKey(Key key, int delay)
        {
            this.Key = key;
            this.Delay = delay;
        }
    }

    public class ChainConfig
    {
        public int id;
        public Key Trigger { get; set; }
        public Key DaggerKey { get; set; }
        public Key InstrumentKey { get; set; }
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
        public ChainConfig(int id, Key trigger)
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
                ChainConfigs.Add(new ChainConfig(i, Key.None));

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

        private int MacroExecutionThread(Client roClient)
        {
            foreach (ChainConfig chainConfig in this.ChainConfigs)
            {
                if (chainConfig.Trigger != Key.None && Keyboard.IsKeyDown(chainConfig.Trigger))
                {
                    Dictionary<string, MacroKey> macro = chainConfig.macroEntries;
                    for (int i = 1; i <= macro.Count; i++)//Ensure to execute keys in Order
                    {
                        MacroKey macroKey = macro["in" + i + "mac" + chainConfig.id];
                        if (macroKey.Key != Key.None)
                        {
                            if (chainConfig.InstrumentKey != Key.None)
                            {
                                //Press instrument key if exists.
                                Keys instrumentKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.InstrumentKey.ToString());
                                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, instrumentKey, 0);
                                Thread.Sleep(30);
                            }

                            Keys thisk = (Keys)Enum.Parse(typeof(Keys), macroKey.Key.ToString());
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                            Thread.Sleep(macroKey.Delay); // Delay after key rather than before

                            if (chainConfig.DaggerKey != Key.None)
                            {
                                //Press instrument key if exists.
                                Keys daggerKey = (Keys)Enum.Parse(typeof(Keys), chainConfig.DaggerKey.ToString());
                                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, daggerKey, 0);
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
                this.thread = new ThreadRunner((_) => MacroExecutionThread(roClient));
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
