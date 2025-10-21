using BruteGamingMacros.UI.Forms;
using BruteGamingMacros.Properties;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static BruteGamingMacros.Core.Model.AutoSwitch;

namespace BruteGamingMacros.Core.Model
{
    internal class AutoSwitchContainer
    {
        public GroupBox container { get; set; }
        public List<Buff> skills { get; set; }

        public AutoSwitchContainer() { }

        public AutoSwitchContainer(GroupBox p, List<Buff> skills)
        {
            this.skills = skills;
            this.container = p;
        }
    }

    internal class AutoSwitchRenderer
    {

//        private readonly int BUFFS_PER_ROW = 1;
//        private readonly int DISTANCE_BETWEEN_CONTAINERS = 10;
        private readonly int DISTANCE_BETWEEN_ROWS = 30;

        private AutoSwitchContainer _container;
        private ToolTip _toolTip;
        private Subject _subject;
        private Form _forms;
        string OldText = string.Empty;

        public AutoSwitchRenderer(AutoSwitchContainer container, ToolTip toolTip, Subject subject, Form forms)
        {
            this._container = container;
            this._toolTip = toolTip;
            this._subject = subject;
            this._forms = forms;
        }

        public void doRender()
        {
            GroupBox group = (GroupBox)this._forms.Controls.Find("AutoSwitchGP", true)[0];
            group.Controls.Clear();
            AutoSwitchContainer bk = _container;
            Point lastLocation = new Point(10, 30);
            headerLabels(bk);

            foreach (Buff skill in bk.skills)
            {
                AutoSwitchConfig config = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping.Find(cfg => cfg.skillId == skill.effectStatusID);
                PictureBox pb = new PictureBox();
                PictureBox seta1pb = new PictureBox();
                PictureBox seta2pb = new PictureBox();
                PictureBox removepb = new PictureBox();
                TextBox itemtb = new TextBox();
                TextBox skilltb = new TextBox();
                TextBox nextItemtb = new TextBox();

                pb.Image = skill.icon;
                pb.BackgroundImageLayout = ImageLayout.Center;
                pb.Location = new Point(lastLocation.X, lastLocation.Y);
                pb.Name = "pbox" + ((int)skill.effectStatusID);
                pb.Size = new Size(26, 26);
                _toolTip.SetToolTip(pb, skill.name);

                itemtb.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                itemtb.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                itemtb.GotFocus += new EventHandler(textBox_GotFocus);
                itemtb.TextChanged += new EventHandler(onTextChange);
                itemtb.Size = new Size(55, 20);
                itemtb.Tag = "ITEMin" + ((int)skill.effectStatusID);
                itemtb.Name = "ITEMin" + ((int)skill.effectStatusID);
                itemtb.Location = new Point(pb.Location.X + 30, pb.Location.Y + 3);
                itemtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
                itemtb.ForeColor = System.Drawing.Color.White;
                itemtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                itemtb.Text = config.itemKey != 0 ? config.itemKey.ToString() : Key.None.ToString();

                seta1pb.Image = BruteGamingMacros.Resources.BruteGaming.ETCResource.arrowright;
                seta1pb.Location = new Point(itemtb.Location.X + 60, itemtb.Location.Y + 3);
                seta1pb.Name = "item" + ((int)skill.effectStatusID);
                seta1pb.Size = new System.Drawing.Size(19, 11);
                seta1pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                seta1pb.TabIndex = 296;
                seta1pb.TabStop = false;

                skilltb.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                skilltb.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                skilltb.GotFocus += new EventHandler(textBox_GotFocus);
                skilltb.TextChanged += new EventHandler(onTextChange);
                skilltb.Size = new Size(55, 20);
                skilltb.Tag = "SKILLin" + ((int)skill.effectStatusID);
                skilltb.Name = "SKILLin" + ((int)skill.effectStatusID);
                skilltb.Location = new Point(seta1pb.Location.X + 25, seta1pb.Location.Y - 3);
                skilltb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
                skilltb.ForeColor = System.Drawing.Color.White;
                skilltb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                skilltb.Text = config.itemKey != 0 ? config.skillKey.ToString() : Key.None.ToString();

                seta2pb.Image = BruteGamingMacros.Resources.BruteGaming.ETCResource.arrowright;
                seta2pb.Location = new Point(skilltb.Location.X + 60, skilltb.Location.Y + 3);
                seta2pb.Name = "pboxSeta2" + ((int)skill.effectStatusID);
                seta2pb.Size = new System.Drawing.Size(19, 11);
                seta2pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                seta2pb.TabIndex = 296;
                seta2pb.TabStop = false;

                nextItemtb.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                nextItemtb.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                nextItemtb.GotFocus += new EventHandler(textBox_GotFocus);
                nextItemtb.TextChanged += new EventHandler(onTextChange);
                nextItemtb.Size = new Size(55, 20);
                nextItemtb.Tag = "NEXTITEMin" + ((int)skill.effectStatusID);
                nextItemtb.Name = "NEXTITEMin" + ((int)skill.effectStatusID);
                nextItemtb.Location = new Point(seta2pb.Location.X + 25, seta2pb.Location.Y - 3);
                nextItemtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
                nextItemtb.ForeColor = System.Drawing.Color.White;
                nextItemtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                nextItemtb.Text = config.itemKey != 0 ? config.nextItemKey.ToString() : Key.None.ToString();

                removepb.Image = BruteGamingMacros.Resources.BruteGaming.ETCResource.remove;
                removepb.Location = new Point(nextItemtb.Location.X + 60, nextItemtb.Location.Y);
                removepb.Name = "remove" + ((int)skill.effectStatusID);
                removepb.Size = new System.Drawing.Size(20, 20);
                removepb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                removepb.Click += new EventHandler(removeSkill);

                bk.container.Controls.Add(pb);
                bk.container.Controls.Add(itemtb);
                bk.container.Controls.Add(seta1pb);
                bk.container.Controls.Add(skilltb);
                bk.container.Controls.Add(seta2pb);
                bk.container.Controls.Add(nextItemtb);
                bk.container.Controls.Add(removepb);

                lastLocation = new Point(lastLocation.X, lastLocation.Y + DISTANCE_BETWEEN_ROWS);

            }

        }

