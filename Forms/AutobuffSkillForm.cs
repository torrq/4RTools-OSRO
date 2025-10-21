using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.UI.Forms
{
    public partial class AutobuffSkillForm : Form, IObserver
    {

        private List<BuffContainer> skillContainers = new List<BuffContainer>();
        private Subject _subject; // Store the subject

        public AutobuffSkillForm(Subject subject)
        {
            this.KeyPreview = true;
            InitializeComponent();
            _subject = subject; // Store the subject
            InitializeSkillContainers(); //Extract this method

            RenderSkillBuffs();

            FormUtils.ApplyColorToButtons(this, new[] { "btnResetAutobuff" }, AppConfig.ResetButtonBackColor);
            //FormUtils.SetNumericUpDownMinimumDelays(this);

            subject.Attach(this);

        }
        private void InitializeSkillContainers()
        {
            skillContainers.Add(new BuffContainer(this.ArcherSkillsGP, Buff.GetArcherBuffs()));
            skillContainers.Add(new BuffContainer(this.SwordmanSkillGP, Buff.GetSwordmanBuffs()));
            skillContainers.Add(new BuffContainer(this.MageSkillGP, Buff.GetMageBuffs()));
            skillContainers.Add(new BuffContainer(this.MerchantSkillsGP, Buff.GetMerchantBuffs()));
            skillContainers.Add(new BuffContainer(this.ThiefSkillsGP, Buff.GetThiefBuffs()));
            skillContainers.Add(new BuffContainer(this.AcolyteSkillsGP, Buff.GetAcolyteBuffs()));
            skillContainers.Add(new BuffContainer(this.TKSkillGroupBox, Buff.GetTaekwonBuffs()));
            skillContainers.Add(new BuffContainer(this.NinjaSkillsGP, Buff.GetNinjaBuffs()));
            skillContainers.Add(new BuffContainer(this.GunsSkillsGP, Buff.GetGunsBuffs()));
            skillContainers.Add(new BuffContainer(this.PadawanSkillsGP, Buff.GetPadawanBuffs()));
        }

        private void RenderSkillBuffs()
        {
            new BuffRenderer(skillContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffSkill.ActionName, _subject).DoRender();
        }

        public void Update(ISubject subject)
        {
            if (subject == null) return;
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    if (ProfileSingleton.GetCurrent()?.AutobuffSkill != null)
                    {
                        BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
                        this.numericDelay.Value = ProfileSingleton.GetCurrent().AutobuffSkill.Delay;
                    }
                    else
                    {
                        // Consider what should happen if AutobuffSkill is null.  For now, clear the form.
                        FormUtils.ResetForm(this);
                    }
                    break;
                case MessageCode.TURN_OFF:
                    if (ProfileSingleton.GetCurrent()?.AutobuffSkill != null)
                        ProfileSingleton.GetCurrent().AutobuffSkill.Stop();
                    break;
                case MessageCode.TURN_ON:
                    if (ProfileSingleton.GetCurrent()?.AutobuffSkill != null)
                        ProfileSingleton.GetCurrent().AutobuffSkill.Start();
                    break;
            }
        }

        private void btnResetAutobuff_Click(object sender, EventArgs e)
        {
            ProfileSingleton.GetCurrent().AutobuffSkill.ClearKeyMapping();
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);
            BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
            this.numericDelay.Value = AppConfig.AutoBuffSkillsDefaultDelay;
        }

        private void numericDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutobuffSkill.Delay = Convert.ToInt16(this.numericDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);
                this.ActiveControl = null;
            }
            catch (Exception ex)
            {
                // Silently ignore conversion errors during typing
                if (!(ex is FormatException || ex is OverflowException))
                {
                    Console.WriteLine($"Error in AutobuffSkillForm delay change: {ex.Message}");
                }
            }
        }

        private void SkillAutoBuffForm_Load(object sender, EventArgs e)
        {

        }
    }
}