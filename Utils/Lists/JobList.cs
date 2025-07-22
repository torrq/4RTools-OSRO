using System.Collections.Generic;

namespace _ORTools.Utils
{
    internal static class JobList
    {
        public static readonly Dictionary<int, string> Jobs = new Dictionary<int, string>
        {
            { 0, "Novice" },
            { 1, "Swordman" },
            { 2, "Mage" },
            { 3, "Archer" },
            { 4, "Acolyte" },
            { 5, "Merchant" },
            { 6, "Thief" },
            { 7, "Knight" },
            { 8, "Priest" },
            { 9, "Wizard" },
            { 10, "Blacksmith" },
            { 11, "Hunter" },
            { 12, "Assassin" },
            { 13, "Knight" }, // Knight (2-2)
            { 14, "Crusader" },
            { 15, "Monk" },
            { 16, "Sage" },
            { 17, "Rogue" },
            { 18, "Alchemist" },
            { 19, "Bard" },
            { 20, "Dancer" },
            { 21, "Crusader" }, // Crusader (2-2)
            { 22, "Wedding" },
            { 23, "Super Novice" },
            { 24, "Gunslinger" },
            { 25, "Ninja" },

            // High/Transcendent First Jobs
            { 4001, "High Novice" },
            { 4002, "High Swordman" },
            { 4003, "High Mage" },
            { 4004, "High Archer" },
            { 4005, "High Acolyte" },
            { 4006, "High Merchant" },
            { 4007, "High Thief" },

            // Transcendent 2nd Jobs
            { 4008, "Lord Knight" },
            { 4009, "High Priest" },
            { 4010, "High Wizard" },
            { 4011, "Whitesmith" },
            { 4012, "Sniper" },
            { 4013, "Assassin Cross" },
            { 4014, "Lord Knight" }, // Lord Knight (2-2)
            { 4015, "Paladin" },
            { 4016, "Champion" },
            { 4017, "Professor" },
            { 4018, "Stalker" },
            { 4019, "Creator" },
            { 4020, "Clown" },
            { 4021, "Gypsy" },

            // Baby Classes
            { 4022, "Baby" },
            { 4023, "Baby Swordman" },
            { 4024, "Baby Mage" },
            { 4025, "Baby Archer" },
            { 4026, "Baby Acolyte" },
            { 4027, "Baby Merchant" },
            { 4028, "Baby Thief" },
            { 4029, "Baby Knight" },
            { 4030, "Baby Priest" },
            { 4031, "Baby Wizard" },
            { 4032, "Baby Blacksmith" },
            { 4033, "Baby Hunter" },
            { 4034, "Baby Assassin" },
            { 4035, "Baby Crusader" },
            { 4036, "Baby Monk" },
            { 4037, "Baby Sage" },
            { 4038, "Baby Rogue" },
            { 4039, "Baby Alchemist" },
            { 4040, "Baby Bard" },
            { 4041, "Baby Dancer" },
            { 4042, "Super Baby" },
            { 4043, "Baby Crusader" }, // Baby Crusader (2-2)
            { 4044, "Baby Clown" },
            { 4045, "Baby Gypsy" },

            // Extended Jobs
            { 4046, "Taekwon" },
            { 4047, "Star Gladiator" },
            { 4048, "Star Gladiator" }, // "alt" SG
            { 4049, "Soul Linker" },

            // HR 3rd Jobs
            { 4060, "Rune Knight" },
            { 4061, "Warlock" },
            { 4062, "Ranger" },
            { 4063, "Arch Bishop" },
            { 4064, "Mechanic" },
            { 4065, "Guillotine Cross" },
            { 4073, "Royal Guard" },
            { 4074, "Sorcerer" },
            { 4075, "Minstrel" },
            { 4077, "Sura" },
            { 4078, "Genetic" },
            { 4079, "Shadow Chaser" },
            { 4233, "Necromancer" },
            { 4234, "Kage" },

            // Padawan jobs HR
            { 4231, "Jedi" },
            { 4232, "Sith" },

            // Padawan jobs MR
            { 4437, "Jedi" },
            { 4438, "Sith" },
        };

        public static string GetNameById(int jobId)
        {
            return Jobs.TryGetValue(jobId, out var name) ? name : $"Job #{jobId}";
        }
    }
}
