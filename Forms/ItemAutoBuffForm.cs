using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class ItemAutoBuffForm : Form, IObserver
    {
        private List<BuffContainer> itemContainers = new List<BuffContainer>();

        public ItemAutoBuffForm(Subject subject)
        {
            InitializeComponent();

            itemContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            itemContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            itemContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            itemContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            itemContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            itemContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));

            new BuffRenderer(itemContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffItem.ActionName, subject).DoRender();

            FormUtils.ApplyColorToButtons(this, new[] { "btnResetAutobuff" }, AppConfig.ResetButtonBackColor);
            //FormUtils.SetNumericUpDownMinimumDelays(this);

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

    }
}
