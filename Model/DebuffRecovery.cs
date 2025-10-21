using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
namespace BruteGamingMacros.Core.Model
{
    public class DebuffRecovery : IAction
    {
        public static string ACTION_NAME_DEBUFF_RECOVERY = "DebuffsRecovery";
        public static string ACTION_NAME_WEIGHT_DEBUFF_RECOVERY = "WeightDebuffsRecovery";

        private ThreadRunner thread;
        public Dictionary<EffectStatusIDs, Key> buffMapping = new Dictionary<EffectStatusIDs, Key>();
        public int Delay { get; set; } = 1;

        private readonly string ActionName;

        // Default constructor
        public DebuffRecovery() : this(ACTION_NAME_DEBUFF_RECOVERY)
        {
        }

        // Constructor with custom action name
        public DebuffRecovery(string actionName)
        {
            this.ActionName = actionName;
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public ThreadRunner RestoreStatusThread(Client c)
        {
            Client roClient = ClientSingleton.GetClient();
            ThreadRunner statusEffectsThread = new ThreadRunner(_ =>
            {
                for (int i = 0; i <= Constants.MAX_BUFF_LIST_INDEX_SIZE - 1; i++)
                {
                    uint currentStatus = c.CurrentBuffStatusCode(i);
                    if (currentStatus == uint.MaxValue) { continue; }
                    EffectStatusIDs status = (EffectStatusIDs)currentStatus;
                    if (buffMapping.ContainsKey((EffectStatusIDs)currentStatus)) //IF FOR REMOVE STATUS - CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                    {
                        //IF CONTAINS CURRENT STATUS ON DICT
                        Key key = buffMapping[(EffectStatusIDs)currentStatus];
                        if (Enum.IsDefined(typeof(EffectStatusIDs), currentStatus))
                        {
                            this.UseStatusRecovery(key);
                        }
                    }
                }
                Thread.Sleep(this.Delay);
                return 0;
            })
            {

            };
            return statusEffectsThread;
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
                this.thread = RestoreStatusThread(roClient);
                ThreadRunner.Start(this.thread);
            }
        }

        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }
            if (FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
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

        private void UseStatusRecovery(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Interop.PostMessage(ClientSingleton.GetClient().Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
            }
        }

    }
}