using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using _4RTools.Utils;

namespace _4RTools.Model
{
    internal class LocalServerManager
    {
        private static readonly string ServersFile = "servers.json";
        private static readonly string CitiesFile = "cities.json";
        private static List<string> cityList;

        public static string GetCitiesFile()
        {
            return CitiesFile;
        }

        public static void Initialize()
        {
            try
            {
                EnsureServersFileExists();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to initialize servers file");
            }

            try
            {
                EnsureCitiesFileExists();
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to initialize cities file");
            }
        }

        private static void EnsureServersFileExists()
        {
            dynamic server = AppConfig.ServerMode == 0
                ? new
                {
                    name = "OsRO Midrate",
                    hpAddress = "00E8F434",
                    nameAddress = "00E91C00",
                    mapAddress = "00E8ABD4"
                }
                : new
                {
                    name = "OSRO",
                    hpAddress = "010DCE10",
                    nameAddress = "010DF5D8",
                    mapAddress = "010D856C"
                };

            string defaultJson = JsonConvert.SerializeObject(new List<dynamic> { server }, Formatting.Indented);

            try
            {
                if (!File.Exists(ServersFile) || string.IsNullOrWhiteSpace(File.ReadAllText(ServersFile)) || File.ReadAllText(ServersFile).Length < 10)
                {
                    File.WriteAllText(ServersFile, defaultJson);
                    DebugLogger.Info("Created or updated servers.json with default data");
                }
            }
            catch (IOException ex)
            {
                DebugLogger.Error(ex, "IO error writing servers.json");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                DebugLogger.Error(ex, "Permission denied for servers.json");
                throw;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Unexpected error with servers.json");
                throw;
            }
        }


        private static void EnsureCitiesFileExists()
        {
            List<string> defaultCities = new List<string>
            {
                "prontera", "morocc", "geffen", "payon", "alberta", "izlude", "aldebaran", "xmas",
                "comodo", "yuno", "amatsu", "gonryun", "umbala", "niflheim", "louyang", "jawaii",
                "ayothaya", "einbroch", "lighthalzen", "einbech", "hugel", "rachel", "veins",
                "moscovia", "mid_camp", "munak", "splendide", "brasilis", "dicastes01", "mora",
                "dewata", "malangdo", "malaya", "eclage", "marketplace", "mainhall", "quiz_00"
            };

            try
            {
                if (!File.Exists(CitiesFile) || string.IsNullOrWhiteSpace(File.ReadAllText(CitiesFile)) || File.ReadAllText(CitiesFile).Length < 10)
                {
                    string json = JsonConvert.SerializeObject(defaultCities, Formatting.Indented);
                    File.WriteAllText(CitiesFile, json);
                    DebugLogger.Info("Created or updated cities.json with default data");
                }
            }
            catch (IOException ex)
            {
                DebugLogger.Error(ex, "IO error writing cities.json");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                DebugLogger.Error(ex, "Permission denied for cities.json");
                throw;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Unexpected error with cities.json");
                throw;
            }
        }

        private static string LoadLocalServerFile()
        {
            try
            {
                return File.Exists(ServersFile) ? File.ReadAllText(ServersFile) : string.Empty;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Error reading servers.json");
                return string.Empty;
            }
        }

        public static List<ClientDTO> GetLocalClients()
        {
            string localServers = LoadLocalServerFile();

            if (string.IsNullOrEmpty(localServers))
            {
                DebugLogger.Warning("servers.json is empty or could not be read");
                return new List<ClientDTO>();
            }

            try
            {
                return JsonConvert.DeserializeObject<List<ClientDTO>>(localServers);
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Error deserializing servers.json");
                return new List<ClientDTO>();
            }
        }

        private static string LoadLocalCityNameFile()
        {
            try
            {
                return File.Exists(CitiesFile) ? File.ReadAllText(CitiesFile) : string.Empty;
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Error reading cities.json");
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
                    DebugLogger.Warning("cities.json is empty or could not be read");
                    return new List<string>();
                }

                try
                {
                    cityList = JsonConvert.DeserializeObject<List<string>>(localCities);
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, "Error deserializing cities.json");
                    return new List<string>();
                }
            }
            return cityList;
        }
    }
}