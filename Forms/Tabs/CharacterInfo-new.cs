using _ORTools.Model;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class CharacterInfo : Form
    {
        private uint _weightCurrent = 0;
        private uint _weightMax = 0;
        private int _hpCur, _hpMax;
        private int _spCur, _spMax;

        // Display Data
        private string _job = "Sniper";
        private int _baseLevel = 245;
        private int _jobLevel = 100;
        private string _percentToNext = "100%";

        private readonly ToolTip _barsToolTip = new ToolTip();
        private string _currentTooltip = "";

        private readonly Timer _refreshTimer;

        // Dynamically created labels to avoid Designer edits
        private Label _lblLevel;
        private Label _lblJob;

        public CharacterInfo()
        {
            InitializeComponent();
            SetupModernUI();

            _refreshTimer = new Timer { Interval = 15 };
            _refreshTimer.Tick += RefreshTick;
            _refreshTimer.Start();

            // Hook up mouse movement for bar tooltips
            this.MouseMove += CharacterInfo_MouseMove;
        }

        private void SetupModernUI()
        {
            // Reposition existing Designer labels
            characterNameLabel.Location = new Point(4, 6);
            characterNameLabel.AutoSize = true;
            characterNameLabel.ForeColor = Color.FromArgb(0, 51, 153); // Dark blue from mockup

            characterMapLabel.Location = new Point(4, 22);
            characterMapLabel.AutoSize = true;
            characterMapLabel.Font = new Font("Tahoma", 8.25f, FontStyle.Regular);
            characterMapLabel.ForeColor = Color.DimGray;

            // Hide the old info block to make room for bars
            characterInfoLabel.Visible = false;

            // Create Base/Job Level Label
            _lblLevel = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 8.25f, FontStyle.Regular),
                ForeColor = Color.Black,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.TopRight,
                Text = $"{_baseLevel}/{_jobLevel}"
            };
            this.Controls.Add(_lblLevel);

            // Create Job Name Label
            _lblJob = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 8.25f, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.TopRight,
                Text = _job
            };
            this.Controls.Add(_lblJob);

            UpdateLabelPositions();
        }

        private void UpdateLabelPositions()
        {
            _lblLevel.Location = new Point(this.Width - _lblLevel.PreferredWidth - 8, 6);
            _lblJob.Location = new Point(this.Width - _lblJob.PreferredWidth - 8, 22);
        }

        // External method to update the new display properties
        public void UpdateCharacterDetails(string name, string map, int baseLevel, int jobLevel, string job, string percent)
        {
            characterNameLabel.Text = name;
            characterMapLabel.Text = map;
            _baseLevel = baseLevel;
            _jobLevel = jobLevel;
            _job = job;
            _percentToNext = percent;

            _lblLevel.Text = $"{_baseLevel}/{_jobLevel}";
            _lblJob.Text = _job;
            UpdateLabelPositions();
        }
        public void UpdateCharacterInfo(Client client)
        {
            if (client?.Process == null) return;

            var hpSp = client.ReadHpSp();

            _hpCur = (int)hpSp.CurrentHp;
            _hpMax = (int)hpSp.MaxHp;
            _spCur = (int)hpSp.CurrentSp;
            _spMax = (int)hpSp.MaxSp;

            var (wCur, wMax) = client.ReadWeight();
            _weightCurrent = wCur;
            _weightMax = wMax;

            // We will eventually want to read Base Level, Job Level, and Job Name
            // from the client here as well so the new UI updates dynamically!
        }
        private void RefreshTick(object sender, EventArgs e)
        {
            // Dummy data for testing the visual - uncomment your client logic here
            /*
            Client client = ClientSingleton.GetClient();
            if (client?.Process == null || client.Process.HasExited || !client.IsLoggedIn)
                return;

            var snap = client.ReadHpSp();
            _hpCur = (int)snap.CurrentHp;
            _hpMax = (int)snap.MaxHp;
            _spCur = (int)snap.CurrentSp;
            _spMax = (int)snap.MaxSp;

            var (wCur, wMax) = client.ReadWeight();
            _weightCurrent = wCur;
            _weightMax = wMax;
            */

            // Hardcoded testing data based on your prompt
            _hpCur = 47614; _hpMax = 47614;
            _spCur = 1583; _spMax = 1583;
            _weightCurrent = 4072; _weightMax = 9990;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Enable smooth rendering for rounded edges
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int width = this.ClientSize.Width - 16; // 8px padding on sides
            const int BAR_HEIGHT = 4;
            const int BAR_GAP = 3;

            int y = 40; // Start drawing bars below the text

            // Render bars matching mockup colors
            DrawRoundedBar(e.Graphics, 8, y, width, BAR_HEIGHT, _hpCur, _hpMax, Color.FromArgb(248, 81, 73)); // Red
            y += BAR_HEIGHT + BAR_GAP;

            DrawRoundedBar(e.Graphics, 8, y, width, BAR_HEIGHT, _spCur, _spMax, Color.FromArgb(47, 129, 247)); // Blue
            y += BAR_HEIGHT + BAR_GAP;

            DrawRoundedBar(e.Graphics, 8, y, width, BAR_HEIGHT, (int)_weightCurrent, (int)_weightMax, Color.FromArgb(17, 180, 180)); // Teal

            // Draw a subtle border around the whole control like the screenshot
            using (var borderPen = new Pen(Color.FromArgb(220, 224, 230), 1))
            using (var borderPath = GetRoundedRect(new RectangleF(0, 0, this.Width - 1, this.Height - 1), 6))
            {
                e.Graphics.DrawPath(borderPen, borderPath);
            }
        }

        private void DrawRoundedBar(Graphics g, int x, int y, int width, int height, int cur, int max, Color color)
        {
            if (max <= 0) return;

            float ratio = Math.Max(0f, Math.Min(1f, (float)cur / max));
            int fillWidth = (int)(width * ratio);

            // Background Track
            using (var path = GetRoundedRect(new RectangleF(x, y, width, height), height / 2f))
            using (var back = new SolidBrush(Color.FromArgb(230, 235, 245)))
            {
                g.FillPath(back, path);
            }

            // Foreground Fill
            if (fillWidth > 0)
            {
                // Ensure fill width is at least the height so the rounded corner doesn't warp
                fillWidth = Math.Max(fillWidth, height);
                using (var path = GetRoundedRect(new RectangleF(x, y, fillWidth, height), height / 2f))
                using (var fill = new SolidBrush(color))
                {
                    g.FillPath(fill, path);
                }
            }
        }

        // Utility to generate a rounded rectangle path
        private GraphicsPath GetRoundedRect(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Hover logic for Tooltips
        private void CharacterInfo_MouseMove(object sender, MouseEventArgs e)
        {
            string newTooltip = "";
            int y = 40;
            const int BAR_HEIGHT = 4;
            const int BAR_GAP = 3;

            // Check if mouse is within the horizontal bounds of the bars
            if (e.X >= 8 && e.X <= this.Width - 8)
            {
                if (e.Y >= y - 1 && e.Y <= y + BAR_HEIGHT + 1)
                    newTooltip = $"HP: {_hpCur} / {_hpMax}";

                y += BAR_HEIGHT + BAR_GAP;
                if (e.Y >= y - 1 && e.Y <= y + BAR_HEIGHT + 1)
                    newTooltip = $"SP: {_spCur} / {_spMax}";

                y += BAR_HEIGHT + BAR_GAP;
                if (e.Y >= y - 1 && e.Y <= y + BAR_HEIGHT + 1)
                    newTooltip = $"Weight: {_weightCurrent} / {_weightMax}";
            }

            // Only update if the tooltip changed to prevent flickering
            if (_currentTooltip != newTooltip)
            {
                _currentTooltip = newTooltip;
                if (string.IsNullOrEmpty(newTooltip))
                {
                    _barsToolTip.Hide(this);
                }
                else
                {
                    // Add EXP to the tooltip if they are hovering
                    string fullTip = newTooltip + $"\nNext Level: {_percentToNext}";
                    _barsToolTip.Show(fullTip, this, e.X + 15, e.Y + 15);
                }
            }
        }
    }
}