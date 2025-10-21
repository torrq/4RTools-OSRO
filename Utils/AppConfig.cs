using System;
using System.Collections.Generic;
using System.Drawing;

namespace BruteGamingMacros.Core.Utils
{
    internal class AppConfig
    {
        // === BRUTE GAMING MACROS v2.0.0 ===
        public static string Name = "Brute Gaming Macros";
        public static string Version = "v2.0.0";
        public static string Tagline = "The Ultimate Gaming Automation Suite";

#if MR_BUILD
        public static int ServerMode = 0; // Mid-rate
#elif HR_BUILD
        public static int ServerMode = 1; // High-rate
#elif LR_BUILD
        public static int ServerMode = 2; // Low-rate
#else
        public static int ServerMode = 0;
#endif

        public static string WindowTitle => $"{Name} {Version}/{GetRateTag()}";
        public static string SystemTrayText => $"{Name} {Version}/{GetRateTag()}";

        // === SUPERIOR PERFORMANCE SETTINGS ===
        // Ultra-fast spam modes (Phase 2 - SuperiorInputEngine)
        public static int UltraSpamDelayMs = 1;        // 1000 actions/second
        public static int TurboSpamDelayMs = 5;        // 200 actions/second
        public static int StandardSpamDelayMs = 10;    // 100 actions/second

        // Engine optimization flags
        public static bool UseHardwareSimulation = true;     // SendInput API
        public static bool UseBatchMemoryReading = true;     // Batch memory reads
        public static int MemoryCacheDurationMs = 100;       // Cache duration

        public static string ProfileFolder = "Profile\\";
        public static string ConfigFolder = "Config\\";
        public static string ConfigFile = ConfigFolder + "config.json";
        public static string ServersFile = ConfigFolder + "servers.json";
        public static string CitiesFile = ConfigFolder + "cities.json";
        public static string DebugLogFile = "debug.log";

        // === WINDOW CLASS NAMES (for process detection) ===
        public static string WindowClassMR = "Oldschool RO - Midrate | www.osro.mr";
        public static string WindowClassHR = "Oldschool RO | www.osro.gg";
        public static string WindowClassLR = "Oldschool RO | Revo";  // Updated from "dunno"

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
                                onlineAddress = "00E8A928"
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
                                onlineAddress = "010D83C7"
                            }
                        };

                    case 2: // Low-rate
                        return new List<dynamic>
                        {
                            new
                            {
                                name          = "OsRO Revo",
                                description   = "OsRO Revo (Lowrate)",
                                hpAddress     = "00000000",
                                nameAddress   = "00000000",
                                mapAddress    = "00000000",
                                onlineAddress = "00000000"
                            }
                        };

                    default:
                        throw new InvalidOperationException($"Unsupported ServerMode value: {ServerMode}");
                }
            }
        }

        public static List<string> DefaultCities => new List<string>
        {
            "prontera", "morocc", "geffen", "payon", "alberta", "izlude", "aldebaran", "xmas",
            "comodo", "yuno", "amatsu", "gonryun", "umbala", "niflheim", "louyang", "jawaii",
            "ayothaya", "einbroch", "lighthalzen", "einbech", "hugel", "rachel", "veins",
            "moscovia", "mid_camp", "munak", "splendide", "brasilis", "dicastes01", "mora",
            "dewata", "malangdo", "malaya", "eclage", "marketplace", "mainhall", "quiz_00"
        };

        // === LINKS & RESOURCES ===
        public static string GithubLink = "https://github.com/epicseo/4RTools-OSRO/releases";  // Repository releases
        public static string LatestVersionURL = "https://api.github.com/repos/epicseo/4RTools-OSRO/releases/latest";  // API endpoint for latest version
        public static string WebsiteMR = "https://osro.mr";
        public static string WebsiteHR = "https://osro.gg";
        public static string WebsiteLR = "https://osro-revo.gg";
        public static string DiscordLinkMR = "https://discord.com/invite/osro2";
        public static string DiscordLinkHR = "https://discord.com/invite/osro";
        public static string DiscordLinkLR = "https://discord.com/invite/osro3";

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

        public static Color DefaultButtonBackColor = Color.White;
        public static Color ResetButtonBackColor = Color.FromArgb(245, 210, 230);
        public static Color RemoveButtonBackColor = Color.Pink;
        public static Color CreateButtonBackColor = Color.FromArgb(190, 249, 155);
        public static Color RenameButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color CopyButtonBackColor = Color.FromArgb(249, 255, 175);
        public static Color AccentBackColor = Color.FromArgb(238, 248, 255);

        public static Color CharacterColor = Color.DarkGreen;
        public static Color MapColor = Color.DarkCyan;

        public static int ProfileButtonBorderDarkenAmount = 60;

        public static Color DebugConsoleBackColor = Color.Black;
        public static Color DebugConsoleForeColor = Color.White;
        public static Font DebugConsoleFont = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, (byte)(0));

        public static Color LogColor_INFO = Color.LightSkyBlue;
        public static Color LogColor_WARNING = Color.Orange;
        public static Color LogColor_ERROR = Color.Red;
        public static Color LogColor_DEBUG = Color.MediumPurple;
        public static Color LogColor_STATUS = Color.ForestGreen;
        public static Color LogColor_StatusId = Color.LimeGreen;
        public static Color LogColor_StatusName = Color.PaleGreen;
        public static Color LogColor_StatusUnknown = Color.Yellow;
        public static Color LogColor_Timestamp = Color.FromArgb(90, 90, 90);

        public const string INFO = "I";
        public const string WARNING = "W";
        public const string ERROR = "E";
        public const string DEBUG = "D";
        public const string STATUS = "S";

        public static bool DebugMode = false;

        private static string GetRateTag()
        {
            switch (ServerMode)
            {
                case 0: return "MR";   // Mid‑rate
                case 1: return "HR";   // High‑rate
                case 2: return "LR";   // Low‑rate
                default:
                    throw new InvalidOperationException($"Unsupported ServerMode value: {ServerMode}");
            }
        }
    }
}