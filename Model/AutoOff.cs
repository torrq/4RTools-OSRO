using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;

namespace _4RTools.Model
{
    public class AutoOff : IDisposable
    {
        #region Constants
        private const int MIN_MINUTES = 1; // 1 minute minimum
        private const int ONE_HOUR = 60; // 1 hour in minutes
        private const int TWO_HOURS = 2 * 60; // 2 hours in minutes
        private const int THREE_HOURS = 3 * 60; // 3 hours in minutes
        private const int FOUR_HOURS = 4 * 60; // 4 hours in minutes
        private const int EIGHT_HOURS = 8 * 60; // 8 hours in minutes
        #endregion

        #region Events
        public event EventHandler<AutoOffEventArgs> TimerTick;
        public event EventHandler<AutoOffEventArgs> TimerStarted;
        public event EventHandler<AutoOffEventArgs> TimerStopped;
        public event EventHandler<AutoOffEventArgs> TimerCompleted;
        #endregion

        #region Private Fields
        private readonly System.Windows.Forms.Timer autoOffTimer;
        private int selectedMinutes;
        private int remainingSeconds;
        private bool isTimerRunning;
        private bool isInitializing;
        #endregion

        #region Public Properties
        public int SelectedMinutes
        {
            get => selectedMinutes;
            set
            {
                selectedMinutes = Math.Max(MIN_MINUTES, Math.Min(value, MaxMinutes));
                SaveToProfile();
            }
        }

        public int RemainingSeconds => remainingSeconds;

        public bool IsTimerRunning => isTimerRunning;

        public int MaxMinutes => AppConfig.ServerMode == 1 ? EIGHT_HOURS : FOUR_HOURS;

        public int MinMinutes => MIN_MINUTES;

        public string SelectedTimeText
        {
            get
            {
                int hours = selectedMinutes / 60;
                int minutes = selectedMinutes % 60;
                return hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
            }
        }

        public string RemainingTimeText
        {
            get
            {
                if (!isTimerRunning) return string.Empty;

                int remainingMinutes = (remainingSeconds + 59) / 60; // Ceiling for accurate display
                int hours = remainingMinutes / 60;
                int minutes = remainingMinutes % 60;
                return hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";
            }
        }
        #endregion

        #region Constructor
        public AutoOff()
        {
            autoOffTimer = new System.Windows.Forms.Timer();
            autoOffTimer.Interval = 1000; // 1-second interval
            autoOffTimer.Tick += AutoOffTimer_Tick;

            isInitializing = true;
            LoadFromProfile();
            isInitializing = false;
        }
        #endregion

        #region Public Methods
        public bool StartTimer()
        {
            if (selectedMinutes < MIN_MINUTES || selectedMinutes > MaxMinutes)
                return false;

            remainingSeconds = selectedMinutes * 60;
            autoOffTimer.Start();
            isTimerRunning = true;

            DebugLogger.Debug($"Auto-off timer started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Set duration: {SelectedTimeText} ({selectedMinutes} minutes). Timer running: {isTimerRunning}.");

            TimerStarted?.Invoke(this, new AutoOffEventArgs(selectedMinutes, remainingSeconds, isTimerRunning));
            return true;
        }

        public void StopTimer()
        {
            autoOffTimer.Stop();
            isTimerRunning = false;
            remainingSeconds = 0;

            TimerStopped?.Invoke(this, new AutoOffEventArgs(selectedMinutes, remainingSeconds, isTimerRunning));
        }

        public void LoadFromProfile()
        {
            int profileAutoOffTime = ProfileSingleton.GetCurrent().UserPreferences.AutoOffTime;
            selectedMinutes = Math.Max(MIN_MINUTES, Math.Min(profileAutoOffTime, MaxMinutes));
            StopTimer(); // Ensure timer is stopped to avoid conflicts
        }

        public void SetTime(int minutes)
        {
            selectedMinutes = Math.Max(MIN_MINUTES, Math.Min(minutes, MaxMinutes));
            SaveToProfile();
            if (isTimerRunning)
            {
                StopTimer();
            }
        }

        public void SetTimeToOneHour() => SetTime(ONE_HOUR);
        public void SetTimeToTwoHours() => SetTime(TWO_HOURS);
        public void SetTimeToThreeHours() => SetTime(THREE_HOURS);
        public void SetTimeToFourHours() => SetTime(FOUR_HOURS);
        public void SetTimeToEightHours() => SetTime(EIGHT_HOURS);
        #endregion

        #region Private Methods
        private void AutoOffTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            TimerTick?.Invoke(this, new AutoOffEventArgs(selectedMinutes, remainingSeconds, isTimerRunning));

            if (remainingSeconds <= 0)
            {
                DebugLogger.Debug($"Auto-off timer completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}. Set duration: {SelectedTimeText} ({selectedMinutes} minutes).");

                StopTimer();
                TimerCompleted?.Invoke(this, new AutoOffEventArgs(selectedMinutes, remainingSeconds, isTimerRunning));
            }
        }

        private void SaveToProfile()
        {
            if (!isInitializing && selectedMinutes > 0)
            {
                ProfileSingleton.GetCurrent().UserPreferences.AutoOffTime = selectedMinutes;
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            autoOffTimer?.Dispose();
        }
        #endregion
    }

    #region Event Args
    public class AutoOffEventArgs : EventArgs
    {
        public int SelectedMinutes { get; }
        public int RemainingSeconds { get; }
        public bool IsTimerRunning { get; }

        public AutoOffEventArgs(int selectedMinutes, int remainingSeconds, bool isTimerRunning)
        {
            SelectedMinutes = selectedMinutes;
            RemainingSeconds = remainingSeconds;
            IsTimerRunning = isTimerRunning;
        }
    }
    #endregion
}