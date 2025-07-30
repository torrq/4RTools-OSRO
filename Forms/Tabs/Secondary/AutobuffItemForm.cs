using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Input;
using static _ORTools.Utils.FormHelper;

namespace _ORTools.Forms
{
    public partial class AutobuffItemForm : Form, IObserver
    {
        private List<BuffContainer> itemContainers = new List<BuffContainer>();
        private Subject _subject; // Store the subject

        // Static constructor to initialize BuffService
        static AutobuffItemForm()
        {
            BuffService.Initialize(new ResourceLoader(), new Logger());
        }

        public AutobuffItemForm(Subject subject)
        {
            InitializeComponent();
            _subject = subject; // Store the subject

            itemContainers.Add(new BuffContainer(this.PotionsGP, BuffService.GetPotionsBuffs()));
            itemContainers.Add(new BuffContainer(this.ElementalsGP, BuffService.GetElementBuffs()));
            itemContainers.Add(new BuffContainer(this.BoxesGP, BuffService.GetBoxBuffs()));
            itemContainers.Add(new BuffContainer(this.FoodsGP, BuffService.GetFoodBuffs()));
            itemContainers.Add(new BuffContainer(this.ScrollBuffsGP, BuffService.GetScrollBuffs()));
            itemContainers.Add(new BuffContainer(this.EtcGP, BuffService.GetEtcBuffs()));

            new BuffRenderer(itemContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffItem.ActionName, subject).DoRender();

            FormHelper.ApplyColorToButtons(this, new[] { "btnResetAutobuff" }, AppConfig.ResetButtonBackColor);

            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffItem.buffMapping), this);
                    this.numericDelay.Value = ProfileSingleton.GetCurrent().AutobuffItem.Delay;
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AutobuffItem.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AutobuffItem.Start();
                    break;
            }
        }

        private void btnResetAutobuff_Click(object sender, EventArgs e)
        {
            ProfileSingleton.GetCurrent().AutobuffItem.ClearKeyMapping();
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffItem);
            BuffRenderer.DoUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffItem.buffMapping), this);
            this.numericDelay.Value = AppConfig.AutoBuffItemsDefaultDelay;
        }

        private void numericDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutobuffItem.Delay = Convert.ToInt16(this.numericDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffItem);
            }
            catch { }
        }

        private void toolTipDelayReset_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
