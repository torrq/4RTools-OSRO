using _ORTools.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Model
{
    public class ConfigProfile : IAction
    {
        private readonly string ACTION_NAME = "UserPreferences";
        public string ToggleStateKey { get; set; } = Keys.End.ToString();
        public List<EffectStatusIDs> AutoBuffOrder { get; set; } = new List<EffectStatusIDs>();
        public bool StopBuffsCity { get; set; } = false;
        public bool SoundEnabled { get; set; } = false;
        public bool AutoOffOverweight { get; set; } = false;
        public Key AutoOffKey1 { get; set; }
        public Key AutoOffKey2 { get; set; }
        public bool AutoOffKillClient { get; set; } = false;
        public bool SwitchAmmo { get; set; } = false;
        public Key Ammo1Key { get; set; }
        public Key Ammo2Key { get; set; }
        public int AutoOffTime { get; set; } = 1;

        public ConfigProfile()
        {
        }

        public void Start() { }

        public void Stop() { }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return ACTION_NAME;
        }
        public void SetAutoBuffOrder(List<EffectStatusIDs> buffs)
        {
            this.AutoBuffOrder = buffs;
        }
    }
}
