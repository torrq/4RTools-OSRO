using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Cursors = System.Windows.Forms.Cursors;

namespace _ORTools.Forms
{
    public partial class AutopotHPForm : Form, IObserver
    {
        private AutopotHP autopotHP;
        private List<Panel> hpPanels; // List to manage UI panels for sorting
        private readonly Dictionary<int, Panel> panelMap = new Dictionary<int, Panel>(); // Maps a Slot ID to its original panel
        private readonly Point startPoint = new Point(11, 25); // Define a constant starting position for the panels

        public AutopotHPForm(Subject subject)
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
            hpPanels = new List<Panel> { hpPanel1, hpPanel2, hpPanel3, hpPanel4, hpPanel5 };

            // Map Slot IDs to their corresponding panels for easy lookup later
            panelMap.Add(1, hpPanel1);
            panelMap.Add(2, hpPanel2);
            panelMap.Add(3, hpPanel3);
            panelMap.Add(4, hpPanel4);
            panelMap.Add(5, hpPanel5);

            // Enable dropping on the form itself
            this.AllowDrop = true;
            this.DragEnter += Form_DragEnter;
            this.DragDrop += Form_DragDrop;

            // Attach event handlers to each panel
            foreach (Panel p in hpPanels)
            {
                // Remove the cursor and mouse down event from the panel itself
                // p.Cursor = Cursors.HSplit; // REMOVED
                // p.MouseDown += Panel_MouseDown; // REMOVED

                // Find the reorder PictureBox in this panel and attach drag events
                PictureBox reorderPB = p.Controls.OfType<PictureBox>().FirstOrDefault(pb => pb.Name.StartsWith("reorderHP"));
                if (reorderPB != null)
                {
                    reorderPB.Cursor = Cursors.SizeAll; // Change cursor to indicate it's draggable
                    reorderPB.MouseDown += ReorderPB_MouseDown;
                }

                // Attach generic event handlers to controls within the panel
                foreach (Control control in p.Controls)
                {
                    if (control is CheckBox chk) chk.CheckedChanged += OnHPEnabledChanged;
                    if (control is TextBox txt)
                    {
                        txt.KeyDown += FormHelper.OnKeyDown;
                        txt.KeyPress += FormHelper.OnKeyPress;
                        txt.TextChanged += OnHPKeyChanged;
                    }
                    if (control is NumericUpDown num)
                    {
                        FormHelper.AttachBlankFix(num);
                        num.ValueChanged += OnHPPercentChanged;
                    }
                }
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopotHP = ProfileSingleton.GetCurrent().AutopotHP;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.autopotHP?.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autopotHP?.Start();
                    break;
            }
        }

        /// <summary>
        /// Initializes or refreshes the form based on the current profile's settings.
        /// </summary>
        private void InitializeApplicationForm()
        {
            if (autopotHP == null) return;

            // Reorder the UI panels based on the saved order in HPSlots
            hpPanels = autopotHP.HPSlots.Select(slot => panelMap[slot.Id]).ToList();

            UpdatePanelLayoutAndData();

            // Other settings
            this.numAutopotHPDelay.Value = this.autopotHP.Delay;
            this.chkStopOnCriticalInjury.Checked = this.autopotHP.StopOnCriticalInjury;
        }

        /// <summary>
        /// Rearranges the panels on the form and binds the data from HPSlots.
        /// </summary>
        private void UpdatePanelLayoutAndData()
        {
            if (autopotHP == null || autopotHP.HPSlots == null) return;

            for (int i = 0; i < hpPanels.Count; i++)
            {
                Panel panel = hpPanels[i];
                AutopotHP.HPSlot slot = autopotHP.HPSlots[i];

                // 1. Update Panel Position using a fixed start point and adding a small margin
                panel.Location = new Point(startPoint.X, startPoint.Y + (i * (panel.Height + 1)));

                // 2. Update Data on Controls inside the Panel
                foreach (Control control in panel.Controls)
                {
                    // Temporarily disable event handlers to prevent them from firing during data binding
                    if (control is CheckBox chk) { chk.CheckedChanged -= OnHPEnabledChanged; chk.Checked = slot.Enabled; chk.CheckedChanged += OnHPEnabledChanged; }
                    if (control is TextBox txt) { txt.TextChanged -= OnHPKeyChanged; txt.Text = slot.Key.ToString(); txt.TextChanged += OnHPKeyChanged; }
                    if (control is NumericUpDown num) { num.ValueChanged -= OnHPPercentChanged; num.Value = slot.HPPercent; num.ValueChanged += OnHPPercentChanged; }
                }
            }
        }

