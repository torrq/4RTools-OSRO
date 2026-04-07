using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class CharacterInfo : Form
    {
        private string mapLink = "";

        // Bar data stored for OnPaint
        private uint _hpCur, _hpMax;
        private uint _spCur, _spMax;
        private uint _weightCur, _weightMax;

        // Line 1 text (Lv/Job/Exp) drawn in OnPaint
        private string _infoLine1 = "";

        private static readonly TimeSpan CacheStaleThreshold = TimeSpan.FromSeconds(2);
        private const int SLOW_REFRESH_MS = 2000;
        private const int MAP_REFRESH_MS  = 1000;

        private DateTime _lastSlowRefresh = DateTime.MinValue;
        private DateTime _lastMapRefresh  = DateTime.MinValue;

        private readonly Timer _refreshTimer;

        // Layout constants
        private const int INFO_ROW_Y   = 13;
        private const int INFO_ROW_H   = 11;
        private const int HP_BAR_Y     = 24;
        private const int SP_BAR_Y     = 36;
        private const int WT_BAR_Y     = 48;
        private const int BAR_H        = 12;
        private const int BAR_PAD      = 2;


        // Cached GDI objects — created once, reused every paint call
        private static readonly Font     _barFont    = new Font("Tahoma", 7.5f);
        private static readonly Font     _infoFont   = new Font("Tahoma", 7.5f);
        private static readonly SolidBrush _infoTextBrush = new SolidBrush(Color.FromArgb(90, 90, 90));
        private static readonly SolidBrush _barBgBrush    = new SolidBrush(Color.FromArgb(218, 223, 233));
        private static readonly SolidBrush _textBrush     = new SolidBrush(Color.FromArgb(30, 30, 30));
        private static readonly SolidBrush _shadowBrush   = new SolidBrush(Color.FromArgb(120, 255, 255, 255));
        private static readonly StringFormat _centerSf    = new StringFormat
        {
            Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap, Trimming = StringTrimming.EllipsisCharacter
        };
        private static readonly StringFormat _barSf       = new StringFormat
        {
            Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap
        };

        public CharacterInfo()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint  |
                ControlStyles.UserPaint, true);
            this.UpdateStyles();

            // Replace plain map label with LinkLabel
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
                TextAlign        = ContentAlignment.TopRight,
            };
            ll.LinkClicked += (s, e) => OpenMapLink();
            Controls.Remove(characterMapLabel);
            characterMapLabel = ll;
            Controls.Add(characterMapLabel);

            // Hide old text info label — replaced by bars
            characterInfoLabel.Visible = false;

            _refreshTimer = new Timer { Interval = 15 };
            _refreshTimer.Tick += RefreshTick;
            _refreshTimer.Start();

            if (components == null) components = new System.ComponentModel.Container();
            components.Add(_refreshTimer);
        }

        // ── Properties ────────────────────────────────────────────────────────

        public string CharacterNameLabel
        {
            get => characterNameLabel.Text;
            set => characterNameLabel.Text = value;
        }

        public string CharacterInfoLabel
        {
            get => _infoLine1;
            set
            {
                int nl = value?.IndexOf('\n') ?? -1;
                _infoLine1 = nl >= 0 ? value.Substring(0, nl) : (value ?? "");
            }
        }

        public string CharacterMapLabel
        {
            get => characterMapLabel.Text;
            set
            {
                characterMapLabel.Text = value;
                if (characterMapLabel is LinkLabel ll)
                {
                    ll.Links.Clear();
                    int len = value?.IndexOf(' ') ?? -1;
                    if (len < 0) len = value?.Length ?? 0;
                    if (len > 0) ll.Links.Add(0, len);
                }
            }
        }

        public string MapLink
        {
            get => mapLink;
            set => mapLink = value ?? "";
        }

        // ── Refresh ───────────────────────────────────────────────────────────

        private void RefreshTick(object sender, EventArgs e)
        {
            Client client = ClientSingleton.GetClient();
            if (client?.Process == null || client.Process.HasExited || !client.IsLoggedIn)
            {
                ClearLabels(); return;
            }

            var cached = HpSpCache.Latest;
            bool cacheIsFresh = cached.IsValid &&
                                (DateTime.UtcNow - cached.Timestamp) < CacheStaleThreshold;

            if (cacheIsFresh)
            {
                UpdateHpSpBars(cached.Snapshot);

                if ((DateTime.UtcNow - _lastMapRefresh).TotalMilliseconds >= MAP_REFRESH_MS)
                {
                    _lastMapRefresh = DateTime.UtcNow;

                    // Name and map
                    characterNameLabel.Text = client.ReadCharacterName() ?? "";
                    string map = client.ReadCurrentMap() ?? string.Empty;
                    this.CharacterMapLabel = map;
                    this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + map;

                    // Job/level/exp — single 52-byte RPM call, cheap
                    var jobSnap = client.ReadJobBlock();
                    if (jobSnap.HasValue)
                    {
                        var job = jobSnap.Value;
                        string expPct = job.ExpToLevel > 0
                            ? $"{(double)job.Exp / job.ExpToLevel * 100:0.00}%"
                            : "100%";
                        _infoLine1 = $"Lv{job.Level} / {JobList.GetNameById((int)job.JobId)} / Lv{job.JobLevel} / Exp {expPct}";
                        Invalidate(new Rectangle(0, INFO_ROW_Y, ClientSize.Width, INFO_ROW_H));
                    }

                    // Weight
                    var (wCur, wMax) = client.ReadWeight();
                    SetWeight(wCur, wMax);
                }
            }
            else
            {
                if ((DateTime.UtcNow - _lastSlowRefresh).TotalMilliseconds >= SLOW_REFRESH_MS)
                {
                    _lastSlowRefresh = DateTime.UtcNow;
                    UpdateCharacterInfo(client);
                }
            }
        }

        private void UpdateHpSpBars(Client.HpSpSnapshot snap)
        {
            if (snap.MaxHp == 0) return;
            bool changed = snap.CurrentHp != _hpCur || snap.MaxHp != _hpMax ||
                           snap.CurrentSp != _spCur || snap.MaxSp != _spMax;
            _hpCur = snap.CurrentHp; _hpMax = snap.MaxHp;
            _spCur = snap.CurrentSp; _spMax = snap.MaxSp;
            if (changed) Invalidate();
        }

        private void SetWeight(uint cur, uint max)
        {
            if (_weightCur == cur && _weightMax == max) return;
            _weightCur = cur; _weightMax = max;
            Invalidate();
        }

        public void UpdateCharacterInfo(Client client)
        {
            if (client?.Process == null) { ClearLabels(); return; }

            var hpSp    = client.ReadHpSp();
            var jobSnap = client.ReadJobBlock();
            string map  = client.ReadCurrentMap() ?? "";
            string name = client.ReadCharacterName();

            if (jobSnap == null) { ClearLabels(); return; }
            var job = jobSnap.Value;

            int lvl   = (int)job.Level;
            int jlvl  = (int)job.JobLevel;
            int jid   = (int)job.JobId;
            int expTo = (int)job.ExpToLevel;
            int exp   = (int)job.Exp;
            int hp    = (int)hpSp.CurrentHp;
            int maxHp = (int)hpSp.MaxHp;

            if (!(lvl > 0 && lvl <= 255 && jlvl > 0 && jlvl <= 255 && hp >= 0 && maxHp > 0 && hp <= maxHp))
            { ClearLabels(); return; }

            string expPct  = expTo > 0 ? $"{(double)exp / expTo * 100:0.00}%" : "100%";
            string jobName = JobList.GetNameById(jid);

            characterNameLabel.Text = name;
            _infoLine1 = $"Lv{lvl} / {jobName} / Lv{jlvl} / Exp {expPct}";
            this.CharacterMapLabel = map;
            this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + map;

            _hpCur = hpSp.CurrentHp; _hpMax = hpSp.MaxHp;
            _spCur = hpSp.CurrentSp; _spMax = hpSp.MaxSp;

            var (wCur, wMax) = client.ReadWeight();
            _weightCur = wCur; _weightMax = wMax;

            Invalidate();
        }

        private void ClearLabels()
        {
            characterNameLabel.Text = "";
            characterMapLabel.Text  = "";
            _infoLine1 = "";
            MapLink = "";
            _hpCur = _hpMax = _spCur = _spMax = _weightCur = _weightMax = 0;
            Invalidate();
        }

        // ── Painting ──────────────────────────────────────────────────────────

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(SystemBrushes.Window, ClientRectangle);

            // Info line (Lv / Job / Exp)
            if (!string.IsNullOrEmpty(_infoLine1))
            {
                var r = new RectangleF(BAR_PAD, INFO_ROW_Y, ClientSize.Width - BAR_PAD * 2, INFO_ROW_H);
                e.Graphics.DrawString(_infoLine1, _infoFont, _infoTextBrush, r, _centerSf);
            }

            // HP bar — turns red when low
            if (_hpMax > 0)
            {
                bool low = _hpCur < _hpMax * 0.25f;
                Color top = low ? Color.FromArgb(255, 80, 80) : Color.FromArgb(80, 255, 80);
                Color bot = low ? Color.FromArgb(200, 30, 30) : Color.FromArgb(20, 160, 20);
                DrawBar(e.Graphics, HP_BAR_Y, _hpCur, _hpMax, top, bot, $"HP  {_hpCur} / {_hpMax}");
            }

            // SP bar — turns orange when low
            if (_spMax > 0)
            {
                bool low = _spCur < _spMax * 0.25f;
                Color top = low ? Color.FromArgb(255, 180, 50) : Color.FromArgb(60, 180, 255);
                Color bot = low ? Color.FromArgb(210, 110, 0) : Color.FromArgb(0, 90, 220);
                DrawBar(e.Graphics, SP_BAR_Y, _spCur, _spMax, top, bot, $"SP  {_spCur} / {_spMax}");
            }

            // Weight bar — brownish red gradient
            if (_weightMax > 0)
            {
                int wPct = (int)Math.Round(_weightCur * 100.0 / _weightMax);
                Color top = Color.FromArgb(210, 100, 80);
                Color bot = Color.FromArgb(150, 45, 30);
                DrawBar(e.Graphics, WT_BAR_Y, _weightCur, _weightMax, top, bot, $"Weight  {_weightCur} / {_weightMax} ({wPct}%)");
            }
        }


        private void DrawBar(Graphics g, int y, uint cur, uint max, Color topColor, Color bottomColor, string label)
        {
            int x = BAR_PAD;
            int w = ClientSize.Width - BAR_PAD * 2;

            // Bar background
            g.FillRectangle(_barBgBrush, x, y, w, BAR_H);

            float ratio = Math.Min(1f, (float)cur / max);
            int fillW = (int)(w * ratio);
            if (fillW > 0)
            {
                var rectArea = new Rectangle(x, y, fillW, BAR_H);
                using (var brush = new LinearGradientBrush(rectArea, topColor, bottomColor, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, rectArea);
                }

                // 1-pixel solid end caps to the colored fill area
                using (var pen = new Pen(bottomColor, 1))
                {
                    g.DrawLine(pen, x, y, x, y + BAR_H - 1); // Left cap
                    g.DrawLine(pen, x + fillW - 1, y, x + fillW - 1, y + BAR_H - 1); // Right cap
                }
            }

            var rectText = new RectangleF(x, y, w, BAR_H);
            g.DrawString(label, _barFont, _shadowBrush, new RectangleF(x + 1, y + 1, w, BAR_H), _barSf);
            g.DrawString(label, _barFont, _textBrush, rectText, _barSf);
        }

        // ── Map link ──────────────────────────────────────────────────────────

        private void OpenMapLink()
        {
            if (!string.IsNullOrWhiteSpace(mapLink))
            {
                try { Process.Start(new ProcessStartInfo(mapLink) { UseShellExecute = true }); }
                catch (Exception ex) { MessageBox.Show("Failed to open link:\n" + ex.Message); }
            }
        }
    }
}
