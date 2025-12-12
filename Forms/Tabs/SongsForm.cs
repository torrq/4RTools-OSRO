using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class SongsForm : Form, IObserver
    {
        private List<GroupBox> dynamicPanels = new List<GroupBox>();

        public SongsForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();

            // Hide the template panel - we'll use it as a template only
            templateSongPanel.Visible = false;

            CreateDynamicRows();

            // Dynamically generate reset button names based on TOTAL_SONG_ROWS
            int totalRows = ConfigGlobal.GetConfig().SongRows;
            string[] resetButtonNames = Enumerable.Range(1, totalRows)
                .Select(i => $"btnResetSong{i}")
                .ToArray();
            FormHelper.ApplyColorToButtons(this, resetButtonNames, AppConfig.ResetButtonBackColor);
            FormHelper.SetNumericUpDownMinimumDelays(this);
            ConfigureSongRows();
            AddCommonResetButtonTooltip();
        }

        private void CreateDynamicRows()
        {
            int totalRows = ConfigGlobal.GetConfig().SongRows;
            for (int i = 1; i <= totalRows; i++)
            {
                GroupBox newPanel = CloneGroupBox(templateSongPanel, i);
                newPanel.Location = new Point(templateSongPanel.Location.X, templateSongPanel.Location.Y + (i - 1) * 110);
                newPanel.Text = $"Song {i}";
                newPanel.Visible = true;

                this.Controls.Add(newPanel);
                dynamicPanels.Add(newPanel);
            }

            // Adjust form height to accommodate all rows
            this.Height = Math.Max(this.Height, templateSongPanel.Location.Y + (totalRows * 110) + 50);
        }

        private GroupBox CloneGroupBox(GroupBox original, int id)
        {
            GroupBox clone = new GroupBox();
            clone.Size = original.Size;
            clone.Text = original.Text;
            clone.FlatStyle = original.FlatStyle;
            clone.Name = $"panelSong{id}";

            foreach (Control control in original.Controls)
            {
                Control clonedControl = CloneControl(control, id);
                if (clonedControl != null)
                {
                    clone.Controls.Add(clonedControl);
                }
            }

            return clone;
        }

        private Control CloneControl(Control original, int id)
        {
            Control clone = null;

            if (original is TextBox textBox)
            {
                clone = new TextBox();
                ((TextBox)clone).BorderStyle = textBox.BorderStyle;
                ((TextBox)clone).TextAlign = textBox.TextAlign;
                ((TextBox)clone).Font = textBox.Font;

                // Update name from Template to actual ID
                string newName = textBox.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is NumericUpDown numericUpDown)
            {
                clone = new NumericUpDown();
                ((NumericUpDown)clone).BorderStyle = numericUpDown.BorderStyle;
                ((NumericUpDown)clone).TextAlign = numericUpDown.TextAlign;
                ((NumericUpDown)clone).Font = numericUpDown.Font;
                ((NumericUpDown)clone).Maximum = numericUpDown.Maximum;
                ((NumericUpDown)clone).Minimum = numericUpDown.Minimum;

                // Update name from Template to actual ID
                string newName = numericUpDown.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is Button button)
            {
                clone = new Button();
                ((Button)clone).FlatStyle = button.FlatStyle;
                ((Button)clone).ForeColor = button.ForeColor;
                ((Button)clone).UseVisualStyleBackColor = button.UseVisualStyleBackColor;
                ((Button)clone).Cursor = button.Cursor;
                clone.Text = button.Text;

                // Update name from Template to actual ID
                string newName = button.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is PictureBox pictureBox)
            {
                clone = new PictureBox();
                ((PictureBox)clone).Image = pictureBox.Image;
                ((PictureBox)clone).BackgroundImage = pictureBox.BackgroundImage;
                ((PictureBox)clone).BackgroundImageLayout = pictureBox.BackgroundImageLayout;
                ((PictureBox)clone).SizeMode = pictureBox.SizeMode;
                ((PictureBox)clone).TabStop = pictureBox.TabStop;
                clone.Name = $"{pictureBox.Name}_Panel{id}";
            }
            else if (original is Label label)
            {
                clone = new Label();
                clone.Text = label.Text;
                ((Label)clone).AutoSize = label.AutoSize;
                ((Label)clone).Font = label.Font;
                clone.Name = $"{label.Name}_Panel{id}";
            }
            else if (original is Panel panel)
            {
                clone = new Panel();
                ((Panel)clone).BackColor = panel.BackColor;
                clone.Name = $"{panel.Name}_Panel{id}";
            }

            if (clone != null)
            {
                clone.Location = original.Location;
                clone.Size = original.Size;
                clone.TabIndex = original.TabIndex;
            }

            return clone;
        }

        private void AddCommonResetButtonTooltip()
        {
            ToolTip tooltip = new ToolTip();
            string tooltipText = "Reset this song row to default values";

            int totalRows = ConfigGlobal.GetConfig().SongRows;
            for (int i = 1; i <= totalRows; i++)
            {
                string buttonName = $"btnResetSong{i}";
                Control[] foundControls = this.Controls.Find(buttonName, true);

                if (foundControls.Length > 0 && foundControls[0] is Button resetButton)
                {
                    tooltip.SetToolTip(resetButton, tooltipText);
                }
                else
                {
                    DebugLogger.Warning($"Reset button '{buttonName}' not found.");
                }
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    UpdateUI();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().SongMacro.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().SongMacro.Stop();
                    break;
            }
        }

        private void UpdateSongRowData(int rowId)
        {
            try
            {
                MacroSong songMacro = ProfileSingleton.GetCurrent().SongMacro;
                if (songMacro == null)
                {
                    DebugLogger.Error($"SongMacro is null for rowId {rowId}");
                    return;
                }

                SongRow songRow = songMacro.GetSongRow(rowId);
                if (songRow == null)
                {
                    DebugLogger.Error($"SongRow is null for rowId {rowId}");
                    return;
                }

                // Ensure SongSequence is properly initialized
                if (songRow.SongSequence == null || songRow.SongSequence.Length != 8)
                {
                    DebugLogger.Info($"Reinitializing SongSequence for rowId {rowId}");
                    songRow.SongSequence = new Keys[8];
                    for (int i = 0; i < 8; i++)
                    {
                        songRow.SongSequence[i] = Keys.None;
                    }
                }

                GroupBox panel = (GroupBox)this.Controls.Find($"panelSong{rowId}", true)[0];
                if (panel == null)
                {
                    DebugLogger.Error($"Panel panelSong{rowId} not found");
                    return;
                }

                // Update Trigger Key
                Control[] triggerControls = panel.Controls.Find($"txtTriggerSong{rowId}", true);
                if (triggerControls.Length > 0 && triggerControls[0] is TextBox triggerTextBox)
                {
                    triggerTextBox.TextChanged -= OnTextChange;
                    triggerTextBox.Text = songRow.TriggerKey.ToString();
                    triggerTextBox.TextChanged += OnTextChange;
                    FormHelper.ApplyInputKeyStyle(triggerTextBox, songRow.TriggerKey != Keys.None);
                }

                // Update Dagger Key
                Control[] daggerControls = panel.Controls.Find($"txtDaggerSong{rowId}", true);
                if (daggerControls.Length > 0 && daggerControls[0] is TextBox daggerTextBox)
                {
                    daggerTextBox.TextChanged -= OnTextChange;
                    daggerTextBox.Text = songRow.DaggerKey.ToString();
                    daggerTextBox.TextChanged += OnTextChange;
                    FormHelper.ApplyInputKeyStyle(daggerTextBox, songRow.DaggerKey != Keys.None);
                }

                // Update Instrument Key
                Control[] instrumentControls = panel.Controls.Find($"txtInstrumentSong{rowId}", true);
                if (instrumentControls.Length > 0 && instrumentControls[0] is TextBox instrumentTextBox)
                {
                    instrumentTextBox.TextChanged -= OnTextChange;
                    instrumentTextBox.Text = songRow.InstrumentKey.ToString();
                    instrumentTextBox.TextChanged += OnTextChange;
                    FormHelper.ApplyInputKeyStyle(instrumentTextBox, songRow.InstrumentKey != Keys.None);
                }

                // Update Song Sequence (8 keys) with extensive safety checks
                for (int i = 0; i < 8 && i < songRow.SongSequence.Length; i++)
                {
                    try
                    {
                        Control[] sequenceControls = panel.Controls.Find($"txtSong{i + 1}Row{rowId}", true);
                        if (sequenceControls.Length > 0 && sequenceControls[0] is TextBox sequenceTextBox)
                        {
                            sequenceTextBox.TextChanged -= OnTextChange;
                            sequenceTextBox.Text = songRow.SongSequence[i].ToString();
                            sequenceTextBox.TextChanged += OnTextChange;
                            FormHelper.ApplyInputKeyStyle(sequenceTextBox, songRow.SongSequence[i] != Keys.None);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        DebugLogger.Error($"Exception updating song sequence {i} for row {rowId}: {innerEx}");
                    }
                }

                // Update Delay
                Control[] delayControls = panel.Controls.Find($"numDelaySong{rowId}", true);
                if (delayControls.Length > 0 && delayControls[0] is NumericUpDown delayInput)
                {
                    delayInput.ValueChanged -= OnDelayChange;
                    delayInput.Value = songRow.Delay;
                    delayInput.ValueChanged += OnDelayChange;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongsForm.UpdateSongRowData for rowId {rowId}: {ex}");
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            try
            {
                MacroSong songMacro = ProfileSingleton.GetCurrent().SongMacro;
                TextBox textBox = (TextBox)sender;

                if (!Enum.TryParse(textBox.Text, out Keys key))
                {
                    key = Keys.None;
                }

                // Parse the textbox name to determine row and type
                string name = textBox.Name;

                if (name.StartsWith("txtTriggerSong"))
                {
                    int rowId = int.Parse(name.Replace("txtTriggerSong", ""));
                    var songRow = songMacro.GetSongRow(rowId);
                    if (songRow != null)
                    {
                        songRow.TriggerKey = key;
                        FormHelper.ApplyInputKeyStyle(textBox, key != Keys.None);
                    }
                }
                else if (name.StartsWith("txtDaggerSong"))
                {
                    int rowId = int.Parse(name.Replace("txtDaggerSong", ""));
                    var songRow = songMacro.GetSongRow(rowId);
                    if (songRow != null)
                    {
                        songRow.DaggerKey = key;
                        FormHelper.ApplyInputKeyStyle(textBox, key != Keys.None);
                    }
                }
                else if (name.StartsWith("txtInstrumentSong"))
                {
                    int rowId = int.Parse(name.Replace("txtInstrumentSong", ""));
                    var songRow = songMacro.GetSongRow(rowId);
                    if (songRow != null)
                    {
                        songRow.InstrumentKey = key;
                        FormHelper.ApplyInputKeyStyle(textBox, key != Keys.None);
                    }
                }
                else if (name.StartsWith("txtSong") && name.Contains("Row"))
                {
                    // Parse song sequence: txtSong1Row1, txtSong2Row1, etc.
                    string[] parts = name.Replace("txtSong", "").Split(new[] { "Row" }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        int sequenceIndex = int.Parse(parts[0]) - 1; // Convert to 0-based index
                        int rowId = int.Parse(parts[1]);

                        var songRow = songMacro.GetSongRow(rowId);
                        if (songRow != null && sequenceIndex >= 0 && sequenceIndex < 8)
                        {
                            songRow.SongSequence[sequenceIndex] = key;
                            FormHelper.ApplyInputKeyStyle(textBox, key != Keys.None);
                        }
                    }
                }

                ProfileSingleton.SetConfiguration(songMacro);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongsForm.OnTextChange: {ex}");
            }
        }

        private void OnDelayChange(object sender, EventArgs e)
        {
            try
            {
                MacroSong songMacro = ProfileSingleton.GetCurrent().SongMacro;
                NumericUpDown delayInput = (NumericUpDown)sender;

                // Parse row ID from control name (numDelaySong1, numDelaySong2, etc.)
                int rowId = int.Parse(delayInput.Name.Replace("numDelaySong", ""));

                var songRow = songMacro.GetSongRow(rowId);
                if (songRow != null)
                {
                    songRow.Delay = (int)delayInput.Value;
                    ProfileSingleton.SetConfiguration(songMacro);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongsForm.OnDelayChange: {ex}");
            }
        }

        private void OnReset(object sender, EventArgs e)
        {
            try
            {
                Button resetButton = (Button)sender;

                // Parse row ID from button name (btnResetSong1, btnResetSong2, etc.)
                int rowId = int.Parse(resetButton.Name.Replace("btnResetSong", ""));

                MacroSong songMacro = ProfileSingleton.GetCurrent().SongMacro;
                songMacro.ResetSongRow(rowId);

                // Update the UI for this row
                UpdateSongRowData(rowId);

                // Save the changes
                ProfileSingleton.SetConfiguration(songMacro);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongsForm.OnReset: {ex}");
            }
        }

        private void UpdateUI()
        {
            int totalRows = ConfigGlobal.GetConfig().SongRows;
            for (int i = 1; i <= totalRows; i++)
            {
                UpdateSongRowData(i);
            }
        }

        private void ConfigureSongRows()
        {
            int totalRows = ConfigGlobal.GetConfig().SongRows;
            for (int i = 1; i <= totalRows; i++)
            {
                InitializeSongRow(i);
            }
        }

        private void InitializeSongRow(int rowId)
        {
            try
            {
                GroupBox panel = (GroupBox)this.Controls.Find($"panelSong{rowId}", true)[0];

                foreach (Control control in panel.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.KeyDown += new KeyEventHandler(FormHelper.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
                        textBox.TextChanged += new EventHandler(this.OnTextChange);
                    }
                    else if (control is Button resetButton && resetButton.Name.StartsWith("btnResetSong"))
                    {
                        resetButton.Click += new EventHandler(this.OnReset);
                    }
                    else if (control is NumericUpDown numericUpDown && numericUpDown.Name.StartsWith("numDelaySong"))
                    {
                        numericUpDown.ValueChanged += new EventHandler(this.OnDelayChange);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongsForm.InitializeSongRow: {ex}");
            }
        }
    }
}