        #region Drag and Drop Handlers

        private void ReorderPB_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox reorderPB = sender as PictureBox;
                if (reorderPB != null)
                {
                    // Find the parent panel of this reorder PictureBox
                    Panel parentPanel = reorderPB.Parent as Panel;
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
            Panel targetPanel = hpPanels.FirstOrDefault(p => p.Bounds.Contains(clientPoint));

            if (targetPanel != null && targetPanel != sourcePanel)
            {
                // Get current indices
                int sourceIndex = hpPanels.IndexOf(sourcePanel);
                int targetIndex = hpPanels.IndexOf(targetPanel);

                if (sourceIndex != -1 && targetIndex != -1)
                {
                    // Swap the HPSlot data
                    var tempSlot = autopotHP.HPSlots[sourceIndex];
                    autopotHP.HPSlots.RemoveAt(sourceIndex);
                    autopotHP.HPSlots.Insert(targetIndex, tempSlot);

                    // Swap the panels in the UI list
                    var tempPanel = hpPanels[sourceIndex];
                    hpPanels.RemoveAt(sourceIndex);
                    hpPanels.Insert(targetIndex, tempPanel);

                    // Refresh UI layout and save changes
                    UpdatePanelLayoutAndData();
                    ProfileSingleton.SetConfiguration(autopotHP);
                }
            }
        }
        #endregion

        #region Generic Control Event Handlers

        private void OnHPKeyChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            Panel parentPanel = txt?.Parent as Panel;
            int index = hpPanels.IndexOf(parentPanel);

            if (index != -1 && autopotHP != null)
            {
                try
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txt.Text);
                    autopotHP.HPSlots[index].Key = key;
                    ProfileSingleton.SetConfiguration(autopotHP);
                    this.ActiveControl = null;
                }
                catch { /* Ignore parse errors */ }
            }
        }

        private void OnHPPercentChanged(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;
            Panel parentPanel = num?.Parent as Panel;
            int index = hpPanels.IndexOf(parentPanel);

            if (index != -1 && autopotHP != null)
            {
                autopotHP.HPSlots[index].HPPercent = (int)num.Value;
                ProfileSingleton.SetConfiguration(autopotHP);
            }
        }

        private void OnHPEnabledChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            Panel parentPanel = chk?.Parent as Panel;
            int index = hpPanels.IndexOf(parentPanel);

            if (index != -1 && autopotHP != null)
            {
                autopotHP.HPSlots[index].Enabled = chk.Checked;
                ProfileSingleton.SetConfiguration(autopotHP);
            }
        }

        #endregion

        #region Other Control Handlers
        private void NumDelay_ValueChanged(object sender, EventArgs e)
        {
            if (autopotHP != null)
            {
                autopotHP.Delay = (int)numAutopotHPDelay.Value;
                ProfileSingleton.SetConfiguration(autopotHP);
            }
        }

        private void ChkStopOnCriticalInjury_CheckedChanged(object sender, EventArgs e)
        {
            if (autopotHP != null)
            {
                autopotHP.StopOnCriticalInjury = chkStopOnCriticalInjury.Checked;
                ProfileSingleton.SetConfiguration(autopotHP);
            }
        }
        #endregion

        private void AutopotHPForm_Load(object sender, EventArgs e)
        {

        }
    }
}