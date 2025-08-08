using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using static _ORTools.Utils.FormHelper;

namespace _ORTools.Forms
{
    public class ResourceLoader : IResourceLoader
    {
        public Bitmap LoadIcon(string iconName)
        {
            return Resources.Media.Icons.ResourceManager.GetObject(iconName) as Bitmap;
        }
    }

    public class Logger : ILogger
    {
        public void Error(string message) => DebugLogger.Error(message);
        public void Error(Exception ex, string message) => DebugLogger.Error(ex, message);
        public void Warn(string message) => DebugLogger.Error(message); // Using Error for Warn as a fallback
    }

    public partial class AutobuffSkillForm : Form, IObserver
    {
        private List<BuffContainer> skillContainers = new List<BuffContainer>();
        private Subject _subject; // Store the subject

        // Static constructor to initialize BuffService
        static AutobuffSkillForm()
        {
            BuffService.Initialize(new ResourceLoader(), new Logger());
        }

        public AutobuffSkillForm(Subject subject)
        {
            this.KeyPreview = true;
            InitializeComponent();
            _subject = subject; // Store the subject
            InitializeSkillContainers(); // Extract this method

            RenderSkillBuffs();

            FormHelper.ApplyColorToButtons(this, new[] { "btnResetAutobuff" }, AppConfig.ResetButtonBackColor);
            //FormUtils.SetNumericUpDownMinimumDelays(this);

            subject.Attach(this);
        }

        private void InitializeSkillContainers()
        {
            skillContainers.Add(new BuffContainer(this.ArcherSkillsGP, BuffService.GetArcherBuffs()));
            skillContainers.Add(new BuffContainer(this.SwordmanSkillGP, BuffService.GetSwordmanBuffs()));
            skillContainers.Add(new BuffContainer(this.MageSkillGP, BuffService.GetMageBuffs()));
            skillContainers.Add(new BuffContainer(this.MerchantSkillsGP, BuffService.GetMerchantBuffs()));
            skillContainers.Add(new BuffContainer(this.ThiefSkillsGP, BuffService.GetThiefBuffs()));
            skillContainers.Add(new BuffContainer(this.AcolyteSkillsGP, BuffService.GetAcolyteBuffs()));
            skillContainers.Add(new BuffContainer(this.TKSkillGroupBox, BuffService.GetTaekwonBuffs()));
            skillContainers.Add(new BuffContainer(this.NinjaSkillsGP, BuffService.GetNinjaBuffs()));
            skillContainers.Add(new BuffContainer(this.GunsSkillsGP, BuffService.GetGunslingerBuffs()));
            skillContainers.Add(new BuffContainer(this.PadawanSkillsGP, BuffService.GetPadawanBuffs()));
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
                        BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Keys>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
                        this.numericDelay.Value = ProfileSingleton.GetCurrent().AutobuffSkill.Delay;
                    }
                    else
                    {
                        // Consider what should happen if AutobuffSkill is null.  For now, clear the form.
                        FormHelper.ResetForm(this);
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
            BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Keys>(ProfileSingleton.GetCurrent().AutobuffSkill.buffMapping), this);
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
            catch { }
        }
    }
}