using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace _4RTools.Forms
{
    public partial class AutoOffForm : Form, IObserver
    {
        private readonly System.Windows.Forms.Timer autoOffTimer;
        private int selectedMinutes;
        private int remainingSeconds; // Track remaining time in seconds for precision
        private bool isTimerRunning;
        private bool isInitializing; // Flag to prevent saving during initialization
        private const int MIN_MINUTES = 1; // 1 minute minimum
        private const int ONE_HOUR = 60; // 1 hour in minutes
        private const int THREE_HOURS = 3 * 60; // 3 hours in minutes
        private const int FOUR_HOURS = 4 * 60; // 4 hours in minutes
        private const int EIGHT_HOURS = 8 * 60; // 8 hours in minutes
        private readonly ToggleStateForm frmToggleApplication;
        private Button btnSet1Hours;
        private Button btnSet3Hours;
        private Button btnSet4Hours;
        private Button btnSet8Hours;
        private const string LABEL_REMAINING_TIME_TEXT = "Remaining Time:";
        private const string LABEL_REMAINING_TIME_STOPPED_TEXT = "Timer Stopped";
        private const string BUTTON_TIMER_START = "Start Timer";
        private const string BUTTON_TIMER_STOP = "Stop Timer";
        private const string BUTTON_SET_1H = "1h";
        private const string BUTTON_SET_3H = "3h";
        private const string BUTTON_SET_4H = "4h";
        private const string BUTTON_SET_8H = "8h";
        private const string ERROR_NOT_ACTIVE_TITLE = "Can't start auto-off timer";
        private const string ERROR_NOT_ACTIVE = "Sorry!\nMacro must be active first!";

        private int MaxMinutes => AppConfig.ServerMode == 1 ? EIGHT_HOURS : FOUR_HOURS; // Dynamic maximum based on ServerMode

        public AutoOffForm(Subject subject, ToggleStateForm toggleStateForm)
        {
            InitializeComponent();
            subject.Attach(this);

            this.frmToggleApplication = toggleStateForm;

            // Set trackbar maximum dynamically based on ServerMode
            trackBarTime.Maximum = MaxMinutes;

            // Initialize timer
            autoOffTimer = new System.Windows.Forms.Timer();
            autoOffTimer.Interval = 1000; // 1-second interval for countdown
            autoOffTimer.Tick += AutoOffTimer_Tick;

            // Prevent saving to profile during initialization
            isInitializing = true;

            // Load initial auto-off time from profile
            LoadAutoOffTimeFromProfile();

            // Create dynamic buttons
            CreateDynamicButtons();

            // Apply colors to buttons
            FormUtils.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
            var dynamicButtonNames = new List<string>();
            if (btnSet1Hours != null) dynamicButtonNames.Add("btnSet1Hours");
            if (btnSet3Hours != null) dynamicButtonNames.Add("btnSet3Hours");
            if (btnSet4Hours != null) dynamicButtonNames.Add("btnSet4Hours");
            if (btnSet8Hours != null) dynamicButtonNames.Add("btnSet8Hours");
            FormUtils.ApplyColorToButtons(this, dynamicButtonNames.ToArray(), AppConfig.CopyButtonBackColor);

            // Initialization complete, allow saving
            isInitializing = false;
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
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 7, FOUR_HOURS));
                potentialButtons.Add(("btnSet3Hours", BUTTON_SET_3H, BtnSet3Hours_Click, 6, THREE_HOURS));
                potentialButtons.Add(("btnSet1Hours", BUTTON_SET_1H, BtnSet1Hours_Click, 5, ONE_HOUR));
            }
            else if (AppConfig.ServerMode == 1)
            {
                potentialButtons.Add(("btnSet8Hours", BUTTON_SET_8H, BtnSet8Hours_Click, 7, EIGHT_HOURS));
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 6, FOUR_HOURS));
                potentialButtons.Add(("btnSet3Hours", BUTTON_SET_3H, BtnSet3Hours_Click, 5, THREE_HOURS));
            }
            else
            {
                potentialButtons.Add(("btnSet4Hours", BUTTON_SET_4H, BtnSet4Hours_Click, 7, FOUR_HOURS));
                potentialButtons.Add(("btnSet3Hours", BUTTON_SET_3H, BtnSet3Hours_Click, 6, THREE_HOURS));
                potentialButtons.Add(("btnSet1Hours", BUTTON_SET_1H, BtnSet1Hours_Click, 5, ONE_HOUR));
            }

            // Filter buttons to only include those within MaxMinutes
            buttonsToCreate.AddRange(potentialButtons.Where(b => b.Minutes <= MaxMinutes));

            foreach (var (name, text, handler, tabIndex, minutes) in buttonsToCreate)
            {
                var button = new Button
                {
                    Name = name,
                    Text = text,
                    Size = new Size(buttonWidth, buttonHeight),
                    Location = new Point(currentX - buttonWidth, buttonY),
                    Cursor = Cursors.Hand,
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
                else if (name == "btnSet1Hours") btnSet1Hours = button;
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    // Load auto-off time from new profile
                    LoadAutoOffTimeFromProfile();
                    btnToggleTimer.Text = BUTTON_TIMER_START;
                    FormUtils.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
                    break;
            }
        }

        // Public method to load auto-off time from profile
        public void LoadAutoOffTimeFromProfile()
        {
            int profileAutoOffTime = ProfileSingleton.GetCurrent().UserPreferences.AutoOffTime;
            // Validate against MIN_MINUTES and MaxMinutes
            selectedMinutes = Math.Max(MIN_MINUTES, Math.Min(profileAutoOffTime, MaxMinutes));
            trackBarTime.Value = selectedMinutes;
            StopTimer(); // Ensure timer is stopped to avoid conflicts
            UpdateTimeLabel();
        }

        private void TrackBarTime_Scroll(object sender, EventArgs e)
        {
            selectedMinutes = Math.Min(trackBarTime.Value, MaxMinutes); // Ensure within bounds
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
                //StartTimer();
            }
        }

        private void BtnToggleTimer_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                StopTimer();
                btnToggleTimer.Text = BUTTON_TIMER_START;
                FormUtils.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
            }
            else
            {
                if (!frmToggleApplication.IsApplicationOn())
                {
                    DialogOK.ShowDialog(ERROR_NOT_ACTIVE, ERROR_NOT_ACTIVE_TITLE);
                }
                else
                {
                    StartTimer();
                    btnToggleTimer.Text = BUTTON_TIMER_STOP;
                    FormUtils.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.ResetButtonBackColor);
                }
            }
        }

        private void BtnSet1Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = Math.Min(ONE_HOUR, MaxMinutes);
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
            }
        }

        private void BtnSet3Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = Math.Min(THREE_HOURS, MaxMinutes);
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
            }
        }

        private void BtnSet4Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = Math.Min(FOUR_HOURS, MaxMinutes);
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
            }
        }

        private void BtnSet8Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = Math.Min(EIGHT_HOURS, MaxMinutes);
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
            }
        }

        private void UpdateTimeLabel()
        {
            int hours = selectedMinutes / 60;
            int minutes = selectedMinutes % 60;
            string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
            lblSelectedTime.Text = $"{timeText}";
            UpdateRemainingTimeLabel();
            if (!isInitializing && selectedMinutes > 0)
            {
                ProfileSingleton.GetCurrent().UserPreferences.AutoOffTime = selectedMinutes;
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
            }
        }

        private void UpdateRemainingTimeLabel()
        {
            if (isTimerRunning)
            {
                int remainingMinutes = (remainingSeconds + 59) / 60; // Ceiling to ensure accurate minute display
                int hours = remainingMinutes / 60;
                int minutes = remainingMinutes % 60;
                string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
                lblRemainingTime.Text = $"{timeText}";
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

        private void StartTimer()
        {
            if (selectedMinutes >= MIN_MINUTES && selectedMinutes <= MaxMinutes)
            {
                remainingSeconds = selectedMinutes * 60; // Convert to seconds for countdown
                autoOffTimer.Start();
                isTimerRunning = true;
                UpdateRemainingTimeLabel();
                int hours = selectedMinutes / 60;
                int minutes = selectedMinutes % 60;
                string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
                DebugLogger.Debug($"Auto-off timer started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Set duration: {timeText} ({selectedMinutes} minutes). Timer running: {isTimerRunning}.");
            }
        }

        private void StopTimer()
        {
            autoOffTimer.Stop();
            isTimerRunning = false;
            remainingSeconds = 0;
            UpdateRemainingTimeLabel();
            btnToggleTimer.Text = BUTTON_TIMER_START;
            FormUtils.ApplyColorToButtons(this, new[] { "btnToggleTimer" }, AppConfig.CreateButtonBackColor);
        }

        private void AutoOffTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateRemainingTimeLabel();
            if (remainingSeconds <= 0)
            {
                int hours = selectedMinutes / 60;
                int minutes = selectedMinutes % 60;
                string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
                StopTimer();

                if (frmToggleApplication != null)
                {
                    frmToggleApplication.toggleStatus();
                    DebugLogger.Debug($"Auto-off timer stopped at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Set duration: {timeText} ({selectedMinutes} minutes). Timer running: {isTimerRunning}.");
                }
                else
                {
                    DebugLogger.Error("AutoOffForm: Could not find 'ToggleApplicationStateForm' to toggle status.");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                autoOffTimer?.Dispose();
                btnSet1Hours?.Dispose();
                btnSet3Hours?.Dispose();
                btnSet4Hours?.Dispose();
                btnSet8Hours?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AutoOffForm_Load(object sender, EventArgs e)
        {

        }
    }
}