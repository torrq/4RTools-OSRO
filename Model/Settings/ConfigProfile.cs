using _ORTools.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public class ConfigProfile : IAction
    {
        public enum OverweightAutoOffMode
        {
            Weight50 = 50,
            Weight90 = 90
        }

        private readonly string ACTION_NAME = "UserPreferences";
        public decimal ConfigVersion { get; set; }
        public string ToggleStateKey { get; set; } = ConfigGlobal.GetConfig().DefaultToggleStateKey;
        public List<EffectStatusIDs> AutoBuffOrder { get; set; } = new List<EffectStatusIDs>();
        public bool StopBuffsCity { get; set; } = false;
        public bool SoundEnabled { get; set; } = false;
        public bool AutoOffOverweight { get; set; } = false;
        public OverweightAutoOffMode AutoOffOverweightMode { get; set; } = OverweightAutoOffMode.Weight90;
        public Keys AutoOffKey1 { get; set; }
        public Keys AutoOffKey2 { get; set; }
        public bool AutoOffKillClient { get; set; } = false;
        public bool SwitchAmmo { get; set; } = false;
        public Keys Ammo1Key { get; set; }
        public Keys Ammo2Key { get; set; }
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
