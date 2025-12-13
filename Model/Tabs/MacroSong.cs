using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
{
    /// <summary>
    /// Represents a song row configuration with trigger, sequence, adaptation, and instrument
    /// </summary>
    public class SongRow
    {
        public int Id { get; set; }
        public Keys TriggerKey { get; set; } = Keys.None;
        public Keys AdaptationKey { get; set; } = Keys.None;
        public Keys InstrumentKey { get; set; } = Keys.None;
        public int Delay { get; set; } = AppConfig.MacroDefaultDelay;

        /// <summary>
        /// Array of 8 song keys in sequence
        /// </summary>
        public Keys[] SongSequence { get; set; }

        public SongRow()
        {
            InitializeSequence();
        }

        public SongRow(int id) : this()
        {
            this.Id = id;
        }

        [JsonConstructor]
        public SongRow(int id, Keys triggerKey, Keys adaptationKey, Keys instrumentKey, int delay, Keys[] songSequence)
        {
            this.Id = id;
            this.TriggerKey = triggerKey;
            this.AdaptationKey = adaptationKey;
            this.InstrumentKey = instrumentKey;
            this.Delay = delay;
            this.SongSequence = songSequence;

            // Ensure sequence is properly initialized
            if (this.SongSequence == null || this.SongSequence.Length != 8)
            {
                InitializeSequence();
            }
        }

        private void InitializeSequence()
        {
            SongSequence = new Keys[8];
            for (int i = 0; i < 8; i++)
            {
                SongSequence[i] = Keys.None;
            }
        }

        /// <summary>
        /// Gets the active (non-None) song keys in sequence order
        /// </summary>
        public List<Keys> GetActiveSongKeys()
        {
            List<Keys> activeKeys = new List<Keys>();
            foreach (var key in SongSequence)
            {
                if (key != Keys.None)
                {
                    activeKeys.Add(key);
                }
            }
            return activeKeys;
        }

        /// <summary>
        /// Resets this row to default values
        /// </summary>
        public void Reset()
        {
            TriggerKey = Keys.None;
            AdaptationKey = Keys.None;
            InstrumentKey = Keys.None;
            Delay = AppConfig.MacroDefaultDelay;
            for (int i = 0; i < 8; i++)
            {
                SongSequence[i] = Keys.None;
            }
        }
    }

    /// <summary>
    /// Dedicated Song Macro class for bard/dancer songs
    /// </summary>
    public class MacroSong : IAction
    {
        public static string ACTION_NAME = "SongMacro";

        public string ActionName { get; set; } = ACTION_NAME;
        private ThreadRunner thread;
        public List<SongRow> SongRows { get; set; } = new List<SongRow>();

        public MacroSong()
        {
            InitializeSongRows();
        }

        [JsonConstructor]
        public MacroSong(string actionName, List<SongRow> songRows)
        {
            this.ActionName = actionName ?? ACTION_NAME;
            this.SongRows = songRows ?? new List<SongRow>();

            // Ensure we have the correct number of rows based on current config
            EnsureCorrectRowCount();
        }

        [JsonIgnore]
        private bool isInitialized = false;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (!isInitialized)
            {
                EnsureCorrectRowCount();
                isInitialized = true;
            }
        }

        private void InitializeSongRows()
        {
            // Initialize rows based on config
            int totalRows = ConfigGlobal.GetConfig().SongRows;
            for (int i = 1; i <= totalRows; i++)
            {
                SongRows.Add(new SongRow(i));
            }
        }

        private void EnsureCorrectRowCount()
        {
            int totalRows = ConfigGlobal.GetConfig().SongRows;

            // Add missing rows if config has more rows than saved data
            while (SongRows.Count < totalRows)
            {
                SongRows.Add(new SongRow(SongRows.Count + 1));
            }

            // Remove extra rows if config has fewer rows than saved data
            while (SongRows.Count > totalRows)
            {
                SongRows.RemoveAt(SongRows.Count - 1);
            }
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Resets a specific song row to default values
        /// </summary>
        public void ResetSongRow(int rowId)
        {
            try
            {
                var songRow = SongRows.Find(row => row.Id == rowId);
                songRow?.Reset();
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in SongMacro.ResetSongRow: {ex}");
            }
        }

        /// <summary>
        /// Gets a song row by ID
        /// </summary>
        public SongRow GetSongRow(int rowId)
        {
            return SongRows.Find(row => row.Id == rowId);
        }

        private int SongMacroThread(Client roClient)
        {
            foreach (SongRow songRow in this.SongRows)
            {
                if (songRow.TriggerKey != Keys.None && Win32Interop.IsKeyPressed(songRow.TriggerKey))
                {
                    List<Keys> activeSongKeys = songRow.GetActiveSongKeys();

                    // Only proceed if there are active song keys
                    if (activeSongKeys.Count > 0)
                    {
                        // Equip instrument if specified
                        if (songRow.InstrumentKey != Keys.None)
                        {
                            Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, songRow.InstrumentKey, 0);
                            Thread.Sleep(30);
                        }

                        // Cast songs with adaptation between each step
                        for (int i = 0; i < activeSongKeys.Count; i++)
                        {
                            // Cast the song key
                            Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, activeSongKeys[i], 0);
                            Thread.Sleep(songRow.Delay);

                            // Send adaptation key after each song step (including the last one)
                            if (songRow.AdaptationKey != Keys.None)
                            {
                                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, songRow.AdaptationKey, 0);
                                Thread.Sleep(songRow.Delay);
                            }
                        }
                    }
                }
            }
            Thread.Sleep(100);
            return 0;
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                }
                this.thread = new ThreadRunner((_) => SongMacroThread(roClient), "SongMacro");
                ThreadRunner.Start(this.thread);
            }
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                ThreadRunner.Stop(this.thread);
                this.thread.Terminate();
                this.thread = null;
            }
        }
    }
}