        private static void headerLabels(AutoSwitchContainer bk)
        {
            Label itemLB = new Label();
            Label skillLB = new Label();
            Label proxItemLB = new Label();

            itemLB.AutoSize = true;
            itemLB.Location = new Point(55, 15);
            itemLB.Name = "itemLB";
            itemLB.Size = new System.Drawing.Size(27, 13);
            itemLB.TabIndex = 304;
            itemLB.Text = "Item";
            bk.container.Controls.Add(itemLB);

            skillLB.AutoSize = true;
            skillLB.Location = new Point(140, 15);
            skillLB.Name = "skillLB";
            skillLB.Size = new System.Drawing.Size(27, 13);
            skillLB.TabIndex = 304;
            skillLB.Text = "Skill";
            bk.container.Controls.Add(skillLB);

            proxItemLB.AutoSize = true;
            proxItemLB.Location = new Point(205, 15);
            proxItemLB.Name = "proxItemLB";
            proxItemLB.Size = new System.Drawing.Size(27, 13);
            proxItemLB.TabIndex = 304;
            proxItemLB.Text = "Proximo Item";
            bk.container.Controls.Add(proxItemLB);
        }

        private void onTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != String.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    EffectStatusIDs statusID = (EffectStatusIDs)Int16.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    string type = txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[0];

                    AutoSwitchConfig config = ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping.Find(cfg => cfg.skillId == statusID);
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

                        ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping.Add(new AutoSwitchConfig(statusID, key, type));
                    }

                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
                }
            }
            catch (Exception ex)
            {
                // Silently ignore invalid key input - this is expected during user typing
                // Only log if it's not a parsing error
                if (!(ex is ArgumentException || ex is FormatException))
                {
                    Console.WriteLine($"Error in AutoSwitchRenderer.onTextChange: {ex.Message}");
                }
            }
        }

        private void removeSkill(object sender, EventArgs e)
        {
            PictureBox removepb = (PictureBox)sender;
            int skillID = short.Parse(removepb.Name.Split(new[] { "remove" }, StringSplitOptions.None)[1]);

            var config =  ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping.FirstOrDefault(x => (int)x.skillId == skillID);
            ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping.Remove(config);
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutoSwitch);
            this._subject.Notify(new Utils.Message(Utils.MessageCode.CHANGED_AUTOSWITCH_SKILL, ProfileSingleton.GetCurrent().AutoSwitch.autoSwitchGenericMapping));
        }

        public static void doUpdate(Dictionary<EffectStatusIDs, Key> autobuffDict, Control control)
        {
            FormUtils.ResetForm(control);
            foreach (EffectStatusIDs effect in autobuffDict.Keys)
            {
                Control[] c = control.Controls.Find("in" + (int)effect, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)c[0];
                    textBox.Text = autobuffDict[effect].ToString();
                }
            }
        }


        private void textBox_GotFocus(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            this.OldText = txtBox.Text.ToString();
        }
    }
}
