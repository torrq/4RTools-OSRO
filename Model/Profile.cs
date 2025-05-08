using _4RTools.Forms;
using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _4RTools.Model
{
    public class ProfileSingleton
    {
        public static Profile profile = new Profile("Default");

        public static void Load(string profileName)
        {
            try
            {
                string json = File.ReadAllText(AppConfig.ProfileFolder + profileName + ".json");
                dynamic rawObject = JsonConvert.DeserializeObject(json);

                if ((rawObject != null))
                {
                    profile.Name = profileName;
                    profile.UserPreferences = JsonConvert.DeserializeObject<ConfigProfile>(Profile.GetByAction(rawObject, profile.UserPreferences));
                    profile.AHK = JsonConvert.DeserializeObject<AHK>(Profile.GetByAction(rawObject, profile.AHK));
                    profile.Autopot = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.Autopot));
                    profile.AutopotYgg = JsonConvert.DeserializeObject<Autopot>(Profile.GetByAction(rawObject, profile.AutopotYgg));
                    profile.StatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(Profile.GetByAction(rawObject, profile.StatusRecovery));
                    profile.AutoRefreshSpammer = JsonConvert.DeserializeObject<AutoRefreshSpammer>(Profile.GetByAction(rawObject, profile.AutoRefreshSpammer));
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
                    profile.AtkDefMode = JsonConvert.DeserializeObject<ATKDEFMode>(Profile.GetByAction(rawObject, profile.AtkDefMode));
                    profile.MacroSwitch = JsonConvert.DeserializeObject<Macro>(Profile.GetByAction(rawObject, profile.MacroSwitch));
                    profile.Custom = JsonConvert.DeserializeObject<Custom>(Profile.GetByAction(rawObject, profile.Custom));
                    profile.DebuffsRecovery = JsonConvert.DeserializeObject<DebuffsRecovery>(Profile.GetByAction(rawObject, profile.DebuffsRecovery));
                    profile.WeightDebuffsRecovery = JsonConvert.DeserializeObject<DebuffsRecovery>(Profile.GetByAction(rawObject, profile.WeightDebuffsRecovery));
                }
            }
            catch
            {
                throw new Exception("There was a problem loading the profile. Delete the Profiles folder and try again.");
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
            catch { }
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
                throw new Exception($"Failed to rename profile: {ex.Message}");
            }
        }

        public static void SetConfiguration(IAction action)
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

        public static Profile GetCurrent()
        {
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
                throw new Exception($"Error copying profile file: {ex.Message}", ex);
            }
        }
    }

    public class Profile
    {
        public string Name { get; set; }
        public ConfigProfile UserPreferences { get; set; }
        public AHK AHK { get; set; }
        public Autopot Autopot { get; set; }
        public Autopot AutopotYgg { get; set; }
        public AutoRefreshSpammer AutoRefreshSpammer { get; set; }
        public AutoBuffSkill AutobuffSkill { get; set; }
        public AutoBuffItem AutobuffItem { get; set; }
        public StatusRecovery StatusRecovery { get; set; }
        public DebuffsRecovery DebuffsRecovery { get; set; }
        public DebuffsRecovery WeightDebuffsRecovery { get; set; }
        public Macro SongMacro { get; set; }
        public Macro MacroSwitch { get; set; }
        public Custom Custom { get; set; }
        public ATKDEFMode AtkDefMode { get; set; }

        public Profile(string name)
        {
            this.Name = name;

            this.UserPreferences = new ConfigProfile();
            this.AHK = new AHK();
            this.Autopot = new Autopot(Autopot.ACTION_NAME_AUTOPOT);
            this.AutopotYgg = new Autopot(Autopot.ACTION_NAME_AUTOPOT_YGG);
            this.AutoRefreshSpammer = new AutoRefreshSpammer();
            this.AutobuffSkill = new AutoBuffSkill(AutoBuffSkill.ACTION_NAME_AUTOBUFFSKILL);
            this.AutobuffItem = new AutoBuffItem(AutoBuffItem.ACTION_NAME_AUTOBUFFITEM);
            this.StatusRecovery = new StatusRecovery();
            this.SongMacro = new Macro(Macro.ACTION_NAME_SONG_MACRO, MacroSongForm.TOTAL_MACRO_LANES_FOR_SONGS);
            this.MacroSwitch = new Macro(Macro.ACTION_NAME_MACRO_SWITCH, MacroSwitchForm.TOTAL_MACRO_LANES);
            this.AtkDefMode = new ATKDEFMode(ATKDEFForm.TOTAL_ATKDEF_LANES);
            this.DebuffsRecovery = new DebuffsRecovery("DebuffsRecovery");
            this.WeightDebuffsRecovery = new DebuffsRecovery("WeightDebuffsRecovery");
            this.Custom = new Custom();
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
}