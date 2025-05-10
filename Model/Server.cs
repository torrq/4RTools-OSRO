using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace _4RTools.Model
{
    internal class Server
    {
        private static List<string> cityList;

        public static string GetCitiesFile()
        {
            return AppConfig.CitiesFile;
        }

        public static void Initialize()
        {
            try
            {
                EnsureServersFileExists();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Failed to initialize {AppConfig.ServersFile}");
            }

            try
            {
                EnsureCitiesFileExists();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Failed to initialize {AppConfig.CitiesFile}");
            }
        }

        private static void EnsureServersFileExists()
        {
            string defaultJson = JsonConvert.SerializeObject(AppConfig.DefaultServers, Formatting.Indented);

            try
            {
                if (!File.Exists(AppConfig.ServersFile) || string.IsNullOrWhiteSpace(File.ReadAllText(AppConfig.ServersFile)) || File.ReadAllText(AppConfig.ServersFile).Length < 10)
                {
                    File.WriteAllText(AppConfig.ServersFile, defaultJson);
                    DebugLogger.Info($"Created or updated {AppConfig.ServersFile} with default data");
                }
            }
            catch (IOException ex)
            {
                DebugLogger.Error(ex, $"IO error writing {AppConfig.ServersFile}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                DebugLogger.Error(ex, $"Permission denied for {AppConfig.ServersFile}");
                throw;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Unexpected error with {AppConfig.ServersFile}");
                throw;
            }
        }

        private static void EnsureCitiesFileExists()
        {
            try
            {
                if (!File.Exists(AppConfig.CitiesFile) || string.IsNullOrWhiteSpace(File.ReadAllText(AppConfig.CitiesFile)) || File.ReadAllText(AppConfig.CitiesFile).Length < 10)
                {
                    string json = JsonConvert.SerializeObject(AppConfig.DefaultCities, Formatting.Indented);
                    File.WriteAllText(AppConfig.CitiesFile, json);
                    DebugLogger.Info($"Created or updated {AppConfig.CitiesFile} with default data");
                }
            }
            catch (IOException ex)
            {
                DebugLogger.Error(ex, $"IO error writing {AppConfig.CitiesFile}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                DebugLogger.Error(ex, $"Permission denied for {AppConfig.CitiesFile}");
                throw;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Unexpected error with {AppConfig.CitiesFile}");
                throw;
            }
        }

        private static string LoadLocalServerFile()
        {
            try
            {
                return File.Exists(AppConfig.ServersFile) ? File.ReadAllText(AppConfig.ServersFile) : string.Empty;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Error reading {AppConfig.ServersFile}");
                return string.Empty;
            }
        }

        public static List<ClientDTO> GetLocalClients()
        {
            string localServers = LoadLocalServerFile();

            if (string.IsNullOrEmpty(localServers))
            {
                DebugLogger.Warning($"{AppConfig.ServersFile} is empty or could not be read");
                return new List<ClientDTO>();
            }

            try
            {
                return JsonConvert.DeserializeObject<List<ClientDTO>>(localServers);
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Error deserializing {AppConfig.ServersFile}");
                return new List<ClientDTO>();
            }
        }

        private static string LoadLocalCityNameFile()
        {
            try
            {
                return File.Exists(AppConfig.CitiesFile) ? File.ReadAllText(AppConfig.CitiesFile) : string.Empty;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, $"Error reading {AppConfig.CitiesFile}");
                return string.Empty;
            }
        }

        public static List<string> GetCityList()
        {
            if (cityList == null || cityList.Count == 0)
            {
                string localCities = LoadLocalCityNameFile();

                if (string.IsNullOrEmpty(localCities))
                {
                    DebugLogger.Warning($"{AppConfig.CitiesFile} is empty or could not be read");
                    return new List<string>();
                }

                try
                {
                    cityList = JsonConvert.DeserializeObject<List<string>>(localCities);
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Error deserializing {AppConfig.CitiesFile}");
                    return new List<string>();
                }
            }
            return cityList;
        }
    }
}