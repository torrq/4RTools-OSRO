using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using _4RTools.Utils;
using _4RTools.Model;
using System.Linq;
using static _4RTools.Model.AutoSwitch;
using System.Diagnostics;

namespace _4RTools.Forms
{
    public partial class AutoSwitchForm : Form, IObserver
    {
        private List<Buff> allBuffs = new List<Buff>();
        private Subject _subject;
        string OldTextKey = string.Empty;
        class ComboboxValue
        {
            public int Id { get; private set; }
            public string Name { get; private set; }

            public ComboboxValue(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public AutoSwitchForm(Subject subject)
        {
            InitializeComponent();

            setupInputs();
            _subject = subject;

            this.allBuffs.AddRange(Buff.GetArcherSkills());
            this.allBuffs.AddRange(Buff.GetSwordmanSkill());
            this.allBuffs.AddRange(Buff.GetMageSkills());
            this.allBuffs.AddRange(Buff.GetMerchantSkills());
            this.allBuffs.AddRange(Buff.GetThiefSkills());
            this.allBuffs.AddRange(Buff.GetAcolyteSkills());
            this.allBuffs.AddRange(Buff.GetTaekwonSkills());
            this.allBuffs.AddRange(Buff.GetNinjaSkills());
            this.allBuffs.AddRange(Buff.GetGunsSkills());
            this.allBuffs.AddRange(Buff.GetPadawanSkills());

            foreach (var skill in this.allBuffs.OrderBy(o => o.name).ToList())
            {
                this.skillCB.Items.Add(skill.name);
            }

            subject.Attach(this);
        }

        private void setupInputs()
        {
            GroupBox group = (GroupBox)this.Controls.Find("ProcSwitchGP", true)[0];

            foreach (Control c in group.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.TextChanged += new EventHandler(this.onTextChange);
                    textBox.GotFocus += new EventHandler(this.onFocus);
                }
            }
        }

        private void loadCustomSkills(Subject subject)
        {
            List<Buff> filteredBuffs = new List<Buff>();
            foreach (var skill in ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping)
            {
                if (this.allBuffs.Exists(x => x.effectStatusID == skill.skillId))
                {
                    filteredBuffs.Add(this.allBuffs.FirstOrDefault(x => x.effectStatusID == skill.skillId));
                }
            }

            AutoSwitchContainer skillContainer = new AutoSwitchContainer();
            skillContainer = new AutoSwitchContainer(this.AutoSwitchGP, filteredBuffs);
            new AutoSwitchRenderer(skillContainer, toolTip1, subject, this).doRender();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    loadExclusiveSkills();
                    loadCustomSkills(subject as Subject);
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AutoSwitch.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AutoSwitch.Start();
                    break;
                case MessageCode.CHANGED_AUTOSWITCH_SKILL:
                    loadCustomSkills(subject as Subject);
                    break;
            }
        }

        private void loadExclusiveSkills()
        {
            var _autoSwitchMapping = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchMapping;

            GroupBox group = (GroupBox)this.Controls.Find("ProcSwitchGP", true)[0];

            foreach (Control c in group.Controls)
            {
                if (c is TextBox)
                {
                    string type = c.Name.Split(new[] { "in" }, StringSplitOptions.None)[0];
                    EffectStatusIDs statID = (EffectStatusIDs)Int16.Parse(c.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    switch (type)
                    {
                        case item:
                            c.Text = _autoSwitchMapping.Exists(x => x.skillId == statID) ? _autoSwitchMapping.FirstOrDefault(x => x.skillId == statID).itemKey.ToString() : Keys.None.ToString();
                            break;

                        case nextItem:
                            c.Text = _autoSwitchMapping.Exists(x => x.skillId == statID) ? _autoSwitchMapping.FirstOrDefault(x => x.skillId == statID).nextItemKey.ToString() : Keys.None.ToString();
                            break;
                    }
                }
            }
            Control[] cDelay = this.Controls.Find("numDelay", true);
            if (cDelay.Length > 0)
            {
                NumericUpDown numeric = (NumericUpDown)cDelay[0];
                numeric.Value = Convert.ToInt16(ProfileSingleton.GetCurrent().AutoSwitch.delay);
            }

            Control[] cSwitchDelay = this.Controls.Find("numSwitchDelay", true);
            if (cSwitchDelay.Length > 0)
            {
                NumericUpDown numeric = (NumericUpDown)cSwitchDelay[0];
                numeric.Value = Convert.ToInt16(ProfileSingleton.GetCurrent().AutoSwitch.switchEquipDelay);
            }
        }
        private void onFocus(object sender, EventArgs e)
        { 
            TextBox txtBox = (TextBox)sender;
            OldTextKey = txtBox.Text;
        }

        private void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                bool textChanged = this.OldTextKey != String.Empty && this.OldTextKey != txtBox.Text.ToString();
                if ((txtBox.Text.ToString() != String.Empty) && textChanged)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    EffectStatusIDs statusID = (EffectStatusIDs)Int16.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    string type = txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[0];

                    AutoSwitchConfig config = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchMapping.Find(cfg => cfg.skillId == statusID);
                    if (config != null)
                    {
                        config.skillId = statusID;

                        switch (type)
                        {
                            case item:
                                config.itemKey = key;
                                break;

                            case skill:
                                config.skillKey = key;
                                break;

                            case nextItem:
                                config.nextItemKey = key;
                                break;
                        }
                        
                    }
                    else
                    {

                        ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchMapping.Add(new AutoSwitchConfig(statusID, key, type));
                    }

                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
                    _subject.Notify(new Utils.Message(Utils.MessageCode.ADDED_NEW_AUTOSWITCH_PETS, null));
                }
                this.ActiveControl = null;
            }
            catch { }
        }

        private void btnNewSwitch(object sender, EventArgs e)
        {
            var txtSkill = skillCB.Text;

            var skill = this.allBuffs.FirstOrDefault(x => x.name == txtSkill);
            var _autoSwitchGenericMapping = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping;
            if (!_autoSwitchGenericMapping.Exists(x => x.skillId == skill.effectStatusID))
            {
                _autoSwitchGenericMapping.Add(new AutoSwitchConfig(skill.effectStatusID, Key.None));
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
                _subject.Notify(new Utils.Message(Utils.MessageCode.CHANGED_AUTOSWITCH_SKILL, null));
            }


        }

        private void txtDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutoSwitch.delay = Convert.ToInt16(this.numDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
            }
            catch { }
        }

        private void txtSwitchDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutoSwitch.switchEquipDelay = Convert.ToInt16(this.numSwitchDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
            }
            catch { }
        }

    }
}
