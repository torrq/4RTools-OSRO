using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Cursors = System.Windows.Forms.Cursors;

namespace _ORTools.Forms
{
    public partial class AutopotSPForm : Form, IObserver
    {
        private AutopotSP autopotSP;
        private List<Panel> spPanels; // List to manage UI panels for sorting
        private readonly Dictionary<int, Panel> panelMap = new Dictionary<int, Panel>(); // Maps a Slot ID to its original panel
        private readonly Point startPoint = new Point(11, 25); // Define a constant starting position for the panels

        public AutopotSPForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);
            InitializeDragDrop();
        }

        /// <summary>
        /// Sets up the drag-and-drop functionality for the reorder picture boxes.
        /// </summary>
        private void InitializeDragDrop()
        {
            // Store panels for easy access and reordering
            spPanels = new List<Panel> { spPanel1, spPanel2, spPanel3, spPanel4, spPanel5 };

            // Map Slot IDs to their corresponding panels for easy lookup later
            panelMap.Add(1, spPanel1);
            panelMap.Add(2, spPanel2);
            panelMap.Add(3, spPanel3);
            panelMap.Add(4, spPanel4);
            panelMap.Add(5, spPanel5);

            // Enable dropping on the form itself
            this.AllowDrop = true;
            this.DragEnter += Form_DragEnter;
            this.DragDrop += Form_DragDrop;

            // Attach event handlers to each panel
            foreach (Panel p in spPanels)
            {
                // Remove the cursor and mouse down event from the panel itself
                // p.Cursor = Cursors.HSplit; // REMOVED
                // p.MouseDown += Panel_MouseDown; // REMOVED

                // Find the reorder PictureBox in this panel and attach drag events
                PictureBox reorderSP = p.Controls.OfType<PictureBox>().FirstOrDefault(pb => pb.Name.StartsWith("reorderSP"));
                if (reorderSP != null)
                {
                    reorderSP.Cursor = Cursors.SizeAll; // Change cursor to indicate it's draggable
                    reorderSP.MouseDown += reorderSP_MouseDown;
                }

                // Attach generic event handlers to controls within the panel
                foreach (Control control in p.Controls)
                {
                    if (control is CheckBox chk) chk.CheckedChanged += OnSPEnabledChanged;
                    if (control is TextBox txt)
                    {
                        txt.KeyDown += FormHelper.OnKeyDown;
                        txt.KeyPress += FormHelper.OnKeyPress;
                        txt.TextChanged += OnSPKeyChanged;
                    }
                    if (control is NumericUpDown num)
                    {
                        FormHelper.AttachBlankFix(num);
                        num.ValueChanged += OnSPPercentChanged;
                    }
                }
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopotSP = ProfileSingleton.GetCurrent().AutopotSP;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.autopotSP?.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autopotSP?.Start();
                    break;
            }
        }

        /// <summary>
        /// Initializes or refreshes the form based on the current profile's settings.
        /// </summary>
        private void InitializeApplicationForm()
        {
            if (autopotSP == null) return;

            // Reorder the UI panels based on the saved order in SPSlots
            spPanels = autopotSP.SPSlots.Select(slot => panelMap[slot.Id]).ToList();

            UpdatePanelLayoutAndData();

            // Other settings
            this.numAutopotSPDelay.Value = this.autopotSP.Delay;
        }

        /// <summary>
        /// Rearranges the panels on the form and binds the data from SPSlots.
        /// </summary>
        private void UpdatePanelLayoutAndData()
        {
            if (autopotSP == null || autopotSP.SPSlots == null) return;

            for (int i = 0; i < spPanels.Count; i++)
            {
                Panel panel = spPanels[i];
                AutopotSP.SPSlot slot = autopotSP.SPSlots[i];

                // 1. Update Panel Position using a fixed start point and adding a small margin
                panel.Location = new Point(startPoint.X, startPoint.Y + (i * (panel.Height + 1)));

                // 2. Update Data on Controls inside the Panel
                foreach (Control control in panel.Controls)
                {
                    // Temporarily disable event handlers to prevent them from firing during data binding
                    if (control is CheckBox chk) {
                        chk.CheckedChanged -= OnSPEnabledChanged;
                        chk.Checked = slot.Enabled;
                        chk.CheckedChanged += OnSPEnabledChanged;
                    }
                    if (control is TextBox txt) {
                        txt.TextChanged -= OnSPKeyChanged;
                        txt.Text = slot.Key.ToString();
                        txt.TextChanged += OnSPKeyChanged;
                        if (slot.Key.ToString() != "None") {
                            FormHelper.ApplyInputKeyStyle(txt, true);
                        } else {
                            FormHelper.ApplyInputKeyStyle(txt, false);
                        }
                    }
                    if (control is NumericUpDown num) {
                        num.ValueChanged -= OnSPPercentChanged;
                        num.Value = slot.SPPercent;
                        num.ValueChanged += OnSPPercentChanged;
                    }
                }
            }
        }

        #region Drag and Drop Handlers

        private void reorderSP_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox reorderSP = sender as PictureBox;
                if (reorderSP != null)
                {
                    // Find the parent panel of this reorder PictureBox
                    Panel parentPanel = reorderSP.Parent as Panel;
                    if (parentPanel != null)
                    {
                        // Start drag operation with the parent panel as the data
                        parentPanel.DoDragDrop(parentPanel, DragDropEffects.Move);
                    }
                }
            }
        }

        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            Panel sourcePanel = (Panel)e.Data.GetData(typeof(Panel));
            if (sourcePanel == null) return;

            // Find the panel under the cursor
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
            Panel targetPanel = spPanels.FirstOrDefault(p => p.Bounds.Contains(clientPoint));

            if (targetPanel != null && targetPanel != sourcePanel)
            {
                // Get current indices
                int sourceIndex = spPanels.IndexOf(sourcePanel);
                int targetIndex = spPanels.IndexOf(targetPanel);

                if (sourceIndex != -1 && targetIndex != -1)
                {
                    // Swap the SPSlot data
                    var tempSlot = autopotSP.SPSlots[sourceIndex];
                    autopotSP.SPSlots.RemoveAt(sourceIndex);
                    autopotSP.SPSlots.Insert(targetIndex, tempSlot);

                    // Swap the panels in the UI list
                    var tempPanel = spPanels[sourceIndex];
                    spPanels.RemoveAt(sourceIndex);
                    spPanels.Insert(targetIndex, tempPanel);

                    // Refresh UI layout and save changes
                    UpdatePanelLayoutAndData();
                    ProfileSingleton.SetConfiguration(autopotSP);
                }
            }
        }
        #endregion

        #region Generic Control Event Handlers

        private void OnSPKeyChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            Panel parentPanel = txt?.Parent as Panel;
            int index = spPanels.IndexOf(parentPanel);

            if (index != -1 && autopotSP != null)
            {
                try
                {
                    //DebugLogger.Debug($"OnSPKeyChanged: TextBox Text = {txt.Text}, Index = {index}");
                    Keys key = (Keys)Enum.Parse(typeof(Keys), txt.Text);
                    autopotSP.SPSlots[index].Key = key;
                    ProfileSingleton.SetConfiguration(autopotSP);
                    this.ActiveControl = null;
                    if (key == Keys.None)
                    {
                        FormHelper.ApplyInputKeyStyle(txt, false);
                    }
                    else
                    {
                        FormHelper.ApplyInputKeyStyle(txt, true);
                    }
                }
                catch { /* Ignore parse errors */ }
            }
        }

        private void OnSPPercentChanged(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;
            Panel parentPanel = num?.Parent as Panel;
            int index = spPanels.IndexOf(parentPanel);

            if (index != -1 && autopotSP != null)
            {
                autopotSP.SPSlots[index].SPPercent = (int)num.Value;
                ProfileSingleton.SetConfiguration(autopotSP);
            }
        }

        private void OnSPEnabledChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            Panel parentPanel = chk?.Parent as Panel;
            int index = spPanels.IndexOf(parentPanel);

            if (index != -1 && autopotSP != null)
            {
                autopotSP.SPSlots[index].Enabled = chk.Checked;
                ProfileSingleton.SetConfiguration(autopotSP);
            }
        }

        #endregion

        #region Other Control Handlers
        private void NumDelay_ValueChanged(object sender, EventArgs e)
        {
            if (autopotSP != null)
            {
                autopotSP.Delay = (int)numAutopotSPDelay.Value;
                ProfileSingleton.SetConfiguration(autopotSP);
            }
        }

        #endregion
    }
}