using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static _ORTools.Utils.FormHelper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace _ORTools.Forms
{
    public partial class DebuffForm : Form, IObserver
    {
        private List<BuffContainer> debuffContainers = new List<BuffContainer>();
        private Dictionary<string, TextBox> statusListTextBoxes = new Dictionary<string, TextBox>();

        // Static constructor to initialize BuffService
        static DebuffForm()
        {
            BuffService.Initialize(new ResourceLoader(), new Logger());
        }

        public DebuffForm(Subject subject)
        {
            InitializeComponent();
            debuffContainers.Add(new BuffContainer(this.DebuffsGP, BuffService.GetDebuffs()));
            new DebuffRenderer(debuffContainers, toolTipPanacea).DoRender();

            // Setup status list textboxes
            SetupStatusListTextBoxes();

            subject.Attach(this);
        }

        private void SetupStatusListTextBoxes()
        {
            if (txtPanaceaKey != null)
            {
                txtPanaceaKey.KeyDown += FormHelper.OnKeyDown;
                txtPanaceaKey.KeyPress += FormHelper.OnKeyPress;
                txtPanaceaKey.TextChanged += (sender, e) => OnStatusListKeyChange("Panacea", sender, e);
                txtPanaceaKey.TextAlign = HorizontalAlignment.Center;
                statusListTextBoxes["Panacea"] = txtPanaceaKey;
            }

            if (txtGreenPotionKey != null)
            {
                txtGreenPotionKey.KeyDown += FormHelper.OnKeyDown;
                txtGreenPotionKey.KeyPress += FormHelper.OnKeyPress;
                txtGreenPotionKey.TextChanged += (sender, e) => OnStatusListKeyChange("GreenPotion", sender, e);
                txtGreenPotionKey.TextAlign = HorizontalAlignment.Center;
                statusListTextBoxes["GreenPotion"] = txtGreenPotionKey;
            }

            if (txtRoyalJellyKey != null)
            {
                txtRoyalJellyKey.KeyDown += FormHelper.OnKeyDown;
                txtRoyalJellyKey.KeyPress += FormHelper.OnKeyPress;
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

                Keys key = statusRecovery.GetKeyForList(listName);
                textBox.Text = key != Keys.None ? key.ToString() : AppConfig.TEXT_NONE;
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

            Keys k;
            // Safely parse the key, defaulting to None if invalid.
            if (!Enum.TryParse(textBox.Text, out k))
            {
                k = Keys.None;
            }

            // Set the key for the specific list
            ProfileSingleton.GetCurrent().StatusRecovery.SetKeyForList(listName, k);

            // Save configuration (only saves the keys, not the predefined lists)
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().StatusRecovery);

            // Update the font style for the textbox
            if (k != Keys.None)
            {
                FormHelper.ApplyInputKeyStyle(textBox, true);
            } else {
                FormHelper.ApplyInputKeyStyle(textBox, false);
            }

            this.ActiveControl = null;
        }

        private void AutoBuffStatusForm_Load(object sender, EventArgs e)
        {
        }
    }
}