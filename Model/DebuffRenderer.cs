using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    internal class DebuffRenderer
    {

        private readonly int BUFFS_PER_ROW = 5;
        private readonly int DISTANCE_BETWEEN_CONTAINERS = 10;
        private readonly int DISTANCE_BETWEEN_ROWS = 36;

        private readonly List<BuffContainer> _containers;
        private readonly ToolTip _toolTip;

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
                    pb.Size = new Size(32, 32);
                    _toolTip.SetToolTip(pb, skill.Name);

                    textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                    textBox.TextChanged += new EventHandler(OnTextChange);
                    textBox.Size = new Size(55, 20);
                    textBox.Tag = ((int)skill.EffectStatusID);
                    textBox.Name = "in" + ((int)skill.EffectStatusID);
                    textBox.Location = new Point(pb.Location.X + 35, pb.Location.Y + 8);
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

                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    EffectStatusIDs statusID = (EffectStatusIDs)short.Parse(txtBox.Name.Split(new[] { "in" }, StringSplitOptions.None)[1]);
                    ProfileSingleton.GetCurrent().DebuffsRecovery.AddKeyToBuff(statusID, key);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().DebuffsRecovery);
                }
            }
            catch (Exception ex)
            {
                // Silently ignore invalid key input - this is expected during user typing
                // Only log if it's not a parsing error
                if (!(ex is ArgumentException || ex is FormatException))
                {
                    Console.WriteLine($"Error in DebuffRenderer.OnTextChange: {ex.Message}");
                }
            }
        }
    }
}
