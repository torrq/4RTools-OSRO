using System;
using System.IO;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class Config
    {
        public string Language { get; set; } = "en";
        public bool DebugMode { get; set; } = false;
    }

    internal class ConfigManager
    {
        private static readonly string ConfigFile = "config.json";
        private static Config config;

        public static void Initialize()
        {
            try
            {
                EnsureConfigFileExists();
                LoadConfig();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to initialize config.json");
            }
        }

        private static void EnsureConfigFileExists()
        {
            if (!File.Exists(ConfigFile) || string.IsNullOrWhiteSpace(File.ReadAllText(ConfigFile)))
            {
                Config defaultConfig = new Config();
                string defaultJson = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);
                File.WriteAllText(ConfigFile, defaultJson);
                DebugLogger.Info("Created config.json with default data");
            }
        }

        private static void LoadConfig()
        {
            try
            {
                string json = File.ReadAllText(ConfigFile);
                config = JsonConvert.DeserializeObject<Config>(json) ?? new Config();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Error loading config.json");
                config = new Config();
            }
        }

        public static Config GetConfig()
        {
            if (config == null)
            {
                Initialize();
            }
            return config;
        }

        public static void SaveConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigFile, json);
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to save config.json");
            }
        }

        public static void SetLanguage(string languageCode)
        {
            GetConfig().Language = languageCode;
            SaveConfig();
        }
    }
}
