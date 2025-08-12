using _ORTools.Forms;
using _ORTools.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace _ORTools.Model
{
    public class Profile
    {
        public string Name { get; set; }
        public ConfigProfile UserPreferences { get; set; }
        public SkillSpammer SkillSpammer { get; set; }
        public AutopotHP AutopotHP { get; set; }
        public AutopotSP AutopotSP { get; set; }
        public SkillTimer SkillTimer { get; set; }
        public AutoBuffSkill AutobuffSkill { get; set; }
        public AutoBuffItem AutobuffItem { get; set; }
        public StatusRecovery StatusRecovery { get; set; }
        public DebuffRecovery DebuffsRecovery { get; set; }
        public MacroSong SongMacro { get; set; }
        public MacroSwitch MacroSwitch { get; set; }
        public TransferHelper TransferHelper { get; set; }
        public ATKDEF AtkDefMode { get; set; }

        public Profile(string name)
        {
            this.Name = name;
            this.UserPreferences = new ConfigProfile();
            this.UserPreferences.ConfigVersion = AppConfig.ConfigVersion;
            this.SkillSpammer = new SkillSpammer();
            this.AutopotHP = new AutopotHP(AutopotHP.ACTION_NAME_AUTOPOT_HP);
            this.AutopotSP = new AutopotSP(AutopotSP.ACTION_NAME_AUTOPOT_SP);
            this.SkillTimer = new SkillTimer();
            this.AutobuffSkill = new AutoBuffSkill(AutoBuffSkill.ACTION_NAME_AUTOBUFFSKILL);
            this.AutobuffItem = new AutoBuffItem(AutoBuffItem.ACTION_NAME_AUTOBUFFITEM);
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new MacroSong();
            this.MacroSwitch = new MacroSwitch(MacroSwitch.ACTION_NAME_MACRO_SWITCH, MacroSwitchKey.TOTAL_MACRO_LANES);
            this.AtkDefMode = new ATKDEF(ATKDEFForm.TOTAL_ATKDEF_LANES);
            this.DebuffsRecovery = new DebuffRecovery("DebuffsRecovery");
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
                string[] files = Directory.GetFiles(AppConfig.ProfileFolder);

                foreach (string fileName in files)
                {
                    string[] len = fileName.Split('\\');
                    string profileName = len[len.Length - 1].Split('.')[0];
                    profiles.Add(profileName);
                }
            }
            catch { }
            return profiles;
        }
    }

    public static class ProfileSingleton
    {
        private static Profile profile = new Profile("Default");

        public static void Load(string profileName)
        {
            try
            {
                string filePath = AppConfig.ProfileFolder + profileName + ".json";
                if (!File.Exists(filePath))
                {
                    Create(profileName); // Create profile if it doesn't exist
                    return;
                }

                string json = File.ReadAllText(filePath);
                dynamic rawObject = JsonConvert.DeserializeObject(json);

                if (rawObject != null)
                {
                    profile.Name = profileName;
                    profile.UserPreferences = JsonConvert.DeserializeObject<ConfigProfile>(Profile.GetByAction(rawObject, profile.UserPreferences));
                    profile.SkillSpammer = JsonConvert.DeserializeObject<SkillSpammer>(Profile.GetByAction(rawObject, profile.SkillSpammer));
                    profile.AutopotHP = JsonConvert.DeserializeObject<AutopotHP>(Profile.GetByAction(rawObject, profile.AutopotHP));
                    profile.AutopotSP = JsonConvert.DeserializeObject<AutopotSP>(Profile.GetByAction(rawObject, profile.AutopotSP));

                    try
                    {
                        string statusRecoveryConfig = Profile.GetByAction(rawObject, profile.StatusRecovery).ToString();
                        profile.StatusRecovery = new StatusRecovery();
                        profile.StatusRecovery.LoadConfiguration(statusRecoveryConfig);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Error(ex, $"Failed to load StatusRecovery configuration: {ex.Message}");
                        profile.StatusRecovery = new StatusRecovery(); // Use default if loading fails
                    }
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
                    profile.SongMacro = JsonConvert.DeserializeObject<MacroSong>(Profile.GetByAction(rawObject, profile.SongMacro));
                    profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDEF>(Profile.GetByAction(rawObject, profile.AtkDefMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<MacroSwitch>(Profile.GetByAction(rawObject, profile.MacroSwitch));
                    profile.TransferHelper = JsonConvert.DeserializeObject<TransferHelper>(Profile.GetByAction(rawObject, profile.TransferHelper));
                    profile.DebuffsRecovery = JsonConvert.DeserializeObject<DebuffRecovery>(Profile.GetByAction(rawObject, profile.DebuffsRecovery));
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Failed to load profile '{profileName}': {ex.Message}");
                throw new Exception($"There was a problem loading the profile: {ex.Message}. Delete the Profiles folder and try again.", ex);
            }
        }

        public static void ClearProfile(string profileName)
        {
            if (profileName != profile.Name)
            {
                profile = new Profile(profileName);
            }
        }

        public static void Create(string profileName)
        {
            string jsonFileName = AppConfig.ProfileFolder + profileName + ".json";

            if (!File.Exists(jsonFileName))
            {
                try
                {
                    if (!Directory.Exists(AppConfig.ProfileFolder))
                    {
                        Directory.CreateDirectory(AppConfig.ProfileFolder);
                    }
                    ClearProfile(profileName);
                    FileStream fs = File.Create(jsonFileName);
                    fs.Close();

                    Profile newProfile = new Profile(profileName);
                    string output = JsonConvert.SerializeObject(newProfile, Formatting.Indented);
                    File.WriteAllText(jsonFileName, output);
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Failed to create profile '{profileName}': {ex.Message}");
                }
            }
        }

        public static void Delete(string profileName)
        {
            try
            {
                if (profileName != "Default") { File.Delete(AppConfig.ProfileFolder + profileName + ".json"); }
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Failed to delete profile '{profileName}': {ex.Message}");
            }
        }

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
                if (profile.Name == oldProfileName)
                {
                    profile.Name = newProfileName;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Failed to rename profile from '{oldProfileName}' to '{newProfileName}': {ex.Message}");
                throw new Exception($"Failed to rename profile: {ex.Message}", ex);
            }
        }

        public static void SetConfiguration(IAction action)
        {
            if (profile != null)
            {
                string filePath = AppConfig.ProfileFolder + profile.Name + ".json";
                try
                {
                    // Create profile file if it doesn't exist
                    if (!File.Exists(filePath))
                    {
                        Create(profile.Name);
                    }

                    // Read existing JSON into a JObject
                    string jsonData = File.ReadAllText(filePath);
                    var jsonObj = !string.IsNullOrEmpty(jsonData) ?
                    JsonConvert.DeserializeObject<JObject>(jsonData) :
                    new JObject();

                    // Get the configuration from the action as a JSON string
                    string actionConfigString = action.GetConfiguration();

                    // Parse the string into a JToken (which will be a JObject)
                    JToken actionConfigToken = JToken.Parse(actionConfigString);

                    // Assign the parsed JToken to the property of the main JObject
                    jsonObj[action.GetActionName()] = actionConfigToken;

                    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

                    File.WriteAllText(filePath, output);
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Failed to save configuration for action '{action.GetActionName()}' to profile '{profile.Name}': {ex.Message}");
                }
            }
        }

        public static Profile GetCurrent()
        {
            if (profile == null)
            {
                Create("Default"); // Ensure default profile exists
                Load("Default");
            }
            return profile;
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
                DebugLogger.Error(ex, $"Failed to copy profile from '{sourceProfileName}' to '{destinationProfileName}': {ex.Message}");
                throw new Exception($"Error copying profile file: {ex.Message}", ex);
            }
        }
    }
}