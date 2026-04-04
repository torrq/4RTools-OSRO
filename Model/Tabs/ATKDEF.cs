using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public enum ATKDEFEnum
    {
        ATK,
        DEF,
    }

    public class EquipConfig
    {
        public int id;

        private int _keySpammerDelay = AppConfig.ATKDEFSpammerDefaultDelay;

        public int KeySpammerDelay
        {
            get => _keySpammerDelay <= 0 ? AppConfig.ATKDEFSpammerDefaultDelay : _keySpammerDelay;
            set => _keySpammerDelay = value;
        }

        private int _switchDelay = AppConfig.ATKDEFSwitchDefaultDelay;

        public int SwitchDelay
        {
            get => _switchDelay <= 0 ? AppConfig.ATKDEFSwitchDefaultDelay : _switchDelay;
            set => _switchDelay = value;
        }

        public Keys KeySpammer { get; set; }
        public bool KeySpammerWithClick { get; set; } = true;
        public Dictionary<string, Keys> DefKeys { get; set; } = new Dictionary<string, Keys>();
        public Dictionary<string, Keys> AtkKeys { get; set; } = new Dictionary<string, Keys>();

        public EquipConfig()
        { }

        public EquipConfig(int id)
        {
            this.id = id;
        }

        public EquipConfig(EquipConfig macro)
        {
            this.id = macro.id;
            this.KeySpammerDelay = macro.KeySpammerDelay;
            this.SwitchDelay = macro.SwitchDelay;
            this.KeySpammer = macro.KeySpammer;
            this.KeySpammerWithClick = macro.KeySpammerWithClick;
            this.DefKeys = new Dictionary<string, Keys>(macro.DefKeys);
            this.AtkKeys = new Dictionary<string, Keys>(macro.AtkKeys);
        }

        public EquipConfig(int id, Keys trigger) : this(id)
        {
            this.KeySpammer = trigger;
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
                EquipConfigs.Add(new EquipConfig(i, Keys.None));
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
                    this.thread.Terminate();
                    this.thread = null;
                }
                this.thread = new ThreadRunner(_ => ATKDEFThread(roClient), "ATKDEF") { IterationDelay = 1 };
                ThreadRunner.Start(this.thread);
            }
        }

        private int ATKDEFThread(Client roClient)
        {
            if (roClient.IsTextInputActive() || roClient.IsDead()) return 0;
            if (!roClient.IsProcessRunning()) return 0;

            IntPtr hWnd = roClient.MainWindowHandle;
            if (hWnd == IntPtr.Zero) return 0;

            if (Win32Interop.GetForegroundWindow() != hWnd) return 0;

            List<EquipConfig> currentConfigs;
            lock (this.EquipConfigs)
            {
                currentConfigs = this.EquipConfigs.ToList();
            }

            try
            {
                foreach (EquipConfig equipConfig in currentConfigs)
                {
                    bool equipAtkItems = false;
                    bool equipDefItems = false;
                    bool ammo = false;

                    if (equipConfig.KeySpammer != Keys.None && Win32Interop.IsKeyPressed(equipConfig.KeySpammer)
                        && !Win32Interop.IsKeyPressed(Keys.LMenu) && !Win32Interop.IsKeyPressed(Keys.RMenu))
                    {
                        Keys thisk = equipConfig.KeySpammer;

                        while (Win32Interop.IsKeyPressed(equipConfig.KeySpammer))
                        {
                            if (Win32Interop.GetForegroundWindow() != hWnd)
                            {
                                break;
                            }

                            if (!equipAtkItems)
                            {
                                List<Keys> atkKeys;
                                lock (this.EquipConfigs)
                                {
                                    atkKeys = equipConfig.AtkKeys.Values.ToList();
                                }
                                foreach (Keys key in atkKeys)
                                {
                                    Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, key, 0); //Equip ATK Items
                                    Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, key, 0);
                                    Thread.Sleep(equipConfig.SwitchDelay);
                                }
                                equipAtkItems = true;
                            }

                            if (equipConfig.KeySpammerWithClick)
                            {
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, thisk, 0);
                                Win32Interop.PostMessage(hWnd, Constants.WM_LBUTTONDOWN, 0, 0);
                                AutoSwitchAmmo(roClient, ref ammo, hWnd);
                                Thread.Sleep(1);
                                Win32Interop.PostMessage(hWnd, Constants.WM_LBUTTONUP, 0, 0);
                                Thread.Sleep(equipConfig.KeySpammerDelay);
                            }
                            else
                            {
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, thisk, 0);
                                Thread.Sleep(equipConfig.KeySpammerDelay);
                            }
                        }

                        if (equipConfig.KeySpammerWithClick)
                        {
                            Win32Interop.PostMessage(hWnd, Constants.WM_LBUTTONDOWN, 0, 0);
                            Thread.Sleep(1);
                            Win32Interop.PostMessage(hWnd, Constants.WM_LBUTTONUP, 0, 0);
                        }

                        if (!equipDefItems)
                        {
                            List<Keys> defKeys;
                            lock (this.EquipConfigs)
                            {
                                defKeys = equipConfig.DefKeys.Values.ToList();
                            }
                            foreach (Keys key in defKeys)
                            {
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, key, 0); //Equip DEF Items
                                Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, key, 0);
                                Thread.Sleep(equipConfig.SwitchDelay);
                            }
                            equipDefItems = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ATKDEFThread] Exception: {ex.Message}");
            }

            return 0;
        }

        private void AutoSwitchAmmo(Client roClient, ref bool ammo, IntPtr hWnd)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (prefs.SwitchAmmo)
            {
                if (prefs.Ammo1Key != Keys.None && prefs.Ammo2Key != Keys.None)
                {
                    if (!ammo)
                    {
                        Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, prefs.Ammo1Key, 0);
                        Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, prefs.Ammo1Key, 0);
                        ammo = true;
                    }
                    else
                    {
                        Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, prefs.Ammo2Key, 0);
                        Win32Interop.PostMessage(hWnd, Constants.WM_KEYUP_MSG_ID, prefs.Ammo2Key, 0);
                        ammo = false;
                    }
                }
            }
        }

        public void AddSwitchItem(int id, string dictKey, Keys k, string type)
        {
            lock (this.EquipConfigs)
            {
                var equips = EquipConfigs.FirstOrDefault(x => x.id == id);
                if (equips == null) return;

                bool isDef = string.Equals(type, ATKDEFEnum.DEF.ToString(), StringComparison.OrdinalIgnoreCase);
                Dictionary<string, Keys> copy = isDef ? equips.DefKeys : equips.AtkKeys;

                if (copy.ContainsKey(dictKey))
                {
                    copy.Remove(dictKey);
                }

                if (k != Keys.None)
                {
                    copy.Add(dictKey, k);
                }
            }
        }

        public void RemoveSwitchEntry(int id, string dictKey, string type)
        {
            lock (this.EquipConfigs)
            {
                var equips = EquipConfigs.FirstOrDefault(x => x.id == id);
                if (equips == null) return;

                bool isDef = string.Equals(type, ATKDEFEnum.DEF.ToString(), StringComparison.OrdinalIgnoreCase);
                Dictionary<string, Keys> copy = isDef ? equips.DefKeys : equips.AtkKeys;

                copy.Remove(dictKey);
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