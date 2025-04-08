using System;
using System.Collections.Generic;

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
        public static string Version = "v1.0.0";

        // ServerMode: 0 = MR (Mid Rate), 1 = HR (High Rate)
        public static int ServerMode = 0; // Default to MR
        public static bool DebugMode = true;

        // Delay defaults in milliseconds
        public static int AutoPotDefaultDelay = 101; // Autopot Tab
        public static int YggDefaultDelay = 102; // Yggdrasil tab
        public static int SkillSpammerDefaultDelay = 103; // Skill Spammer tab
        public static int AutoBuffSkillsDefaultDelay = 104; // Autobuff Skills tab
        public static int AutoBuffItemsDefaultDelay = 105; // Autobuff Items tab
        public static int ATKDEFSpammerDefaultDelay = 11; // ATK x DEF tab, spammer
        public static int ATKDEFSwitchDefaultDelay = 51; // ATK x DEF tab, switch
        public static int MacroDefaultDelay = 52; // default for Macro Song + Switch tabs
    }
}
