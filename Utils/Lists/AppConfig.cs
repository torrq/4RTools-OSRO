using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _ORTools.Utils
{
    internal static class AppConfig
    {
        #region General

        public static string Name = "OSRO Tools";
        public static string Version = "v1.0.10";
        public static decimal ConfigVersion = 1;

        // 0 = Mid-rate, 1 = High-rate
        public static int ServerMode = 0;

        // Set to true for pre-release versions, false for stable releases
        public static bool preRelease = true;

        public static string WindowTitle => $"{Name} {Version}/{GetRateTag()}{(preRelease ? $" (BETA 20251213)" : "")}";
        public static string SystemTrayText => $"{Name} {Version}/{GetRateTag()}{(preRelease ? $" (BETA 20251213)" : "")}";

        #endregion

        #region File Paths

        public static string ProfileFolder = "Profiles\\";
        public static string ConfigFolder = "Config\\";
        public static string ConfigFile = ConfigFolder + "config.json";
        public static string ServersFile = ConfigFolder + "servers.json";
        public static string CitiesFile = ConfigFolder + "cities.json";
        public static string DebugLogFile = "debug.log";

        #endregion

        #region Memory Addresses / Server Types

        public static List<dynamic> DefaultServers
        {
            get
            {
                switch (ServerMode)
                {
                    case 0: // Mid‑rate
                        return new List<dynamic>
                        {
                            new
                            {
                                name          = "OsRO Midrate",
                                description   = "OsRO Midrate",
                                hpAddress     = "00E8F434",
                                nameAddress   = "00E91C00",
                                mapAddress    = "00E8ABD4",
                                jobAddress    = "00E8BA54",
                                onlineAddress = "00E884B1"
                            }
                        };

                    case 1: // High‑rate
                        return new List<dynamic>
                        {
                            new
                            {
                                name          = "OSRO",
                                description   = "OsRO Highrate",
                                hpAddress     = "010DCE10",
                                nameAddress   = "010DF5D8",
                                mapAddress    = "010D856C",
                                jobAddress    = "010D93D8",
                                onlineAddress = "010A2FB0"
                            }
                        };

                    default:
                        throw new InvalidOperationException($"Unsupported ServerMode value: {ServerMode}");
                }
            }
        }

        private static string GetRateTag()
        {
            switch (ServerMode)
            {
                case 0: return "MR";   // Mid‑rate
                case 1: return "HR";   // High‑rate
                default:
                    throw new InvalidOperationException($"Unsupported ServerMode value: {ServerMode}");
            }
        }

        #endregion

        #region Cities

        public static List<string> DefaultCities => new List<string>
        {
            "prontera", "morocc", "geffen", "payon", "alberta", "izlude", "aldebaran", "xmas",
            "comodo", "yuno", "amatsu", "gonryun", "umbala", "niflheim", "louyang", "jawaii",
            "ayothaya", "einbroch", "lighthalzen", "einbech", "hugel", "rachel", "veins",
            "moscovia", "mid_camp", "munak", "splendide", "brasilis", "dicastes01", "mora",
            "dewata", "malangdo", "malaya", "eclage", "marketplace", "mainhall", "quiz_00"
        };

        #endregion

        #region URLs

        public static string GithubLink = "https://github.com/torrq/4RTools-OSRO/releases";
        public static string WebsiteMR = "https://osro.mr";
        public static string WebsiteHR = "https://osro.gg";
        public static string DiscordLinkMR = "https://discord.com/invite/osro2";
        public static string DiscordLinkHR = "https://discord.com/invite/osro";
        public static string WikiLinkMR = "https://wiki.osro.mr";
        public static string WikiLinkHR = "https://wiki.osro.gg";

        #endregion

        #region Default Delays

        public static int AutoPotDefaultDelay = 50;
        public static int YggDefaultDelay = 50;
        public static int SkillSpammerDefaultDelay = 50;
        public static int AutoBuffSkillsDefaultDelay = 50;
        public static int AutoBuffItemsDefaultDelay = 50;
        public static int ATKDEFSpammerDefaultDelay = 100;
        public static int ATKDEFSwitchDefaultDelay = 100;
        public static int MacroDefaultDelay = 100;
        public static int SkillTimerDefaultDelay = 1000;
        public static decimal DefaultMinimumDelay = 0;

        #endregion

        #region Colors

        public static Color DefaultButtonBackColor = Color.White;
        public static Color ResetButtonBackColor = Color.FromArgb(245, 210, 230);
        public static Color RemoveButtonBackColor = Color.Pink;
        public static Color CreateButtonBackColor = Color.FromArgb(190, 249, 155);
        public static Color RenameButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color CopyButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color AccentBackColor = Color.FromArgb(238, 248, 255);
        public static Color CheckBoxCheckedBorderColor = Color.FromArgb(0, 120, 215); // Blue for "Checked"
        public static Color CheckBoxIndeterminateBorderColor = Color.FromArgb(255, 128, 0); // Orange for "Indeterminate"
        public static Color CheckBoxUncheckedBorderColor = Color.FromArgb(171, 171, 171); // A standard gray for "Unchecked"
        public static Color CheckBoxDisabledBorderColor = Color.FromArgb(204, 204, 204); // Lighter gray for "Disabled"
        public static Color CharacterColor = Color.DarkGreen;
        public static Color MapColor = Color.DarkCyan;
        public static Color ActiveKeyColor = Color.FromArgb(0, 0, 0);
        public static Color InactiveKeyColor = Color.FromArgb(150, 150, 150);
        public static int ProfileButtonBorderDarkenAmount = 60;

        #endregion

        #region String Constants

        public const string TEXT_NONE = "None";

        #endregion

        #region DebugLog

        public static Color LogColor_INFO = Color.LightSkyBlue;
        public static Color LogColor_WARNING = Color.Orange;
        public static Color LogColor_ERROR = Color.Red;
        public static Color LogColor_DEBUG = Color.MediumPurple;
        public static Color LogColor_STATUS = Color.ForestGreen;
        public static Color LogColor_StatusId = Color.LimeGreen;
        public static Color LogColor_StatusName = Color.PaleGreen;
        public static Color LogColor_StatusUnknown = Color.Yellow;
        public static Color LogColor_Timestamp = Color.FromArgb(90, 90, 90);
        public static Color DebugConsoleBackColor = Color.Black;
        public static Color DebugConsoleForeColor = Color.White;
        public static Font DebugConsoleFont = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, (byte)(0));
        public const string INFO = "I";
        public const string WARNING = "W";
        public const string ERROR = "E";
        public const string DEBUG = "D";
        public const string STATUS = "S";
        public static bool DebugMode = false;

        #endregion
    }
}