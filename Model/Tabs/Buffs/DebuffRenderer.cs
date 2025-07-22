using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Model
{
    internal class DebuffRenderer
    {

        private readonly int BUFFS_PER_ROW = 3;
        private readonly int DISTANCE_BETWEEN_CONTAINERS = 0;
        private readonly int DISTANCE_BETWEEN_ROWS = 52;
        private readonly int ICON_SPACING = 180;
        private readonly int DEBUFFS_Y_MARGIN = 35;
        private readonly int DEBUFFS_X_MARGIN_OFFSET = 0;
        private readonly int DEBUFFS_Y_MARGIN_OFFSET = 10;
        private readonly int LABEL_SIZE_WIDTH = 85;
        private readonly int LABEL_X_OFFSET = 6;
        private readonly int LABEL_Y_OFFSET = -11;
        private readonly Size LABEL_SIZE = new Size(80, 50);
        private readonly Size PICTUREBOX_SIZE = new Size(48, 48);
        private readonly Size TEXTBOX_SIZE = new Size(55, 20);
        private readonly int TEXTBOX_X_OFFSET = 35;
        private readonly int TEXTBOX_Y_OFFSET = 4;

        private readonly List<BuffContainer> _containers;
        private readonly ToolTip _toolTip;
        private string OldText = string.Empty;

        public DebuffRenderer(List<BuffContainer> containers, ToolTip toolTip)
        {
            this._containers = containers;
            this._toolTip = toolTip;
        }

        public void DoRender()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                BuffContainer bk = _containers[i];
                Point lastLocation = new Point(bk.Container.Location.X + DEBUFFS_X_MARGIN_OFFSET, DEBUFFS_Y_MARGIN + DEBUFFS_Y_MARGIN_OFFSET);
                int colCount = 0;

                if (i > 0)
                {
                    //If not first container to be rendered, get last container height and append 70
                    bk.Container.Location = new Point(_containers[i - 1].Container.Location.X, _containers[i - 1].Container.Location.Y + _containers[i - 1].Container.Height + DISTANCE_BETWEEN_CONTAINERS);
                }

                foreach (Buff skill in bk.Skills)
                {
                    Label label = new Label();
                    PictureBox pb = new PictureBox();
                    TextBox textBox = new TextBox();

                    label.BackgroundImageLayout = ImageLayout.Center;
                    label.Location = new Point(lastLocation.X + (colCount * ICON_SPACING) + LABEL_X_OFFSET, lastLocation.Y + LABEL_Y_OFFSET);
                    label.Name = "lbl" + ((int)skill.EffectStatusID);
                    label.Size = LABEL_SIZE;
                    label.Text = skill.Name;
                    label.TextAlign = ContentAlignment.MiddleRight;
                    _toolTip.SetToolTip(label, skill.Name);

                    pb.Image = skill.Icon;
                    pb.BackgroundImageLayout = ImageLayout.Center;
                    pb.Location = new Point(lastLocation.X + (colCount * ICON_SPACING) + LABEL_SIZE_WIDTH, lastLocation.Y);
                    pb.Name = "pbox" + ((int)skill.EffectStatusID);
                    pb.Size = PICTUREBOX_SIZE;
                    _toolTip.SetToolTip(pb, skill.Name);

                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormHelper.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
                    textBox.TextChanged += new EventHandler(OnTextChange);
                    textBox.Size = TEXTBOX_SIZE;
                    textBox.Tag = ((int)skill.EffectStatusID);
                    textBox.Name = "in" + ((int)skill.EffectStatusID);
                    textBox.Location = new Point(pb.Location.X + TEXTBOX_X_OFFSET, pb.Location.Y + TEXTBOX_Y_OFFSET);
                    textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    textBox.TextAlign = HorizontalAlignment.Center;

                    bk.Container.Controls.Add(label);
                    bk.Container.Controls.Add(textBox);
                    bk.Container.Controls.Add(pb);

                    colCount++;

                    if (colCount == BUFFS_PER_ROW)
                    {
                        //5 Buffs per row
                        colCount = 0;
                        lastLocation = new Point(bk.Container.Location.X + DEBUFFS_X_MARGIN_OFFSET, lastLocation.Y + DISTANCE_BETWEEN_ROWS);
                    }
                }
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            if (this.OldText == txtBox.Text) return;

            try
            {
                Key key;
                bool textChanged = this.OldText != string.Empty && this.OldText != txtBox.Text.ToString();

                if (!Enum.TryParse(txtBox.Text, out key))
                {
                    key = Key.None;
                }

                if (txtBox.Text.ToString() != string.Empty)
                {
                    EffectStatusIDs statusID = (EffectStatusIDs)short.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    ProfileSingleton.GetCurrent().DebuffsRecovery.AddKeyToBuff(statusID, key);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().DebuffsRecovery);
                }

                if (key != Key.None)
                {
                    txtBox.Font = new Font(txtBox.Font, FontStyle.Bold);
                    txtBox.ForeColor = AppConfig.ActiveKeyColor;
                }
                else
                {
                    txtBox.Font = new Font(txtBox.Font, FontStyle.Regular);
                    txtBox.ForeColor = AppConfig.InactiveKeyColor;
                }

            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"OnTextChange: Error processing TextChanged event: {ex.Message}");
            }
        }


    }
}
