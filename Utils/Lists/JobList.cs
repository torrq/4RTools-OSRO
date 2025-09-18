using System.Collections.Generic;

namespace _ORTools.Utils
{
    internal static class JobList
    {
        public static readonly Dictionary<int, string> Jobs = new Dictionary<int, string>
        {
            // Novice / 1st Class
            { 0, "Novice" },
            { 1, "Swordman" },
            { 2, "Mage" },
            { 3, "Archer" },
            { 4, "Acolyte" },
            { 5, "Merchant" },
            { 6, "Thief" },

            // 2nd Class
            { 7, "Knight" },
            { 8, "Priest" },
            { 9, "Wizard" },
            { 10, "Blacksmith" },
            { 11, "Hunter" },
            { 12, "Assassin" },
            { 13, "Knight 2-2" },
            { 14, "Crusader" },
            { 15, "Monk" },
            { 16, "Sage" },
            { 17, "Rogue" },
            { 18, "Alchemist" },
            { 19, "Bard" },
            { 20, "Dancer" },
            { 21, "Crusader 2-2" },

            // Special Modes
            { 22, "Wedding" },
            { 26, "Christmas" },
            { 27, "Summer" },

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

            // 3rd Class (Regular)
            { 4054, "Rune Knight" },
            { 4055, "Warlock" },
            { 4056, "Ranger" },
            { 4057, "Arch Bishop" },
            { 4058, "Mechanic" },
            { 4059, "Gulliotine Cross" },
            { 4066, "Royal Guard" },
            { 4067, "Sorcerer" },
            { 4068, "Minstrel" },
            { 4069, "Wanderer" },
            { 4070, "Sura" },
            { 4071, "Genetic" },
            { 4072, "Shadow Chaser" },

            // 3rd Class (Transcendent)
            { 4060, "Rune Knight" },
            { 4061, "Warlock" },
            { 4062, "Ranger" },
            { 4063, "Arch Bishop" },
            { 4064, "Mechanic" },
            { 4065, "Guillotine Cross" },
            { 4073, "Royal Guard" },
            { 4074, "Sorcerer" },
            { 4075, "Minstrel" },
            { 4076, "Wanderer" },
            { 4077, "Sura" },
            { 4078, "Genetic" },
            { 4079, "Shadow Chaser" },

            // Expanded Class
            { 23, "Super Novice" },
            { 24, "Gunslinger" },
            { 25, "Ninja" },
            { 4045, "Super Baby" },
            { 4046, "Taekwon" },
            { 4047, "Star Gladiator" },
            { 4048, "Star Gladiator (Union)" },
            { 4049, "Soul Linker" },
            { 4050, "Gangsi" },
            { 4051, "Death Knight" },
            { 4052, "Dark Collector" },
            { 4190, "Ex. Super Novice" },
            { 4191, "Ex. Super Baby" },
            { 4211, "Kangerou" },
            { 4212, "Oboro" },
            { 4215, "Rebellion" },
            { 4218, "Summoner" },
            { 4239, "Star Emperor" },
            { 4240, "Soul Reaper" },

            // Baby Novice and Baby 1st Class
            { 4023, "Baby Novice" },
            { 4024, "Baby Swordman" },
            { 4025, "Baby Magician" },
            { 4026, "Baby Archer" },
            { 4027, "Baby Acolyte" },
            { 4028, "Baby Merchant" },
            { 4029, "Baby Thief" },

            // Baby 2nd Class
            { 4030, "Baby Knight" },
            { 4031, "Baby Priest" },
            { 4032, "Baby Wizard" },
            { 4033, "Baby Blacksmith" },
            { 4034, "Baby Hunter" },
            { 4035, "Baby Assassin" },
            { 4037, "Baby Crusader" },
            { 4038, "Baby Monk" },
            { 4039, "Baby Sage" },
            { 4040, "Baby Rogue" },
            { 4041, "Baby Alchemist" },
            { 4042, "Baby Bard" },
            { 4043, "Baby Dancer" },

            // Baby 3rd Class
            { 4096, "Baby Rune Knight" },
            { 4097, "Baby Warlock" },
            { 4098, "Baby Ranger" },
            { 4099, "Baby Arch Bishop" },
            { 4100, "Baby Mechanic" },
            { 4101, "Baby Glt. Cross" },
            { 4102, "Baby Royal Guard" },
            { 4103, "Baby Sorcerer" },
            { 4104, "Baby Minstrel" },
            { 4105, "Baby Wanderer" },
            { 4106, "Baby Sura" },
            { 4107, "Baby Genetic" },
            { 4108, "Baby Shadow Chaser" },

            // Baby Expanded Class
            { 4220, "Baby Summoner" },
            { 4222, "Baby Ninja" },
            { 4223, "Baby Kagero" },
            { 4224, "Baby Oboro" },
            { 4225, "Baby Taekwon" },
            { 4226, "Baby Star Gladiator" },
            { 4227, "Baby Soul Linker" },
            { 4228, "Baby Gunslinger" },
            { 4229, "Baby Rebellion" },
            { 4241, "Baby Star Emperor" },
            { 4242, "Baby Soul Reaper" },

            // HR
            { 4233, "Necromancer" },
            { 4234, "Kage" },
            // Padawan jobs HR
            { 4231, "Jedi" },
            { 4232, "Sith" },

            // Padawan jobs MR
            { 4436, "Padawan" },
            { 4437, "Jedi" },
            { 4438, "Sith" },
            { 4439, "Jedi (Levitate)" },
            { 4440, "Baby Padawan" },
            { 4441, "Baby Jedi" },
            { 4442, "Baby Sith" },
            { 4443, "Baby Jedi (Levitate)" },

        };

        public static string GetNameById(int jobId)
        {
            return Jobs.TryGetValue(jobId, out var name) ? name : $"Job #{jobId}";
        }
    }
}
