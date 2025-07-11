using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Model
{
    public class AutopotSP : IAction
    {
        public static string ACTION_NAME_AUTOPOT_SP = "AutopotSP";

        private static readonly int AUTOPOT_SP_ROWS = 5;

        // New data structure using a list of objects. This allows for reordering.
        public List<SPSlot> SPSlots { get; set; } = new List<SPSlot>();

        private int _delay = AppConfig.AutoPotDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoPotDefaultDelay : _delay;
            set => _delay = value;
        }

        public string ActionName { get; set; }
        private ThreadRunner thread;

        public AutopotSP() { }

        public AutopotSP(string actionName)
        {
            this.ActionName = actionName;
            InitializeSlots();
        }

        /// <summary>
        /// Creates the initial 5 SP slots.
        /// </summary>
        private void InitializeSlots()
        {
            if (this.SPSlots == null || this.SPSlots.Count == 0)
            {
                this.SPSlots = new List<SPSlot>();
                for (int i = 1; i <= AUTOPOT_SP_ROWS; i++)
                {
                    this.SPSlots.Add(new SPSlot { Id = i });
                }
            }
        }

        /// <summary>
        /// This method is called by Newtonsoft.Json after deserialization.
        /// It checks if the new HPSlots list is empty. If so, it means we're loading an old profile.
        /// It then migrates the data from the old flat properties to the new list structure.
        /// </summary>
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            if (SPSlots == null || SPSlots.Count == 0)
            {
                InitializeSlots(); // Make sure list is created
            }
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
                this.thread = new ThreadRunner(_ => AutopotThreadExecution(roClient));
                ThreadRunner.Start(this.thread);
            }
        }

        private int AutopotThreadExecution(Client roClient)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                ProcessSPHealing(roClient);
            }
            Thread.Sleep(this.Delay);
            return 0;
        }

        private void ProcessSPHealing(Client roClient)
        {

            // Check the global pot cooldown before attempting to use a pot.
            if (!PotManager.CanUsePot())
                return;

            // The healing logic now iterates through the SPSlots list.
            // Since the list is ordered by the user via drag-and-drop, the priority is automatically handled.
            foreach (var slot in SPSlots)
            {
                if (slot.Enabled && slot.SPPercent > 0 && roClient.IsSpBelow(slot.SPPercent))
                {
                    UsePot(slot.Key);
                    PotManager.RecordPotUsage(); // Notify the manager that a pot was used.
                    break; // Only use one pot per cycle
                }
            }
        }

        private void UsePot(Key key)
        {
            if (key == Key.None) return;

            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                var handle = ClientSingleton.GetClient().Process.MainWindowHandle;
                Interop.PostMessage(handle, Constants.WM_KEYDOWN_MSG_ID, k, 0);
                Interop.PostMessage(handle, Constants.WM_KEYUP_MSG_ID, k, 0);
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

        public string GetConfiguration()
        {
            // Now serializes the entire AutopotSP object, including the ordered SPSlots list.
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName() => ActionName ?? ACTION_NAME_AUTOPOT_SP;

    }
}
