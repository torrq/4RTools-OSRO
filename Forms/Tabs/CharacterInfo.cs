using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class CharacterInfo : Form
    {
        private string mapLink = "";
        private uint _weightCurrent = 0;
        private uint _weightMax = 0;
        private readonly ToolTip _weightTip = new ToolTip();

        // How old a cache entry can be before we consider macros "off" and do a full read
        private static readonly TimeSpan CacheStaleThreshold = TimeSpan.FromSeconds(2);

        private const int SLOW_REFRESH_MS = 2000;
        private const int MAP_REFRESH_MS = 1000;

        private readonly Timer _refreshTimer;

        public CharacterInfo()
        {
            InitializeComponent();
            this.CharacterNameLabel = "";
            this.CharacterInfoLabel = "";
            this.CharacterMapLabel = "";
            this.MapLink = "";

            // Replace plain label with LinkLabel so only the map name is a clickable link
            var ll = new LinkLabel
            {
                AutoSize         = characterMapLabel.AutoSize,
                Font             = characterMapLabel.Font,
                Location         = characterMapLabel.Location,
                Size             = characterMapLabel.Size,
                Cursor           = Cursors.Hand,
                LinkBehavior     = LinkBehavior.HoverUnderline,
                LinkColor        = characterMapLabel.ForeColor,
                ActiveLinkColor  = characterMapLabel.ForeColor,
                VisitedLinkColor = characterMapLabel.ForeColor,
                TextAlign        = System.Drawing.ContentAlignment.BottomRight,
            };
            ll.LinkClicked += (s, e) => OpenMapLink();
            Controls.Remove(characterMapLabel);
            characterMapLabel = ll;
            Controls.Add(characterMapLabel);

            _refreshTimer = new Timer { Interval = 15 };
            _refreshTimer.Tick += RefreshTick;
            _refreshTimer.Start();

            // Register with the designer's components container so Dispose() in Designer.cs cleans it up
            if (components == null) components = new System.ComponentModel.Container();
            components.Add(_refreshTimer);
            components.Add(_weightTip);

            // Weight bar — 2px tall, spans full width, sits at the very bottom of the form
            _weightTip.SetToolTip(this, "");
            _weightTip.InitialDelay = 400;
            _weightTip.AutoPopDelay = 2000;
            _weightTip.ReshowDelay = 200;
        }

        public string CharacterNameLabel
        {
            get { return characterNameLabel.Text; }
            set { characterNameLabel.Text = value; }
        }

        public string CharacterInfoLabel
        {
            get { return characterInfoLabel.Text; }
            set { characterInfoLabel.Text = value; }
        }

        public string CharacterMapLabel
        {
            get { return characterMapLabel.Text; }
            set
            {
                characterMapLabel.Text = value;
                // Set link area to cover only the map name (up to first space, or whole string)
                if (characterMapLabel is LinkLabel ll)
                {
                    ll.Links.Clear();
                    int linkLen = value.IndexOf(' ');
                    if (linkLen < 0) linkLen = value.Length;
                    if (linkLen > 0) ll.Links.Add(0, linkLen);
                }
            }
        }

        public string MapLink
        {
            get { return mapLink; }
            set { mapLink = value ?? ""; }
        }

        private DateTime _lastSlowRefresh = DateTime.MinValue;
        private DateTime _lastMapRefresh = DateTime.MinValue;

        /// <summary>
        /// Timer callback — fires every 500 ms on the UI thread.
        /// Uses the HpSpCache written by macro threads when fresh; falls back to a
        /// full re-read every SLOW_REFRESH_MS when macros are idle.
        /// </summary>
        private void RefreshTick(object sender, EventArgs e)
        {
            Client client = ClientSingleton.GetClient();
            if (client?.Process == null || client.Process.HasExited || !client.IsLoggedIn)
            {
                ClearLabels();
                return;
            }

            var cached = HpSpCache.Latest;
            bool cacheIsFresh = cached.IsValid &&
                                (DateTime.UtcNow - cached.Timestamp) < CacheStaleThreshold;

            if (cacheIsFresh)
            {
                // Fast path — just repaint the HP/SP line; no RPM call needed
                UpdateHpSpLine(cached.Snapshot);

                // Map can change independently — refresh it on its own interval
                if ((DateTime.UtcNow - _lastMapRefresh).TotalMilliseconds >= MAP_REFRESH_MS)
                {
                    _lastMapRefresh = DateTime.UtcNow;
                    string map = client.ReadCurrentMap() ?? string.Empty;
                    this.CharacterMapLabel = map;
                    this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + map;
                    var (wCur, wMax) = client.ReadWeight();
                    UpdateWeightBar(wCur, wMax);
                }
            }
            else
            {
                // Macros aren't running; do a full refresh at a slower rate
                if ((DateTime.UtcNow - _lastSlowRefresh).TotalMilliseconds >= SLOW_REFRESH_MS)
                {
                    _lastSlowRefresh = DateTime.UtcNow;
                    UpdateCharacterInfo(client);
                }
            }
        }

        /// <summary>
        /// Updates only the HP/SP line from an already-read snapshot — zero RPM calls.
        /// </summary>
        public void UpdateHpSpLine(Client.HpSpSnapshot snap)
        {
            if (snap.MaxHp == 0) return; // zeroed snapshot, nothing useful
            string hpLine = $"HP {snap.CurrentHp} / {snap.MaxHp} | SP {snap.CurrentSp} / {snap.MaxSp}";

            // CharacterInfoLabel is "line1\nline2" — preserve line1, replace line2
            string current = this.CharacterInfoLabel;
            int nl = current.IndexOf('\n');
            string line1 = nl >= 0 ? current.Substring(0, nl) : current;
            this.CharacterInfoLabel = line1 + "\n" + hpLine;

            characterInfoLabel.HpLow = snap.MaxHp > 0 && snap.CurrentHp < snap.MaxHp * 0.25;
            characterInfoLabel.SpLow = snap.MaxSp > 0 && snap.CurrentSp < snap.MaxSp * 0.25;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_weightMax == 0) return;

            const int BAR_HEIGHT = 8;
            float ratio = Math.Min(1f, (float)_weightCurrent / _weightMax);
            int barY = 0;
            int barWidth = (int)(this.ClientSize.Width * ratio);

            if (barWidth <= 0) return;

            // Gradient: green → yellow → red over 0–90%, solid red from 90–100%
            using (var bmp = new System.Drawing.Bitmap(barWidth, BAR_HEIGHT))
            {
                for (int x = 0; x < barWidth; x++)
                {
                    float t = Math.Min(1f, (x / (float)(this.ClientSize.Width * 0.9f)));
                    Color px;
                    if (ratio >= 0.9f && x >= (int)(this.ClientSize.Width * 0.9f))
                    {
                        px = Color.FromArgb(210, 60, 60); // solid red tail
                    }
                    else if (t < 0.5f)
                    {
                        float s = t / 0.5f;
                        px = Color.FromArgb(
                            (int)(80  + s * (220 - 80)),
                            (int)(200 + s * (180 - 200)),
                            (int)(80  + s * (0   - 80)));
                    }
                    else
                    {
                        float s = (t - 0.5f) / 0.5f;
                        px = Color.FromArgb(
                            (int)(220 + s * (210 - 220)),
                            (int)(180 + s * (60  - 180)),
                            (int)(0   + s * (60  - 0)));
                    }
                    for (int y = 0; y < BAR_HEIGHT; y++)
                        bmp.SetPixel(x, y, px);
                }
                e.Graphics.DrawImage(bmp, 0, barY);
            }
        }

        private void UpdateWeightBar(uint current, uint max)
        {
            if (_weightCurrent == current && _weightMax == max) return;
            _weightCurrent = current;
            _weightMax = max;
            if (max > 0)
            {
                int pct = (int)Math.Round(current * 100.0 / max);
                _weightTip.SetToolTip(this, $"Weight: {current} / {max} ({pct}%)");
            }
            else
                _weightTip.SetToolTip(this, "");
            Invalidate(new Rectangle(0, 0, this.ClientSize.Width, 8));
        }

        private void ClearLabels()
        {
            this.CharacterNameLabel = "";
            this.CharacterInfoLabel = "";
            this.CharacterMapLabel  = "";
            this.MapLink            = "";
            UpdateWeightBar(0, 0);
        }

        /// <summary>
        /// Updates character information with client data and formats it for display
        /// </summary>
        public void UpdateCharacterInfo(Client client)
        {
            // Check if client is null, has no process, or is not logged in
            if (client?.Process == null || !IsClientLoggedIn(client))
            {
                ClearLabels();
                return;
            }

            // Read all data in bulk — 3 RPM calls instead of 10
            var hpSp   = client.ReadHpSp();
            var jobSnap = client.ReadJobBlock();
            string currentMap = client.ReadCurrentMap() ?? "";
            string characterName = client.ReadCharacterName();

            if (jobSnap == null) { ClearLabels(); return; }
            var job = jobSnap.Value;

            int currentLevel     = (int)job.Level;
            int currentJobLevel  = (int)job.JobLevel;
            int currentJobId     = (int)job.JobId;
            int currentExpToLevel = (int)job.ExpToLevel;
            int currentExp       = (int)job.Exp;
            int currentHP        = (int)hpSp.CurrentHp;
            int currentMaxHP     = (int)hpSp.MaxHp;
            int currentSP        = (int)hpSp.CurrentSp;
            int currentMaxSP     = (int)hpSp.MaxSp;

            // Validate data (example: check if level is reasonable)
            if (!IsValidCharacterData(currentLevel, currentJobLevel, currentHP, currentMaxHP))
            {
                ClearLabels();
                return;
            }

            // Calculate experience percentage
            string currentExpPercent;
            if (currentExpToLevel > 0)
            {
                double ratio = (double)currentExp / currentExpToLevel;
                currentExpPercent = $"{(ratio * 100):0.00}%";
            }
            else
            {
                currentExpPercent = "100%";
            }

            // Get job name
            string jobName = JobList.GetNameById(currentJobId);

            // Format the multi-line info text
            string line1 = $"Lv{currentLevel} / {jobName} / Lv{currentJobLevel} / Exp {currentExpPercent}";
            string line2 = $"HP {currentHP} / {currentMaxHP} | SP {currentSP} / {currentMaxSP}";

            string clientDebugInfo = line1 + "\n" + line2;

            // Update the form
            this.CharacterNameLabel = characterName;
            this.CharacterInfoLabel = clientDebugInfo;
            this.CharacterMapLabel = currentMap;
            this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + currentMap;
            var (wCur2, wMax2) = client.ReadWeight();
            UpdateWeightBar(wCur2, wMax2);

            characterInfoLabel.HpLow = currentMaxHP > 0 && currentHP < currentMaxHP * 0.25;
            characterInfoLabel.SpLow = currentMaxSP > 0 && currentSP < currentMaxSP * 0.25;
        }

        // Helper method to check if client is logged in
        private bool IsClientLoggedIn(Client client)
        {
            // Replace with actual logic to check if client is logged in
            // Example: return client.IsLoggedIn; // Assuming Client has an IsLoggedIn property
            // If no such property exists, you might check if characterName is non-empty or other indicators
            return true;
            //!string.IsNullOrEmpty(client.ReadCharacterName());
        }

        // Helper method to validate character data
        private bool IsValidCharacterData(int level, int jobLevel, int hp, int maxHP)
        {
            //DebugLogger.Debug($"Validating character data: Level={level}, JobLevel={jobLevel}, HP={hp}, MaxHP={maxHP}");
            // Example validation: ensure level and HP are within reasonable ranges
            return level > 0 && level <= 255 && // Adjust max level based on game
                   jobLevel > 0 && jobLevel <= 255 && // Adjust max job level based on game
                   hp >= 0 && maxHP > 0 && hp <= maxHP;
        }

        private void OpenMapLink()
        {
            if (!string.IsNullOrWhiteSpace(mapLink))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(mapLink) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open link:\n" + ex.Message);
                }
            }
        }
    }
}