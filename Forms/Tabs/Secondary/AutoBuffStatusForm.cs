using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace _4RTools.Forms
{
    public partial class AutoBuffStatusForm : Form, IObserver
    {
        private List<BuffContainer> debuffContainers = new List<BuffContainer>();

        // Add controls for multiple status lists
        private Dictionary<string, TextBox> statusListTextBoxes = new Dictionary<string, TextBox>();

        public AutoBuffStatusForm(Subject subject)
        {
            InitializeComponent();
            debuffContainers.Add(new BuffContainer(this.DebuffsGP, Buff.GetDebuffs()));
            new DebuffRenderer(debuffContainers, toolTipPanacea).DoRender();

            // Setup the main Panacea textbox
            SetupPanaceaTextBox();

            // Setup additional status list textboxes (you'll need to add these to your form)
            SetupStatusListTextBoxes();

            subject.Attach(this);
        }

        private void SetupPanaceaTextBox()
        {
            txtPanaceaKey.KeyDown += FormUtils.OnKeyDown;
            txtPanaceaKey.KeyPress += FormUtils.OnKeyPress;
            txtPanaceaKey.TextChanged += (sender, e) => OnStatusListKeyChange("Panacea", sender, e);
            txtPanaceaKey.TextAlign = HorizontalAlignment.Center;
            statusListTextBoxes["Panacea"] = txtPanaceaKey;
        }

        private void SetupStatusListTextBoxes()
        {
            if (txtGreenPotionKey != null)
            {
                txtGreenPotionKey.KeyDown += FormUtils.OnKeyDown;
                txtGreenPotionKey.KeyPress += FormUtils.OnKeyPress;
                txtGreenPotionKey.TextChanged += (sender, e) => OnStatusListKeyChange("GreenPotion", sender, e);
                txtGreenPotionKey.TextAlign = HorizontalAlignment.Center;
                statusListTextBoxes["GreenPotion"] = txtGreenPotionKey;
            }

            if (txtRoyalJellyKey != null)
            {
                txtRoyalJellyKey.KeyDown += FormUtils.OnKeyDown;
                txtRoyalJellyKey.KeyPress += FormUtils.OnKeyPress;
                txtRoyalJellyKey.TextChanged += (sender, e) => OnStatusListKeyChange("RoyalJelly", sender, e);
                txtRoyalJellyKey.TextAlign = HorizontalAlignment.Center;
                statusListTextBoxes["RoyalJelly"] = txtRoyalJellyKey;
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    UpdateStatusListTextBoxes();
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

        private void UpdateStatusListTextBoxes()
        {
            var statusRecovery = ProfileSingleton.GetCurrent().StatusRecovery;

            foreach (var kvp in statusListTextBoxes)
            {
                string listName = kvp.Key;
                TextBox textBox = kvp.Value;

                Key key = statusRecovery.GetKeyForList(listName);
                textBox.Text = key != Key.None ? key.ToString() : AppConfig.TEXT_NONE;
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
                    txt.Text = AppConfig.TEXT_NONE;
                }
            }
        }

        // Generic method for handling status list key changes
        private void OnStatusListKeyChange(string listName, object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            Key k;
            // Safely parse the key, defaulting to None if invalid.
            if (!Enum.TryParse(textBox.Text, out k))
            {
                k = Key.None;
            }

            // Set the key for the specific list
            ProfileSingleton.GetCurrent().StatusRecovery.SetKeyForList(listName, k);

            // Save configuration (only saves the keys, not the predefined lists)
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().StatusRecovery);

            // Update the font style for the textbox
            if (k != Key.None)
            {
                textBox.Font = new System.Drawing.Font(textBox.Font, System.Drawing.FontStyle.Bold);
                textBox.ForeColor = AppConfig.ActiveKeyColor;
            }
            else
            {
                textBox.Font = new System.Drawing.Font(textBox.Font, System.Drawing.FontStyle.Regular);
                textBox.ForeColor = AppConfig.InactiveKeyColor;
            }

            this.ActiveControl = null;
        }

        // Legacy method for backward compatibility (if needed)
        private void OnPanaceaKeyChange(object sender, EventArgs e)
        {
            OnStatusListKeyChange("Panacea", sender, e);
        }

        private void AutoBuffStatusForm_Load(object sender, EventArgs e)
        {
        }
    }
}