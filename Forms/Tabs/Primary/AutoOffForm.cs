using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Forms
{
    public partial class AutoOffForm : Form, IObserver
    {
        #region Constants
        private const string LABEL_REMAINING_TIME_TEXT = "Remaining Time:";
        private const string LABEL_REMAINING_TIME_STOPPED_TEXT = "Timer Stopped";
        private const string BUTTON_TIMER_START = "Start Timer";
        private const string BUTTON_TIMER_STOP = "Stop Timer";
        private const string BUTTON_SET_1H = "1h";
        private const string BUTTON_SET_2H = "2h";
        private const string BUTTON_SET_3H = "3h";
        private const string BUTTON_SET_4H = "4h";
        private const string BUTTON_SET_8H = "8h";
        private const string ERROR_NOT_ACTIVE_TITLE = "Can't start auto-off timer";
        private const string ERROR_NOT_ACTIVE = "Sorry!\nMacro must be active first!";
        #endregion

        #region Private Fields
        private readonly AutoOff autoOffModel;
        private readonly ToggleStateForm frmToggleApplication;
        private Button btnSet1Hours;
        private Button btnSet2Hours;
        private Button btnSet3Hours;
        private Button btnSet4Hours;
        private Button btnSet8Hours;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the AutoOff model instance for external control
        /// </summary>
        public AutoOff AutoOffModel => autoOffModel;
        #endregion

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #region Constructor
        public AutoOffForm(Subject subject, ToggleStateForm toggleStateForm)
        {
            InitializeComponent();

            this.AutoOffKey1.KeyDown += FormHelper.OnKeyDown;
            this.AutoOffKey1.KeyPress += FormHelper.OnKeyPress;
            this.AutoOffKey1.TextChanged += this.AutoOffKey1_TextChanged;

            this.AutoOffKey2.KeyDown += FormHelper.OnKeyDown;
            this.AutoOffKey2.KeyPress += FormHelper.OnKeyPress;
            this.AutoOffKey2.TextChanged += this.AutoOffKey2_TextChanged;

            subject.Attach(this);
            this.frmToggleApplication = toggleStateForm;

            // Initialize the AutoOff model
            autoOffModel = new AutoOff();

            // Subscribe to model events
            autoOffModel.TimerStarted += AutoOffModel_TimerStarted;
            autoOffModel.TimerStopped += AutoOffModel_TimerStopped;
            autoOffModel.TimerTick += AutoOffModel_TimerTick;
            autoOffModel.TimerCompleted += AutoOffModel_TimerCompleted;

            // Set trackbar maximum dynamically based on ServerMode
            trackBarTime.Maximum = autoOffModel.MaxMinutes;
            trackBarTime.Value = autoOffModel.SelectedMinutes;

            // Create dynamic buttons
            CreateDynamicButtons();

            // Apply colors to form elements
            ApplyFormColors();

            // Initialize UI
            UpdateUI();
        }
        #endregion

        #region Model Event Handlers
        private void AutoOffModel_TimerStarted(object sender, AutoOffEventArgs e)
        {
            btnToggleTimer.Text = BUTTON_TIMER_STOP;
            FormHelper.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.ResetButtonBackColor);
            animatedClockImage.Image = _ORTools.Resources.Media.Icons.clock_animated;
            UpdateUI();
        }

        private void AutoOffModel_TimerStopped(object sender, AutoOffEventArgs e)
        {
            btnToggleTimer.Text = BUTTON_TIMER_START;
            FormHelper.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
            animatedClockImage.Image = null;
            UpdateUI();
        }

        private void AutoOffModel_TimerTick(object sender, AutoOffEventArgs e)
        {
            UpdateUI();
        }

        private void AutoOffModel_TimerCompleted(object sender, AutoOffEventArgs e)
        {
            if (frmToggleApplication != null)
            {
                frmToggleApplication.toggleStatus();
                WeightLimitMacro.SendOverweightMacro();
            }
            else
            {
                DebugLogger.Error("AutoOffForm: Could not find 'ToggleApplicationStateForm' to toggle status.");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Stops the auto-off timer from external code
        /// </summary>
        public void StopAutoOffTimer()
        {
            autoOffModel.StopTimer();
        }

        /// <summary>
        /// Starts the auto-off timer from external code (if application is active)
        /// </summary>
        public bool StartAutoOffTimer()
        {
            if (!frmToggleApplication.IsApplicationOn())
            {
                return false;
            }
            return autoOffModel.StartTimer();
        }

        /// <summary>
        /// Gets whether the auto-off timer is currently running
        /// </summary>
        public bool IsAutoOffTimerRunning()
        {
            return autoOffModel.IsTimerRunning;
        }

        /// <summary>
        /// Sets the auto-off time in minutes
        /// </summary>
        public void SetAutoOffTime(int minutes)
        {
            autoOffModel.SetTime(minutes);
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }
        #endregion

        #region Observer Pattern
        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
                    autoOffModel.LoadFromProfile();
                    trackBarTime.Value = autoOffModel.SelectedMinutes;
                    UpdateUI();
                    this.AutoOffKey1.Text = prefs.AutoOffKey1.ToString();
                    this.AutoOffKey2.Text = prefs.AutoOffKey2.ToString();
                    this.AutoOffOverweightCB.Checked = prefs.AutoOffOverweight;
                    this.AutoOffKillClientChk.Checked = prefs.AutoOffKillClient;
                    break;
            }
        }
        #endregion

        #region UI Event Handlers
        private void TrackBarTime_Scroll(object sender, EventArgs e)
        {
            autoOffModel.SetTime(trackBarTime.Value);
            UpdateUI();
        }

        private void BtnToggleTimer_Click(object sender, EventArgs e)
        {
            if (autoOffModel.IsTimerRunning)
            {
                autoOffModel.StopTimer();
            }
            else
            {
                if (!frmToggleApplication.IsApplicationOn())
                {
                    DialogOK.ShowDialog(ERROR_NOT_ACTIVE, ERROR_NOT_ACTIVE_TITLE);
                }
                else
                {
                    autoOffModel.StartTimer();
                }
            }
        }

        private void BtnSet1Hours_Click(object sender, EventArgs e)
        {
            autoOffModel.SetTimeToOneHour();
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }

        private void BtnSet2Hours_Click(object sender, EventArgs e)
        {
            autoOffModel.SetTimeToTwoHours();
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }

        private void BtnSet3Hours_Click(object sender, EventArgs e)
        {
            autoOffModel.SetTimeToThreeHours();
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }

        private void BtnSet4Hours_Click(object sender, EventArgs e)
        {
            autoOffModel.SetTimeToFourHours();
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }

        private void BtnSet8Hours_Click(object sender, EventArgs e)
        {
            autoOffModel.SetTimeToEightHours();
            trackBarTime.Value = autoOffModel.SelectedMinutes;
            UpdateUI();
        }

        private void AutoOffKey1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.AutoOffKey1= key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error("Invalid key entered for AutoOffKey1: " + ex.Message);
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unexpected error in AutoOffKey1_TextChanged: " + ex.Message);
            }
        }

        private void AutoOffKey2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.AutoOffKey2= key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error("Invalid key entered for AutoOffKey2" + ex.Message);
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unexpected error in AutoOffKey2_TextChanged: " + ex.Message);
            }
        }

        private void AutoOffOverweight_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.AutoOffOverweight = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void AutoOffKillClientChk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.AutoOffKillClient = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        #endregion

        #region Private Methods
        private void UpdateUI()
        {
            lblSelectedTime.Text = autoOffModel.SelectedTimeText;

            if (autoOffModel.IsTimerRunning)
            {
                lblRemainingTime.Text = autoOffModel.RemainingTimeText;
                lblRemainingTimeText.Text = LABEL_REMAINING_TIME_TEXT;
                lblRemainingTimeText.Font = new Font(lblRemainingTimeText.Font, FontStyle.Regular);
            }
            else
            {
                lblRemainingTime.Text = "";
                lblRemainingTimeText.Text = LABEL_REMAINING_TIME_STOPPED_TEXT;
                lblRemainingTimeText.Font = new Font(lblRemainingTimeText.Font, FontStyle.Regular);
            }
        }

        private void CreateDynamicButtons()
        {
            int buttonWidth = 35;
            int buttonHeight = 24;
            int buttonSpacing = 4;
            int rightMargin = 12;
            int buttonY = btnToggleTimer.Location.Y;
            int currentX = ClientSize.Width - rightMargin;

            var buttonsToCreate = new List<(string Name, string Text, EventHandler Handler, int TabIndex, int Minutes)>();

            // Define buttons based on ServerMode, but only add those within MaxMinutes
            var potentialButtons = new List<(string Name, string Text, EventHandler Handler, int TabIndex, int Minutes)>();
            if (AppConfig.ServerMode == 0)
            {
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 8, 4 * 60));
                potentialButtons.Add(("btnSet3Hours", BUTTON_SET_3H, BtnSet3Hours_Click, 7, 3 * 60));
                potentialButtons.Add(("btnSet2Hours", BUTTON_SET_2H, BtnSet2Hours_Click, 6, 2 * 60));
                potentialButtons.Add(("btnSet1Hours", BUTTON_SET_1H, BtnSet1Hours_Click, 5, 1 * 60));
            }
            else if (AppConfig.ServerMode == 1)
            {
                potentialButtons.Add(("btnSet8Hours", BUTTON_SET_8H, BtnSet8Hours_Click, 8, 8 * 60));
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 7, 4 * 60));
                potentialButtons.Add(("btnSet2Hours", BUTTON_SET_2H, BtnSet2Hours_Click, 6, 2 * 60));
                potentialButtons.Add(("btnSet1Hours", BUTTON_SET_1H, BtnSet1Hours_Click, 5, 1 * 60));
            }
            else
            {
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 8, 4 * 60));
                potentialButtons.Add(("btnSet3Hours", BUTTON_SET_3H, BtnSet3Hours_Click, 7, 3 * 60));
                potentialButtons.Add(("btnSet2Hours", BUTTON_SET_2H, BtnSet2Hours_Click, 6, 2 * 60));
                potentialButtons.Add(("btnSet1Hours", BUTTON_SET_1H, BtnSet1Hours_Click, 5, 1 * 60));
            }

            // Filter buttons to only include those within MaxMinutes
            buttonsToCreate.AddRange(potentialButtons.Where(b => b.Minutes <= autoOffModel.MaxMinutes));

            foreach (var (name, text, handler, tabIndex, minutes) in buttonsToCreate)
            {
                var button = new Button
                {
                    Name = name,
                    Text = text,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(currentX - buttonWidth, buttonY),
                    Cursor = System.Windows.Forms.Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    UseVisualStyleBackColor = true,
                    TabIndex = tabIndex
                };
                button.Click += handler;
                Controls.Add(button);
                currentX -= buttonWidth + buttonSpacing;

                if (name == "btnSet8Hours") btnSet8Hours = button;
                else if (name == "btnSet4Hours") btnSet4Hours = button;
                else if (name == "btnSet3Hours") btnSet3Hours = button;
                else if (name == "btnSet2Hours") btnSet2Hours = button;
                else if (name == "btnSet1Hours") btnSet1Hours = button;
            }
        }

        private void ApplyFormColors()
        {
            FormHelper.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
            var dynamicButtonNames = new List<string>();
            if (btnSet1Hours != null) dynamicButtonNames.Add("btnSet1Hours");
            if (btnSet2Hours != null) dynamicButtonNames.Add("btnSet2Hours");
            if (btnSet3Hours != null) dynamicButtonNames.Add("btnSet3Hours");
            if (btnSet4Hours != null) dynamicButtonNames.Add("btnSet4Hours");
            if (btnSet8Hours != null) dynamicButtonNames.Add("btnSet8Hours");
            FormHelper.ApplyColorToButtons(this, dynamicButtonNames.ToArray(), AppConfig.CopyButtonBackColor);
        }
        #endregion

        #region Cleanup
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                autoOffModel?.Dispose();
                btnSet1Hours?.Dispose();
                btnSet2Hours?.Dispose();
                btnSet3Hours?.Dispose();
                btnSet4Hours?.Dispose();
                btnSet8Hours?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Unused Event Handlers (kept for Designer compatibility)
        private void AutoOffForm_Load(object sender, EventArgs e)
        {
        }
        #endregion

        private void toolTip2_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}