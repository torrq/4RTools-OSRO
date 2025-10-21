using BruteGamingMacros.UI.Forms;
using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class ProfileSingleton
    {
        // THREAD SAFETY FIX: Private field with lock-protected access
        private static Profile profile = new Profile("Default");
        private static readonly object profileLock = new object();

        // Temporary class to deserialize old "Custom" data
        private class LegacyCustom
        {
            public string ActionName { get; set; }
            public Key tiMode { get; set; }
        }

        /// <summary>
        /// THREAD-SAFE: Load profile from disk with lock protection
        /// </summary>
        public static void Load(string profileName)
        {
            try
            {
                string json = File.ReadAllText(AppConfig.ProfileFolder + profileName + ".json");
                dynamic rawObject = JsonConvert.DeserializeObject(json);

                // Migrate old "Custom" key to "TransferHelper"
                if (rawObject != null && rawObject["Custom"] != null && rawObject["TransferHelper"] == null)
                {
                    try
                    {
                        // Deserialize the old "Custom" data
                        string customJson = rawObject["Custom"].ToString();
                        LegacyCustom legacyCustom = JsonConvert.DeserializeObject<LegacyCustom>(customJson);

                        // Create new TransferHelper data
                        TransferHelper newTransferHelper = new TransferHelper
                        {
                            ActionName = TransferHelper.ACTION_NAME_TRANSFER,
                            TransferKey = legacyCustom.tiMode
                        };

                        // Update the JSON object
                        rawObject["TransferHelper"] = JsonConvert.SerializeObject(newTransferHelper);
                        rawObject.Property("Custom").Remove();

                        // Save the updated JSON back to the file
                        File.WriteAllText(AppConfig.ProfileFolder + profileName + ".json", JsonConvert.SerializeObject(rawObject, Formatting.Indented));
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue loading with default TransferHelper
                        Console.WriteLine($"Failed to migrate Custom to TransferHelper: {ex.Message}");
                        rawObject["TransferHelper"] = JsonConvert.SerializeObject(new TransferHelper());
                        rawObject.Property("Custom").Remove();
                        File.WriteAllText(AppConfig.ProfileFolder + profileName + ".json", JsonConvert.SerializeObject(rawObject, Formatting.Indented));
                    }
                }

                if (rawObject != null)
                {
                    lock (profileLock)
                    {
                        profile.Name = profileName;
                        profile.UserPreferences = JsonConvert.DeserializeObject<ConfigProfile>(Profile.GetByAction(rawObject, profile.UserPreferences));
                        profile.SkillSpammer = JsonConvert.DeserializeObject<SkillSpammer>(Profile.GetByAction(rawObject, profile.SkillSpammer));
                        profile.Autopot = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.Autopot));
                        profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.AutopotYgg));
                        profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(Profile.GetByAction(rawObject, profile.StatusRecovery));
                        profile.SkillTimer = JsonConvert.DeserializeObject<SkillTimer>(Profile.GetByAction(rawObject, profile.SkillTimer));
                        profile.AutobuffSkill = JsonConvert.DeserializeObject<AutoBuffSkill>(Profile.GetByAction(rawObject, profile.AutobuffSkill));
                        if (profile.AutobuffSkill.Delay < 0)
                        {
                            profile.AutobuffSkill.Delay = AppConfig.AutoBuffSkillsDefaultDelay;
                        }
                        profile.AutobuffItem = JsonConvert.DeserializeObject<AutoBuffItem>(Profile.GetByAction(rawObject, profile.AutobuffItem));
                        if (profile.AutobuffItem.Delay < 0)
                        {
                            profile.AutobuffItem.Delay = AppConfig.AutoBuffItemsDefaultDelay;
                        }
                        profile.SongMacro = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.SongMacro));
                        profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDEF>(Profile.GetByAction(rawObject, profile.AtkDefMode));
                        profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.MacroSwitch));
                        profile.TransferHelper = JsonConvert.DeserializeObject<TransferHelper>(Profile.GetByAction(rawObject, profile.TransferHelper));
                        profile.DebuffsRecovery = JsonConvert.DeserializeObject<DebuffRecovery>(Profile.GetByAction(rawObject, profile.DebuffsRecovery));
                        profile.WeightDebuffsRecovery = JsonConvert.DeserializeObject<DebuffRecovery>(Profile.GetByAction(rawObject, profile.WeightDebuffsRecovery));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem loading the profile: {ex.Message}. Delete the Profiles folder and try again.");
            }
        }

        /// <summary>
        /// THREAD-SAFE: Clear profile with lock protection
        /// </summary>
        public static void ClearProfile(string profileName)
        {
            lock (profileLock)
            {
                if (profileName != profile.Name)
                {
                    profile = new Profile(profileName);
                }
            }
        }

        public static void Create(string profileName)
        {
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
            try
            {
                if (profileName != "Default") { File.Delete(AppConfig.ProfileFolder + profileName + ".json"); }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - file may already be deleted
                Console.WriteLine($"Failed to delete profile '{profileName}': {ex.Message}");
            }
        }

        /// <summary>
        /// THREAD-SAFE: Rename profile with lock protection
        /// </summary>
        public static void Rename(string oldProfileName, string newProfileName)
        {
            try
            {
                if (oldProfileName == "Default")
                {
                    throw new Exception("Cannot rename the Default profile!");
                }

                string oldFilePath = AppConfig.ProfileFolder + oldProfileName + ".json";
                string newFilePath = AppConfig.ProfileFolder + newProfileName + ".json";

                if (!File.Exists(oldFilePath))
                {
                    throw new Exception("Profile file does not exist!");
                }
                if (File.Exists(newFilePath))
                {
                    throw new Exception("A profile with the new name already exists!");
                }

                // Rename the file
                File.Move(oldFilePath, newFilePath);

                // Update the current profile if it was the one renamed
                lock (profileLock)
                {
                    if (profile.Name == oldProfileName)
                    {
                        profile.Name = newProfileName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to rename profile: {ex.Message}");
            }
        }

        /// <summary>
        /// THREAD-SAFE: Set configuration with lock protection
        /// </summary>
        public static void SetConfiguration(IAction action)
        {
            lock (profileLock)
            {
                if (profile != null)
                {
                    string jsonData = File.ReadAllText(AppConfig.ProfileFolder + profile.Name + ".json");
                    dynamic jsonObj = JsonConvert.DeserializeObject(jsonData);
                    jsonObj[action.GetActionName()] = action.GetConfiguration();
                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(AppConfig.ProfileFolder + profile.Name + ".json", output);
                }
            }
        }

        /// <summary>
        /// THREAD-SAFE: Get current profile with lock protection
        /// </summary>
        public static Profile GetCurrent()
        {
            lock (profileLock)
            {
                return profile;
            }
        }

        public static void Copy(string sourceProfileName, string destinationProfileName)
        {
            if (string.IsNullOrWhiteSpace(destinationProfileName) || destinationProfileName.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid new profile name.");
            }
            if (string.IsNullOrWhiteSpace(sourceProfileName))
            {
                throw new ArgumentException("Source profile name cannot be empty.");
            }

            string sourceFilePath = AppConfig.ProfileFolder + sourceProfileName + ".json";
            string destinationFilePath = AppConfig.ProfileFolder + destinationProfileName + ".json";

            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException($"Source profile '{sourceProfileName}' not found.");
            }

            if (File.Exists(destinationFilePath))
            {
                throw new ArgumentException($"A profile named '{destinationProfileName}' already exists.");
            }

            try
            {
                if (!Directory.Exists(AppConfig.ProfileFolder))
                {
                    Directory.CreateDirectory(AppConfig.ProfileFolder);
                }

                File.Copy(sourceFilePath, destinationFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error copying profile file: {ex.Message}", ex);
            }
        }
    }

    public class Profile
    {
        public string Name { get; set; }
        public ConfigProfile UserPreferences { get; set; }
        public SkillSpammer SkillSpammer { get; set; }
        public Autopot Autopot { get; set; }
        public Autopot AutopotYgg { get; set; }
        public SkillTimer SkillTimer { get; set; }
        public AutoBuffSkill AutobuffSkill { get; set; }
        public AutoBuffItem AutobuffItem { get; set; }
        public StatusRecovery StatusRecovery { get; set; }
        public DebuffRecovery DebuffsRecovery { get; set; }
        public DebuffRecovery WeightDebuffsRecovery { get; set; }
        public Macro SongMacro { get; set; }
        public Macro MacroSwitch { get; set; }
        public TransferHelper TransferHelper { get; set; }
        public ATKDEF AtkDefMode { get; set; }

        public Profile(string name)
        {
            this.Name = name;

            this.UserPreferences = new ConfigProfile();
            this.SkillSpammer = new SkillSpammer();
            this.Autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT);
            this.AutopotYgg = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG);
            this.SkillTimer = new SkillTimer();
            this.AutobuffSkill = new AutoBuffSkill(AutoBuffSkill.ACTION_NAME_AUTOBUFFSKILL);
            this.AutobuffItem = new AutoBuffItem(AutoBuffItem.ACTION_NAME_AUTOBUFFITEM);
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new Macro(Macro.ACTION_NAME_SONG_MACRO, MacroSongForm.TOTAL_MACRO_LANES_FOR_SONGS);
            this.MacroSwitch = new Macro(Macro.ACTION_NAME_MACRO_SWITCH, MacroSwitchForm.TOTAL_MACRO_LANES);
            this.AtkDefMode = new ATKDEF(ATKDEFForm.TOTAL_ATKDEF_LANES);
            this.DebuffsRecovery = new DebuffRecovery("DebuffsRecovery");
            this.WeightDebuffsRecovery = new DebuffRecovery("WeightDebuffsRecovery");
            this.TransferHelper = new TransferHelper();
        }

        public static object GetByAction(dynamic obj, IAction action)
        {
            if (obj != null && obj[action.GetActionName()] != null)
            {
                return obj[action.GetActionName()].ToString();
            }

            return action.GetConfiguration();
        }

        public static List<string> ListAll()
        {
            List<string> profiles = new List<string>();
            try
            {
                // Ensure profile folder exists before reading
                if (!Directory.Exists(AppConfig.ProfileFolder))
                {
                    return profiles; // Return empty list if folder doesn't exist
                }

                string[] files = Directory.GetFiles(AppConfig.ProfileFolder);

                foreach (string fileName in files)
                {
                    string[] len = fileName.Split('\\');
                    string profileName = len[len.Length - 1].Split('.')[0];
                    profiles.Add(profileName);
                }
            }
            catch (Exception ex)
            {
                // Log error and return empty list
                Console.WriteLine($"Failed to list profiles: {ex.Message}");
            }
            return profiles;
        }
    }
}