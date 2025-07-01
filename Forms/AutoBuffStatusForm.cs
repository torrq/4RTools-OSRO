using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
namespace _4RTools.Forms
{
    public partial class AutoBuffStatusForm : Form, IObserver
    {
        private List<BuffContainer> debuffContainers = new List<BuffContainer>();
        public AutoBuffStatusForm(Subject subject)
        {
            InitializeComponent();
            debuffContainers.Add(new BuffContainer(this.DebuffsGP, Buff.GetDebuffs()));
            new DebuffRenderer(debuffContainers, toolTipPanacea).DoRender();
            txtPanaceaKey.KeyDown += FormUtils.OnKeyDown;
            txtPanaceaKey.KeyPress += FormUtils.OnKeyPress;
            txtPanaceaKey.TextChanged += OnPanaceaKeyChange;
            subject.Attach(this);
        }
        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    txtPanaceaKey.Text = ProfileSingleton.GetCurrent().StatusRecovery.buffMapping.Keys.Contains(EffectStatusIDs.SILENCE) ? ProfileSingleton.GetCurrent().StatusRecovery.buffMapping[EffectStatusIDs.SILENCE].ToString() : Keys.None.ToString();
                    UpdateAllDebuffs();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().DebuffsRecovery.Stop();
                    ProfileSingleton.GetCurrent().StatusRecovery.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().DebuffsRecovery.Start();
                    ProfileSingleton.GetCurrent().StatusRecovery.Start();
                    break;
            }
        }

        // Method to update all debuffs from both containers
        private void UpdateAllDebuffs()
        {
            UpdateDebuffs(DebuffsGP);
        }

        // Update regular debuffs
        private void UpdateDebuffs(GroupBox groupbox)
        {
            var autobuffDict = ProfileSingleton.GetCurrent().DebuffsRecovery.buffMapping;

            foreach (TextBox txt in groupbox.Controls.OfType<TextBox>())
            {
                var buffid = int.Parse(txt.Name.Split('n')[1]);
                var existe = autobuffDict.FirstOrDefault(x => x.Key.Equals((EffectStatusIDs)buffid));
                if (existe.Key != 0)
                {
                    txt.Text = autobuffDict[(EffectStatusIDs)buffid].ToString();
                }
                else
                {
                    txt.Text = "None";
                }
            }
        }

        // This method is for Panacea
        private void OnPanaceaKeyChange(object sender, EventArgs e)
        {
            Key k = (Key)Enum.Parse(typeof(Key), txtPanaceaKey.Text);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.POISON, k);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.SILENCE, k);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.BLIND, k);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.CONFUSION, k);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.HALLUCINATION, k);
            ProfileSingleton.GetCurrent().StatusRecovery.AddKeyToBuff(EffectStatusIDs.CURSE, k);
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().StatusRecovery);
            this.ActiveControl = null;
        }

        private void AutoBuffStatusForm_Load(object sender, EventArgs e)
        {
        }

    }
}