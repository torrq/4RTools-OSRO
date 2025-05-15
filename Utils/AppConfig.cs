using System.Drawing;
using System.Collections.Generic;

namespace _4RTools.Utils
{
    internal class AppConfig
    {
        public static string Name = "OSRO Tools";
        public static string Version = "v1.0.6";

        // 0 = MR/Midrate, 1 = HR/Highrate
        public static int ServerMode = 0;

        public static string WindowTitle => $"{Name} {Version}/{(ServerMode == 0 ? "MR" : "HR")}";

        public static string SystemTrayText => $"{Name} {Version}/{(ServerMode == 0 ? "MR" : "HR")}";

        // File Paths
        public static string ProfileFolder = "Profile" + "\\";
        public static string ConfigFolder = "Config" + "\\";
        public static string ConfigFile = ConfigFolder + "config.json";
        public static string ServersFile = ConfigFolder + "servers.json";
        public static string CitiesFile = ConfigFolder + "cities.json";
        public static string DebugLogFile = "debug.log";

        // Server Information
        public static List<dynamic> DefaultServers => new List<dynamic>
        {
            ServerMode == 0
                ? new
                {
                    name = "OsRO Midrate",
                    hpAddress = "00E8F434",
                    nameAddress = "00E91C00",
                    mapAddress = "00E8ABD4"
                }
                : new
                {
                    name = "OSRO",
                    hpAddress = "010DCE10",
                    nameAddress = "010DF5D8",
                    mapAddress = "010D856C"
                }
        };

        // Default City List
        public static List<string> DefaultCities => new List<string>
        {
            "prontera", "morocc", "geffen", "payon", "alberta", "izlude", "aldebaran", "xmas",
            "comodo", "yuno", "amatsu", "gonryun", "umbala", "niflheim", "louyang", "jawaii",
            "ayothaya", "einbroch", "lighthalzen", "einbech", "hugel", "rachel", "veins",
            "moscovia", "mid_camp", "munak", "splendide", "brasilis", "dicastes01", "mora",
            "dewata", "malangdo", "malaya", "eclage", "marketplace", "mainhall", "quiz_00"
        };

        // URLs
        public static string GithubLink = "https://github.com/torrq/4RTools-OSRO/releases";
        public static string WebsiteMR = "https://osro.mr";
        public static string WebsiteHR = "https://osro.gg";
        public static string DiscordLinkMR = "https://discord.com/invite/osro2";
        public static string DiscordLinkHR = "https://discord.com/invite/b5mjuCxY";

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

        // Character and Map colors
        public static Color CharacterColor = Color.DarkGreen;
        public static Color MapColor = Color.DarkCyan;

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