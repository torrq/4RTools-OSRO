using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;

namespace _4RTools.Model
{
    internal class BuffRenderer
    {

        private readonly int BUFFS_PER_ROW = 5;
        private readonly int DISTANCE_BETWEEN_CONTAINERS = 10;
        private readonly int DISTANCE_BETWEEN_ROWS = 30;

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
        }

        public void DoRender()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                BuffContainer bk = _containers[i];
                Point lastLocation = new Point(bk.Container.Location.X, 20);
                int colCount = 0;

                if (i > 0)
                {
                    //If not first container to be rendered, get last container height and append 70
                    bk.Container.Location = new Point(_containers[i - 1].Container.Location.X, _containers[i - 1].Container.Location.Y + _containers[i - 1].Container.Height + DISTANCE_BETWEEN_CONTAINERS);
                }

                foreach (Buff skill in bk.Skills)
                {
                    PictureBox pb = new PictureBox();
                    TextBox textBox = new TextBox();

                    pb.Image = skill.Icon;
                    pb.BackgroundImageLayout = ImageLayout.Center;
                    pb.Location = new Point(lastLocation.X + (colCount * 100), lastLocation.Y);
                    pb.Name = "pbox" + ((int)skill.EffectStatusID);
                    pb.Size = new Size(26, 26);
                    _toolTip.SetToolTip(pb, skill.Name);

                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.GotFocus += new EventHandler(TextBox_GotFocus);
                    textBox.TextChanged += new EventHandler(OnTextChange);
                    textBox.Size = new Size(55, 20);
                    textBox.Tag = ((int)skill.EffectStatusID);
                    textBox.Name = "in" + ((int)skill.EffectStatusID);
                    textBox.Location = new Point(pb.Location.X + 35, pb.Location.Y + 3);
                    textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                    bk.Container.Controls.Add(textBox);
                    bk.Container.Controls.Add(pb);

                    colCount++;

                    if (colCount == BUFFS_PER_ROW)
                    {
                        //5 Buffs per row
                        colCount = 0;
                        lastLocation = new Point(bk.Container.Location.X, lastLocation.Y + DISTANCE_BETWEEN_ROWS);
                    }
                }
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            try
            {
                if (this._typeAutoBuff == ProfileSingleton.GetCurrent().AutobuffSkill.ActionName)
                {
                    var _autoBuff = ProfileSingleton.GetCurrent().AutobuffSkill;
                }

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
                        var _autoBuffStuff = ProfileSingleton.GetCurrent().AutobuffStuff;
                        _autoBuffStuff.AddKeyToBuff(statusID, key);
                        ProfileSingleton.SetConfiguration(_autoBuffStuff);
                    }
                }

            }
            catch { }
        }

        public static void DoUpdate(Dictionary<EffectStatusIDs, Key> autobuffDict, Control control)
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


        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            this.OldText = txtBox.Text.ToString();
        }
    }
}
