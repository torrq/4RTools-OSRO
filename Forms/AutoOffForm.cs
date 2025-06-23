using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;

namespace _4RTools.Forms
{
    public partial class AutoOffForm : Form, IObserver
    {
        private readonly System.Windows.Forms.Timer autoOffTimer;
        private int selectedMinutes;
        private int remainingSeconds; // Track remaining time in seconds for precision
        private bool isTimerRunning;
        private const int MIN_MINUTES = 1; // 1 minute minimum
        private const int MAX_MINUTES = 8 * 60; // 8 hours maximum
        private const int THREE_HOURS = 3 * 60; // 3 hours in minutes
        private const int FOUR_HOURS = 4 * 60; // 4 hours in minutes
        private const int EIGHT_HOURS = 8 * 60; // 8 hours in minutes

        public AutoOffForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);

            // Initialize timer
            autoOffTimer = new System.Windows.Forms.Timer();
            autoOffTimer.Interval = 1000; // 1-second interval for countdown
            autoOffTimer.Tick += AutoOffTimer_Tick;

            // Set initial TrackBar value and update label
            trackBarTime.Value = MIN_MINUTES;
            selectedMinutes = MIN_MINUTES;
            UpdateTimeLabel();

            // Set initial button text
            btnToggleTimer.Text = "Start Timer";
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    // Reset timer if profile changes
                    StopTimer();
                    UpdateTimeLabel();
                    btnToggleTimer.Text = "Start Timer";
                    break;
            }
        }

        private void TrackBarTime_Scroll(object sender, EventArgs e)
        {
            selectedMinutes = trackBarTime.Value;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                // Restart timer with new duration if already running
                StopTimer();
                StartTimer();
            }
        }

        private void BtnToggleTimer_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                StopTimer();
                btnToggleTimer.Text = "Start Timer";
            }
            else
            {
                StartTimer();
                btnToggleTimer.Text = "Stop Timer";
            }
        }

        private void BtnSet3Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = THREE_HOURS;
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
                StartTimer();
            }
        }

        private void BtnSet4Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = FOUR_HOURS;
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
                StartTimer();
            }
        }

        private void BtnSet8Hours_Click(object sender, EventArgs e)
        {
            selectedMinutes = EIGHT_HOURS;
            trackBarTime.Value = selectedMinutes;
            UpdateTimeLabel();
            if (isTimerRunning)
            {
                StopTimer();
                StartTimer();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            StopTimer();
            selectedMinutes = MIN_MINUTES;
            trackBarTime.Value = MIN_MINUTES;
            UpdateTimeLabel();
            btnToggleTimer.Text = "Start Timer";
        }

        private void UpdateTimeLabel()
        {
            int hours = selectedMinutes / 60;
            int minutes = selectedMinutes % 60;
            string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
            lblSelectedTime.Text = $"Selected Time: {timeText}";
            UpdateRemainingTimeLabel();
        }

        private void UpdateRemainingTimeLabel()
        {
            if (isTimerRunning)
            {
                int remainingMinutes = (remainingSeconds + 59) / 60; // Ceiling to ensure accurate minute display
                int hours = remainingMinutes / 60;
                int minutes = remainingMinutes % 60;
                string timeText = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
                lblRemainingTime.Text = $"Remaining: {timeText}";
            }
            else
            {
                lblRemainingTime.Text = "Remaining: Not running";
            }
        }

        private void StartTimer()
        {
            if (selectedMinutes >= MIN_MINUTES)
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
            btnToggleTimer.Text = "Start Timer";
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
                DebugLogger.Debug($"Auto-off timer stopped at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Set duration: {timeText} ({selectedMinutes} minutes). Timer running: {isTimerRunning}.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                autoOffTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}