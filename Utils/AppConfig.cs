using System.Drawing;

namespace _4RTools.Utils
{
    internal class AppConfig
    {
        public static string Name = "4RTools-OSRO";
        public static string ProfileFolder = "Profile\\";
        public static string Website = "";
        public static string GithubLink = "";
        public static string DiscordLink = "";
        public static string _4RLatestVersionURL = "";
        public static string Version = "v1.0.1";

        // ServerMode: 0 = MR (Mid Rate), 1 = HR (High Rate)
        public static int ServerMode = 0; // Default to MR

        // DebugMode=true will output to DebugLogFilePath with extra information
        public static bool DebugMode = true;
        public static string DebugLogFilePath = "4rtools_debug.log";

        // Delay defaults in milliseconds
        public static int AutoPotDefaultDelay = 100; // Autopot Tab
        public static int YggDefaultDelay = 100; // Yggdrasil tab
        public static int SkillSpammerDefaultDelay = 100; // Skill Spammer tab
        public static int AutoBuffSkillsDefaultDelay = 100; // Autobuff Skills tab
        public static int AutoBuffItemsDefaultDelay = 100; // Autobuff Items tab
        public static int ATKDEFSpammerDefaultDelay = 100; // ATK x DEF tab, spammer
        public static int ATKDEFSwitchDefaultDelay = 100; // ATK x DEF tab, switch
        public static int MacroDefaultDelay = 100; // default for Macro Song + Switch tabs
        public static int SkillTimerDefaultDelay = 1000; // Skill Timer tab
        public static decimal DefaultMinimumDelay = 15; // Default minimum delay for all tabs

        // Default colors
        public static Color DefaultButtonBackColor = Color.White;
        public static Color ResetButtonBackColor = Color.FromArgb(245, 210, 230);
        public static Color RemoveButtonBackColor = Color.Pink;
        public static Color CreateButtonBackColor = Color.FromArgb(155, 221, 249);
        public static Color AccentBackColor = Color.FromArgb(238, 248, 255);

        public static string WindowTitle => $"{Name} - {Version} ({(ServerMode == 0 ? "MR" : "HR")})";
        public static string NotifyIconTitle => $"{Name} ({(ServerMode == 0 ? "MR" : "HR")})";
    }
}
