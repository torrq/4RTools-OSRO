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
    public class SkillTimerKey
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

        /// <summary>
        /// Constructor used by Newtonsoft.Json for deserialization.
        /// This allows loading profiles that may or may not contain the click-related properties.
        /// </summary>
        [JsonConstructor]
        public SkillTimerKey(Keys key, int delay)
        {
            this.Key = key;
            this.Delay = delay;
        }
    }
}