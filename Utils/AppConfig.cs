using System.Drawing;

namespace _4RTools.Utils
{
    internal class AppConfig
    {
        public static string Name = "OSRO Tools";
        public static string Version = "v1.0.5";

        // 0 = MR/Midrate, 1 = HR/Highrate
        public static int ServerMode = 0;

        public static string WindowTitle => $"{Name} {Version}/{(ServerMode == 0 ? "MR" : "HR")}";

        // Paths
        public static string DebugLogFilePath = "debug.log";
        public static string ProfileFolder = "Profile\\";

        // URLs
        public static string GithubLink = "https://github.com/torrq/4RTools-OSRO";
        public static string Website => ServerMode == 0 ? "https://osro.mr" : "https://osro.gg";
        public static string DiscordLink => ServerMode == 0 ? "https://discord.com/invite/osro2" : "https://discord.com/invite/b5mjuCxY";

        // Default minimum delays
        public static int AutoPotDefaultDelay = 50;
        public static int YggDefaultDelay = 50;
        public static int SkillSpammerDefaultDelay = 50;
        public static int AutoBuffSkillsDefaultDelay = 50;
        public static int AutoBuffItemsDefaultDelay = 50;
        public static int ATKDEFSpammerDefaultDelay = 100;
        public static int ATKDEFSwitchDefaultDelay = 100;
        public static int MacroDefaultDelay = 100;
        public static int SkillTimerDefaultDelay = 1000;
        public static decimal DefaultMinimumDelay = 10;

        // Button background colors
        public static Color DefaultButtonBackColor = Color.White;
        public static Color ResetButtonBackColor = Color.FromArgb(245, 210, 230);
        public static Color RemoveButtonBackColor = Color.Pink;
        public static Color CreateButtonBackColor = Color.FromArgb(190, 249, 155);
        public static Color RenameButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color CopyButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color AccentBackColor = Color.FromArgb(238, 248, 255);

        // Dynamically darkens border colors according to background colors
        public static int ProfileButtonBorderDarkenAmount = 60;

        // Debug Console Settings
        public static Color DebugConsoleBackColor = Color.Black;
        public static Color DebugConsoleForeColor = Color.White;
        public static Font DebugConsoleFont = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        // Debug Console syntax highlighting colors
        public static Color LogColor_INFO = Color.LightSkyBlue;
        public static Color LogColor_WARNING = Color.Orange;
        public static Color LogColor_ERROR = Color.Red;
        public static Color LogColor_DEBUG = Color.MediumPurple;
        public static Color LogColor_STATUS = Color.ForestGreen;
        public static Color LogColor_StatusId = Color.LimeGreen;
        public static Color LogColor_StatusName = Color.PaleGreen;
        public static Color LogColor_StatusUnknown = Color.Yellow;
        public static Color LogColor_Timestamp = Color.FromArgb(90, 90, 90);

        // Debug Logger message types
        public const string INFO = "I";
        public const string WARNING = "W";
        public const string ERROR = "E";
        public const string DEBUG = "D";
        public const string STATUS = "S";

        // Rarely necessary to set this true anymore, global settings should enable it
        public static bool DebugMode = false;
    }
}