﻿using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.IO;

namespace _ORTools.Model
{
    public class Config
    {
        public bool DebugMode { get; set; } = false;
        public bool DebugModeShowLog { get; set; } = true;
        public bool DisableSystray { get; set; } = false;
        public string LastUsedProfile { get; set; } = "Default";
        public bool MiniMode { get; set; } = false;
        public int SongRows { get; set; } = 4; // Added new global setting
        public int MacroSwitchRows { get; set; } = 4; // Added new global setting
    }

    internal static class ConfigGlobal
    {
        private static readonly string ConfigFile = AppConfig.ConfigFile;
        private static Config config;

        public static void Initialize()
        {
            try
            {
                EnsureConfigFolderExists();
                EnsureConfigFileExists();
                LoadConfig();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to initialize config.json");
            }
        }

        public static void EnsureConfigFolderExists()
        {
            if (!Directory.Exists(AppConfig.ConfigFolder))
            {
                Directory.CreateDirectory(AppConfig.ConfigFolder);
                DebugLogger.Info($"Created config folder: {AppConfig.ConfigFolder}");
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
    }
}