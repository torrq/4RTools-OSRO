using System;
using System.Collections.Generic;
using System.IO;
using _4RTools.Forms;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class ProfileSingleton
    {
        public static Profile profile = new Profile("Default");

        public static void Load(string profileName)
        {
            DebugLogger.Info($"ProfileSingleton.Load: Attempting to load profile: {profileName}");
            try
            {
                string jsonFilePath = AppConfig.ProfileFolder + profileName + ".json";
                if (!File.Exists(jsonFilePath))
                {
                    DebugLogger.Info($"ProfileSingleton.Load: Profile file not found: {jsonFilePath}. This might be a new profile.");
                    // Depending on logic, you might want to create a default here
                    // ProfileSingleton.Create(profileName); // Or handle this upstream
                    return; // Exit if file doesn't exist, creation might be handled elsewhere
                }

                string json = File.ReadAllText(jsonFilePath);
                DebugLogger.Info($"ProfileSingleton.Load: Successfully read JSON file for profile: {profileName}");

                dynamic rawObject = JsonConvert.DeserializeObject(json);

                if ((rawObject != null))
                {
                    profile.Name = profileName;
                    // DebugLogger.Info($"ProfileSingleton.Load: Profile name set to: {profile.Name}"); // Keeping this optional, maybe too much

                    // Deserialize other configurations (logging removed here as requested)
                    profile.UserPreferences = JsonConvert.DeserializeObject<ConfigProfile>(Profile.GetByAction(rawObject, profile.UserPreferences));
                    profile.AHK = JsonConvert.DeserializeObject<AHK>(Profile.GetByAction(rawObject, profile.AHK));
                    profile.Autopot = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.Autopot));
                    profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.AutopotYgg));
                    profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(Profile.GetByAction(rawObject, profile.StatusRecovery));
                    profile.AutoRefreshSpammer = JsonConvert.DeserializeObject<AutoRefreshSpammer>(Profile.GetByAction(rawObject, profile.AutoRefreshSpammer));

                    // Focused Debugging for AutobuffSkill
                    DebugLogger.Info($"ProfileSingleton.Load: Deserializing AutobuffSkill for profile: {profileName}");
                    profile.AutobuffSkill = JsonConvert.DeserializeObject<AutoBuffSkill>(Profile.GetByAction(rawObject, profile.AutobuffSkill));
                    DebugLogger.Info($"ProfileSingleton.Load: AutobuffSkill loaded. Initial Delay: {profile.AutobuffSkill.Delay}");
                    if (profile.AutobuffSkill.Delay < 0)
                    {
                        DebugLogger.Info($"ProfileSingleton.Load: AutobuffSkill Delay was invalid ({profile.AutobuffSkill.Delay}). Setting to default: {AppConfig.AutoBuffSkillsDefaultDelay}");
                        profile.AutobuffSkill.Delay = AppConfig.AutoBuffSkillsDefaultDelay;
                    }
                    DebugLogger.Info($"ProfileSingleton.Load: Final AutobuffSkill Delay: {profile.AutobuffSkill.Delay}");


                    // Focused Debugging for AutobuffItem
                    DebugLogger.Info($"ProfileSingleton.Load: Deserializing AutobuffItem for profile: {profileName}");
                    profile.AutobuffItem = JsonConvert.DeserializeObject<AutoBuffItem>(Profile.GetByAction(rawObject, profile.AutobuffItem));
                    DebugLogger.Info($"ProfileSingleton.Load: AutobuffItem loaded. Initial Delay: {profile.AutobuffItem.Delay}");
                    if (profile.AutobuffItem.Delay < 0)
                    {
                        DebugLogger.Info($"ProfileSingleton.Load: AutobuffItem Delay was invalid ({profile.AutobuffItem.Delay}). Setting to default: {AppConfig.AutoBuffItemsDefaultDelay}");
                        profile.AutobuffItem.Delay = AppConfig.AutoBuffItemsDefaultDelay;
                    }
                    DebugLogger.Info($"ProfileSingleton.Load: Final AutobuffItem Delay: {profile.AutobuffItem.Delay}");

                    // Deserialize other configurations (logging removed here as requested)
                    profile.SongMacro = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.SongMacro));
                    profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDEFMode>(Profile.GetByAction(rawObject, profile.AtkDefMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.MacroSwitch));
                    profile.Custom = JsonConvert.DeserializeObject<Custom>(Profile.GetByAction(rawObject, profile.Custom));
                    profile.DebuffsRecovery = JsonConvert.DeserializeObject<DebuffsRecovery>(Profile.GetByAction(rawObject, profile.DebuffsRecovery));
                    profile.WeightDebuffsRecovery = JsonConvert.DeserializeObject<DebuffsRecovery>(Profile.GetByAction(rawObject, profile.WeightDebuffsRecovery));

                    DebugLogger.Info($"ProfileSingleton.Load: Successfully loaded profile: {profileName}");
                }
                else
                {
                    DebugLogger.Info($"ProfileSingleton.Load: rawObject was null when deserializing profile: {profileName}");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"ProfileSingleton.Load: Error loading profile '{profileName}': {ex.Message}");
                throw new Exception("There was a problem loading the profile. Delete the Profiles folder and try again.");
            }
        }

        public static void ClearProfile(string profileName)
        {
            // Reduced/Removed logging
            if (profileName != profile.Name)
            {
                profile = new Profile(profileName);
            }
        }

        public static void Create(string profileName)
        {
            // Reduced/Removed logging
            string jsonFileName = AppConfig.ProfileFolder + profileName + ".json";

            if (!File.Exists(jsonFileName))
            {
                ClearProfile(profileName);
                if (!Directory.Exists(AppConfig.ProfileFolder)) { Directory.CreateDirectory(AppConfig.ProfileFolder); }
                FileStream fs = File.Create(jsonFileName);
                fs.Close();

                Profile profile = new Profile(profileName);
                string output = JsonConvert.SerializeObject(profile, Formatting.Indented);
                File.WriteAllText(jsonFileName, output);
            }

            ProfileSingleton.Load(profileName);
        }

        public static void Delete(string profileName)
        {
            // Reduced/Removed logging
            try
            {
                if (profileName != "Default") { File.Delete(AppConfig.ProfileFolder + profileName + ".json"); }
            }
            catch { } // Consider logging the exception even here for unexpected issues
        }

        public static void SetConfiguration(IAction action)
        {
            // Reduced/Removed logging
            if (profile != null)
            {
                try
                {
                    string jsonFilePath = AppConfig.ProfileFolder + profile.Name + ".json";
                    string jsonData = File.ReadAllText(jsonFilePath);
                    dynamic jsonObj = JsonConvert.DeserializeObject(jsonData);
                    jsonObj[action.GetActionName()] = action.GetConfiguration();
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(jsonFilePath, output);
                }
                catch { } // Consider logging the exception even here
            }
        }

        public static Profile GetCurrent()
        {
            return profile;
        }
    }

    // Assuming Profile, IAction, ConfigProfile, AHK, Autopot, AutoRefreshSpammer,
    // AutoBuffSkill, AutoBuffItem, StatusRecovery, DebuffsRecovery, Macro, Custom,
    // ATKDEFMode, MacroSongForm, MacroSwitchForm, ATKDEFForm, AppConfig, DebugLogger,
    // and Utils namespace are defined elsewhere in your project.
    // IAction interface needs GetActionName() and GetConfiguration() methods.
    // The Profile class structure and the GetByAction method are included for completeness.
    // Constants like ACTION_NAME_AUTOPOT etc. are assumed to exist on their respective classes.
    // TOTAL_MACRO_LANES_FOR_SONGS, TOTAL_MACRO_LANES, TOTAL_ATKDEF_LANES assumed to exist on forms.

    public class Profile // Assuming this class is within the same file or namespace
    {
        public string Name { get; set; }
        public ConfigProfile UserPreferences { get; set; } // Assuming ConfigProfile exists
        public AHK AHK { get; set; } // Assuming AHK exists
        public Autopot Autopot { get; set; } // Assuming Autopot exists
        public Autopot AutopotYgg { get; set; } // Assuming AutopotYgg is also of type Autopot
        public AutoRefreshSpammer AutoRefreshSpammer { get; set; } // Assuming AutoRefreshSpammer exists
        public AutoBuffSkill AutobuffSkill { get; set; } // Assuming AutoBuffSkill exists
        public AutoBuffItem AutobuffItem { get; set; } // Assuming AutoBuffItem exists
        public StatusRecovery StatusRecovery { get; set; } // Assuming StatusRecovery exists
        public DebuffsRecovery DebuffsRecovery { get; set; } // Assuming DebuffsRecovery exists
        public DebuffsRecovery WeightDebuffsRecovery { get; set; } // Assuming WeightDebuffsRecovery is also of type DebuffsRecovery
        public Macro SongMacro { get; set; } // Assuming Macro exists
        public Macro MacroSwitch { get; set; } // Assuming MacroSwitch is also of type Macro

        public Custom Custom { get; set; } // Assuming Custom exists
        public ATKDEFMode AtkDefMode { get; set; } // Assuming ATKDEFMode exists

        public Profile(string name)
        {
            this.Name = name;

            this.UserPreferences = new ConfigProfile();
            this.AHK = new AHK();
            this.Autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT); // Assuming ACTION_NAME_AUTOPOT constant exists
            this.AutopotYgg = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG); // Assuming ACTION_NAME_AUTOPOT_YGG constant exists
            this.AutoRefreshSpammer = new AutoRefreshSpammer();
            this.AutobuffSkill = new AutoBuffSkill(AutoBuffSkill.ACTION_NAME_AUTOBUFFSKILL); // Assuming ACTION_NAME_AUTOBUFFSKILL constant exists
            this.AutobuffItem = new AutoBuffItem(AutoBuffItem.ACTION_NAME_AUTOBUFFITEM); // Assuming ACTION_NAME_AUTOBUFFITEM constant exists
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new Macro(Macro.ACTION_NAME_SONG_MACRO, MacroSongForm.TOTAL_MACRO_LANES_FOR_SONGS); // Assuming constants exist
            this.MacroSwitch = new Macro(Macro.ACTION_NAME_MACRO_SWITCH, MacroSwitchForm.TOTAL_MACRO_LANES); // Assuming constants exist
            this.AtkDefMode = new ATKDEFMode(ATKDEFForm.TOTAL_ATKDEF_LANES); // Assuming constant exists
            this.DebuffsRecovery = new DebuffsRecovery("DebuffsRecovery");
            this.WeightDebuffsRecovery = new DebuffsRecovery("WeightDebuffsRecovery");
            this.Custom = new Custom();
        }

        public static object GetByAction(dynamic obj, IAction action) // Assuming IAction interface exists
        {
            if (obj != null && obj[action.GetActionName()] != null)
            {
                return obj[action.GetActionName()].ToString();
            }

            return action.GetConfiguration(); // Assuming GetConfiguration returns a default or current config
        }

        public static List<string> ListAll()
        {
            List<string> profiles = new List<string>();
            // Reduced/Removed logging
            try
            {
                if (Directory.Exists(AppConfig.ProfileFolder))
                {
                    string[] files = Directory.GetFiles(AppConfig.ProfileFolder, "*.json");

                    foreach (string fileName in files)
                    {
                        string profileName = Path.GetFileNameWithoutExtension(fileName);
                        profiles.Add(profileName);
                    }
                }
            }
            catch { } // Consider logging the exception even here
            return profiles;
        }
    }
}