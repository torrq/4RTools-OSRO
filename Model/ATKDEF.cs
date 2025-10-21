using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public enum ATKDEFEnum
    {
        ATK,
        DEF,
    }

    public class EquipConfig
    {
        public int id;

        private int _AHKDelay = AppConfig.ATKDEFSpammerDefaultDelay;

        public int AHKDelay
        {
            get => _AHKDelay <= 0 ? AppConfig.ATKDEFSpammerDefaultDelay : _AHKDelay;
            set => _AHKDelay = value;
        }

        private int _switchdelay = AppConfig.ATKDEFSwitchDefaultDelay;

        public int SwitchDelay
        {
            get => _switchdelay <= 0 ? AppConfig.ATKDEFSwitchDefaultDelay : _switchdelay;
            set => _switchdelay = value;
        }

        public Key KeySpammer { get; set; }
        public bool KeySpammerWithClick { get; set; } = true;
        public Dictionary<string, Key> DefKeys { get; set; } = new Dictionary<string, Key>();
        public Dictionary<string, Key> AtkKeys { get; set; } = new Dictionary<string, Key>();

        public EquipConfig()
        { }

        public EquipConfig(int id)
        {
            this.id = id;
            this.DefKeys = new Dictionary<string, Key>();
            this.AtkKeys = new Dictionary<string, Key>();
        }

        public EquipConfig(EquipConfig macro)
        {
            this.id = macro.id;
            this.AHKDelay = macro.AHKDelay;
            this.SwitchDelay = macro.SwitchDelay;
            this.KeySpammer = macro.KeySpammer;
            this.KeySpammerWithClick = macro.KeySpammerWithClick;
            this.DefKeys = new Dictionary<string, Key>(macro.DefKeys);
            this.AtkKeys = new Dictionary<string, Key>(macro.AtkKeys);
        }

        public EquipConfig(int id, Key trigger)
        {
            this.id = id;
            this.KeySpammer = trigger;
            this.DefKeys = new Dictionary<string, Key>();
            this.AtkKeys = new Dictionary<string, Key>();
        }
    }

    public class ATKDEF : IAction
    {
        public static string ACTION_NAME_ATKDEF = "ATKDEFMode";
        private ThreadRunner thread;
        public List<EquipConfig> EquipConfigs { get; set; } = new List<EquipConfig>();

        public ATKDEF(int macroLanes)
        {
            for (int i = 1; i <= macroLanes; i++)
            {
                EquipConfigs.Add(new EquipConfig(i, Key.None));
            }
        }

        public string GetActionName()
        {
            return ACTION_NAME_ATKDEF;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
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
                this.thread = new ThreadRunner(_ => AHKThreadExecution(roClient));
                ThreadRunner.Start(this.thread);
            }
        }

        private int AHKThreadExecution(Client roClient)
        {
            foreach (EquipConfig equipConfig in this.EquipConfigs)
            {
                bool equipAtkItems = false;
                bool equipDefItems = false;
                bool ammo = false;

                if (equipConfig.KeySpammer != Key.None && Keyboard.IsKeyDown(equipConfig.KeySpammer)
                    && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                {
                    Keys thisk = toKeys(equipConfig.KeySpammer);

                    while (Keyboard.IsKeyDown(equipConfig.KeySpammer))
                    {
                        if (!equipAtkItems)
                        {
                            foreach (Key key in equipConfig.AtkKeys.Values)
                            {
                                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0); //Equip ATK Items
                                Thread.Sleep(equipConfig.SwitchDelay);
                            }
                            equipAtkItems = true;
                        }

                        if (equipConfig.KeySpammerWithClick)
                        {
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                            AutoSwitchAmmo(roClient, ref ammo);
                            Thread.Sleep(1);
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                            Thread.Sleep(equipConfig.AHKDelay);
                        }
                        else
                        {
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                            Thread.Sleep(equipConfig.AHKDelay);
                        }
                    }
                    if (!equipDefItems)
                    {
                        foreach (Key key in equipConfig.DefKeys.Values)
                        {
                            Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0); //Equip DEF Items
                            Thread.Sleep(equipConfig.SwitchDelay);
                        }
                        equipDefItems = true;
                    }
                }
            }
            return 0;
        }

        private void AutoSwitchAmmo(Client roClient, ref bool ammo)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (prefs.SwitchAmmo)
            {
                if (prefs.Ammo1Key.ToString() != string.Empty && prefs.Ammo2Key.ToString() != string.Empty)
                {
                    if (ammo == false)
                    {
                        Key key = prefs.Ammo1Key;
                        Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0);
                        ammo = true;
                    }
                    else
                    {
                        Key key = prefs.Ammo2Key;
                        Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, toKeys(key), 0);
                        ammo = false;
                    }
                }
            }
        }

        public void AddSwitchItem(int id, string dictKey, Key k, string type)
        {
            var equips = EquipConfigs.FirstOrDefault(x => x.id == id);

            Dictionary<string, Key> copy = type == "DEF" ? equips.DefKeys : equips.AtkKeys;

            if (copy.ContainsKey(dictKey))
            {
                RemoveSwitchEntry(id, dictKey, type);
            }

            if (k != Key.None)
            {
                copy.Add(dictKey, k);
            }
        }

        public void RemoveSwitchEntry(int id, string dictKey, string type)
        {
            var equips = EquipConfigs.FirstOrDefault(x => x.id == id);
            Dictionary<string, Key> copy = type == "DEF" ? equips.DefKeys : equips.AtkKeys;
            copy.Remove(dictKey);
        }

        private Keys toKeys(Key k)
        {
            return (Keys)Enum.Parse(typeof(Keys), k.ToString());
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