using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    internal class BuffRenderer
    {
        private readonly int BUFFS_PER_ROW = 6;
        private readonly int DISTANCE_BETWEEN_CONTAINERS = 2;
        private readonly int DISTANCE_BETWEEN_ROWS = 28;
        private readonly int ICON_TEXT_SPACING = 27;
        private readonly int ICON_SPACING = 93;
        private readonly Size TEXTBOX_SIZE = new Size(60, 20);
        private const int TEXTBOX_VERTICAL_ADJUSTMENT = 2;

        private readonly List<BuffContainer> _containers;
        private readonly ToolTip _toolTip;
        private readonly string _typeAutoBuff;
        private readonly Subject _subject;
        string OldText = string.Empty;

        public BuffRenderer(List<BuffContainer> containers, ToolTip toolTip, string autoBuff, Subject subject)
        {
            this._containers = containers;
            this._toolTip = toolTip;
            this._typeAutoBuff = autoBuff;
            this._subject = subject;
            ConfigureToolTipDelays();
        }

        private void ConfigureToolTipDelays()
        {
            this._toolTip.InitialDelay = 50;
            this._toolTip.AutoPopDelay = 5000;
            this._toolTip.ReshowDelay = 50;
        }

        public void DoRender()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                BuffContainer bk = _containers[i];
                bk.Container.Controls.Clear();

                Point lastLocation = new Point(bk.Container.Location.X, 20);
                int colCount = 0;
                int maxRowHeight = 0;
                int lastElementY = 0;

                if (i > 0)
                {
                    bk.Container.Location = new Point(_containers[i - 1].Container.Location.X, _containers[i - 1].Container.Location.Y + _containers[i - 1].Container.Height + DISTANCE_BETWEEN_CONTAINERS);
                }

                foreach (Buff skill in bk.Skills)
                {
                    PictureBox pb = new PictureBox();
                    TextBox textBox = new TextBox();

                    pb.Image = skill.Icon;
                    pb.BackgroundImageLayout = ImageLayout.Center;
                    pb.Location = new Point(lastLocation.X + (colCount * ICON_SPACING), lastLocation.Y);
                    pb.Name = "pbox" + ((int)skill.EffectStatusID);
                    pb.Size = new Size(26, 26);
                    _toolTip.SetToolTip(pb, skill.Name);

                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.GotFocus += new EventHandler(TextBox_GotFocus);
                    textBox.TextChanged += new EventHandler(OnTextChange);
                    textBox.Size = TEXTBOX_SIZE;
                    textBox.Tag = ((int)skill.EffectStatusID);
                    textBox.Name = "in" + ((int)skill.EffectStatusID);
                    textBox.Location = new Point(pb.Location.X + ICON_TEXT_SPACING, pb.Location.Y + 3 - TEXTBOX_VERTICAL_ADJUSTMENT);
                    textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    textBox.Text = "None"; // Set default value

                    bk.Container.Controls.Add(textBox);
                    bk.Container.Controls.Add(pb);

                    colCount++;
                    maxRowHeight = Math.Max(maxRowHeight, pb.Height + textBox.Height);
                    lastElementY = Math.Max(lastElementY, pb.Location.Y + pb.Height);

                    if (colCount == BUFFS_PER_ROW)
                    {
                        colCount = 0;
                        lastLocation = new Point(bk.Container.Location.X, lastLocation.Y + DISTANCE_BETWEEN_ROWS);
                        maxRowHeight = 0;
                    }
                }
                int desiredHeight = lastElementY + 10;
                bk.Container.Height = desiredHeight;
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                bool textChanged = this.OldText != string.Empty && this.OldText != txtBox.Text.ToString();

                if ((txtBox.Text.ToString() != string.Empty) && textChanged)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    EffectStatusIDs statusID = (EffectStatusIDs)short.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);

                    if (this._typeAutoBuff == ProfileSingleton.GetCurrent().AutobuffSkill.ActionName)
                    {
                        var _autoBuffSkill = ProfileSingleton.GetCurrent().AutobuffSkill;
                        _autoBuffSkill.AddKeyToBuff(statusID, key);
                        ProfileSingleton.SetConfiguration(_autoBuffSkill);
                        _subject.Notify(new Utils.Message(Utils.MessageCode.ADDED_NEW_AUTOBUFF_SKILL, _autoBuffSkill));
                    }
                    else
                    {
                        var _autoBuffItem = ProfileSingleton.GetCurrent().AutobuffItem;
                        _autoBuffItem.AddKeyToBuff(statusID, key);
                        ProfileSingleton.SetConfiguration(_autoBuffItem);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"OnTextChange: Error processing TextChanged event: {ex.Message}");
            }
        }

        public static void DoUpdate(Dictionary<EffectStatusIDs, Key> autobuffDict, Control control)
        {
            if (control == null || autobuffDict == null)
            {
                return;
            }

            ResetTextBoxes(control);

            foreach (EffectStatusIDs effect in autobuffDict.Keys)
            {
                Control[] c = control.Controls.Find("in" + (int)effect, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)c[0];
                    textBox.Text = autobuffDict[effect].ToString();
                }
            }
            // Set unmapped TextBoxes back to "None"
            foreach (Control c in control.Controls)
            {
                if (c is TextBox textBox && textBox.Name.StartsWith("in") && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "None";
                }
                if (c.HasChildren)
                {
                    DoUpdate(autobuffDict, c); // Recursive update for nested controls
                }
            }
        }

        private static void ResetTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox textBox && textBox.Name.StartsWith("in"))
                {
                    textBox.Text = "";
                }
                if (c.HasChildren)
                {
                    ResetTextBoxes(c);
                }
            }
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            this.OldText = txtBox.Text.ToString();
        }
    }
}