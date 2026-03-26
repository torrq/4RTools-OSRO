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

        // Tooltip for bar hover
        private readonly ToolTip _barTip = new ToolTip { InitialDelay = 300, AutoPopDelay = 2500, ReshowDelay = 100 };
        private string _lastTip = "";

        // Layout constants
        private const int INFO_ROW_Y   = 13;
        private const int INFO_ROW_H   = 11;
        private const int HP_BAR_Y     = 25;
        private const int SP_BAR_Y     = 37;
        private const int WT_BAR_Y     = 49;
        private const int BAR_H        = 12;
        private const int BAR_PAD      = 2;


        // Cached GDI objects — created once, reused every paint call
        private static readonly Font     _barFont    = new Font("Tahoma", 7.5f);
        private static readonly Font     _infoFont   = new Font("Tahoma", 7.5f);
        private static readonly SolidBrush _infoTextBrush = new SolidBrush(Color.FromArgb(90, 90, 90));
        private static readonly SolidBrush _barBgBrush    = new SolidBrush(Color.FromArgb(218, 223, 233));
        private static readonly SolidBrush _textBrush     = new SolidBrush(Color.FromArgb(30, 30, 30));
        private static readonly SolidBrush _shadowBrush   = new SolidBrush(Color.FromArgb(120, 255, 255, 255));
        private static readonly SolidBrush _hpBrush       = new SolidBrush(Color.FromArgb(33, 219, 31));
        private static readonly SolidBrush _hpLowBrush    = new SolidBrush(Color.FromArgb(220, 55, 55));
        private static readonly SolidBrush _spBrush       = new SolidBrush(Color.FromArgb(0, 111, 245));
        private static readonly SolidBrush _spLowBrush    = new SolidBrush(Color.FromArgb(230, 140, 30));
        private static readonly SolidBrush _wtBrush       = new SolidBrush(Color.FromArgb(185, 65, 50));
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
            components.Add(_barTip);

            this.MouseMove += OnMouseMoveBar;
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
            if (changed) Invalidate(new Rectangle(0, HP_BAR_Y, ClientSize.Width, BAR_H * 2 + 2));
            // Also refresh weight bar since it sits below SP
        }

        private void SetWeight(uint cur, uint max)
        {
            if (_weightCur == cur && _weightMax == max) return;
            _weightCur = cur; _weightMax = max;
            Invalidate(new Rectangle(0, WT_BAR_Y, ClientSize.Width, BAR_H));
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
                DrawBar(e.Graphics, HP_BAR_Y, _hpCur, _hpMax,
                    _hpCur < _hpMax * 0.25f ? _hpLowBrush : _hpBrush,
                    $"HP  {_hpCur} / {_hpMax}");

            // SP bar — turns orange when low
            if (_spMax > 0)
                DrawBar(e.Graphics, SP_BAR_Y, _spCur, _spMax,
                    _spCur < _spMax * 0.25f ? _spLowBrush : _spBrush,
                    $"SP  {_spCur} / {_spMax}");

            // Weight bar — flat red, shows percentage
            if (_weightMax > 0)
            {
                int wPct = (int)Math.Round(_weightCur * 100.0 / _weightMax);
                DrawBar(e.Graphics, WT_BAR_Y, _weightCur, _weightMax, _wtBrush,
                    $"Weight  {wPct}%");
            }
        }


        private void DrawBar(Graphics g, int y, uint cur, uint max, SolidBrush fillBrush, string label)
        {
            int x = BAR_PAD;
            int w = ClientSize.Width - BAR_PAD * 2;

            g.FillRectangle(_barBgBrush, x, y, w, BAR_H);

            float ratio = Math.Min(1f, (float)cur / max);
            int fillW = (int)(w * ratio);
            if (fillW > 0)
                g.FillRectangle(fillBrush, x, y, fillW, BAR_H);

            var rect = new RectangleF(x, y, w, BAR_H);
            g.DrawString(label, _barFont, _shadowBrush, new RectangleF(x + 1, y + 1, w, BAR_H), _barSf);
            g.DrawString(label, _barFont, _textBrush, rect, _barSf);
        }

        // ── Tooltip on hover ──────────────────────────────────────────────────

        private void OnMouseMoveBar(object sender, MouseEventArgs e)
        {
            string tip = "";
            if (e.Y >= HP_BAR_Y && e.Y < HP_BAR_Y + BAR_H && _hpMax > 0)
                tip = $"HP: {_hpCur} / {_hpMax}";
            else if (e.Y >= SP_BAR_Y && e.Y < SP_BAR_Y + BAR_H && _spMax > 0)
                tip = $"SP: {_spCur} / {_spMax}";
            else if (e.Y >= WT_BAR_Y && e.Y < WT_BAR_Y + BAR_H && _weightMax > 0)
            {
                int pct = (int)Math.Round(_weightCur * 100.0 / _weightMax);
                tip = $"Weight: {_weightCur} / {_weightMax} ({pct}%)";
            }

            if (tip != _lastTip)
            {
                _lastTip = tip;
                if (string.IsNullOrEmpty(tip)) _barTip.Hide(this);
                else _barTip.Show(tip, this, e.X + 12, e.Y + 12);
            }
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
