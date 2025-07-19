using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using Label = System.Windows.Forms.Label;

namespace _4RTools.Model
{
    internal class BuffRenderer
    {
        // Configuration
        private readonly int GroupsPerRow = 2;

        private readonly int ElementSpacing = 2;          // Between icon, textbox, label
        private readonly int GroupSpacing = 8;            // Between two groups in a row
        private readonly int RowSpacing = 2;              // Between rows
        private readonly int ContainerSpacing = 10;       // Between BuffContainer blocks
        private readonly int LabelVerticalAdjust = 3;     // For label vertical alignment
        private readonly int TextboxVerticalAdjust = 2;
        private readonly int ContainerPaddingLeft = 10;
        private readonly int ContainerPaddingTop = 18;
        private readonly int ContainerPaddingBottom = 10;

        private readonly Size IconSize = new Size(26, 26);
        private readonly Size TextBoxSize = new Size(55, 26);
        private readonly Size LabelSize = new Size(190, 26);

        private readonly Font LabelFont = new Font("Tahoma", 9, FontStyle.Regular);
        private readonly Font LabelFontSmall = new Font("Tahoma", 8, FontStyle.Regular);

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
            this._toolTip.AutoPopDelay = 15000;
            this._toolTip.ReshowDelay = 50;
        }

        public void DoRender()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                BuffContainer bk = _containers[i];
                bk.Container.Controls.Clear();

                // Position container vertically
                if (i > 0)
                {
                    var prev = _containers[i - 1];
                    bk.Container.Location = new Point(prev.Container.Location.X, prev.Container.Location.Y + prev.Container.Height + ContainerSpacing);
                }

                Point currentPos = new Point(ContainerPaddingLeft, ContainerPaddingTop);
                int groupCount = 0;

                foreach (Buff skill in bk.Skills)
                {
                    int groupOffsetX = groupCount % GroupsPerRow *
                        (IconSize.Width + TextBoxSize.Width + LabelSize.Width + (ElementSpacing * 2) + GroupSpacing);

                    int rowOffsetY = (groupCount / GroupsPerRow) *
                        (Math.Max(IconSize.Height, TextBoxSize.Height) + RowSpacing);

                    int baseX = currentPos.X + groupOffsetX;
                    int baseY = currentPos.Y + rowOffsetY;

                    PictureBox pb = new PictureBox
                    {
                        Image = skill.Icon,
                        BackgroundImageLayout = ImageLayout.Center,
                        Location = new Point(baseX, baseY),
                        Name = "pbox" + (int)skill.EffectStatusID,
                        Size = IconSize
                    };
                    _toolTip.SetToolTip(pb, skill.Name);

                    TextBox textBox = new TextBox
                    {
                        Size = TextBoxSize,
                        Location = new Point(pb.Right + ElementSpacing, baseY + TextboxVerticalAdjust),
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = (int)skill.EffectStatusID,
                        Name = "in" + (int)skill.EffectStatusID,
                        Text = AppConfig.TEXT_NONE,
                        Font = new Font("Tahoma", 9, FontStyle.Regular),
                        TextAlign = HorizontalAlignment.Center
                    };
                    textBox.KeyDown += FormUtils.OnKeyDown;
                    textBox.KeyPress += FormUtils.OnKeyPress;
                    textBox.GotFocus += TextBox_GotFocus;
                    textBox.TextChanged += OnTextChange;

                    string SkillName = skill.Name;

                    if (SkillName.Length > 33)
                    {
                        int breakIndex = SkillName.LastIndexOf(' ', 33);
                        if (breakIndex > 0)
                            SkillName = SkillName.Substring(0, breakIndex) + "\r\n" + SkillName.Substring(breakIndex + 1);
                        else
                            SkillName = SkillName.Insert(33, "\r\n"); // fallback if no space
                    }

                    Label label = new Label
                    {
                        Size = LabelSize,
                        Location = new Point(textBox.Right + ElementSpacing, baseY + LabelVerticalAdjust),
                        Tag = (int)skill.EffectStatusID,
                        Name = "inl" + (int)skill.EffectStatusID,
                        Text = SkillName,
                    };

                    if (skill.Name.Length > 33)
                    {
                        label.Font = LabelFontSmall;
                        label.Location = new Point(label.Location.X, label.Location.Y - 4);
                    } else {
                        label.Font = LabelFont;
                    }

                    bk.Container.Controls.Add(pb);
                    bk.Container.Controls.Add(textBox);
                    bk.Container.Controls.Add(label);

                    groupCount++;
                }

                // Final height calculation includes vertical padding
                int totalRows = (int)Math.Ceiling(groupCount / (double)GroupsPerRow);
                int contentHeight = totalRows * (Math.Max(IconSize.Height, TextBoxSize.Height) + RowSpacing);
                bk.Container.Height = ContainerPaddingTop + contentHeight - RowSpacing + ContainerPaddingBottom;
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
                    textBox.Text = AppConfig.TEXT_NONE;
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