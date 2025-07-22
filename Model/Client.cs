using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public class ClientDTO
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HPAddress { get; set; }
        public string NameAddress { get; set; }
        public string MapAddress { get; set; }
        public string JobAdress { get; set; }
        public string OnlineAddress { get; set; }
        public int HPAddressPointer { get; set; }
        public int NameAddressPointer { get; set; }
        public int MapAddressPointer { get; set; }
        public int JobAddressPointer { get; set; }
        public int OnlineAddressPointer { get; set; }

        public ClientDTO() { }

        public ClientDTO(string name, string description, string hpAddress, string nameAddress, string mapAddress, string jobAddress, string onlineAddress)
        {
            this.Name = name;
            this.Description = description;
            this.HPAddress = hpAddress;
            this.NameAddress = nameAddress;
            this.MapAddress = mapAddress;
            this.JobAdress = jobAddress;
            this.OnlineAddress = onlineAddress;

            this.HPAddressPointer = Convert.ToInt32(hpAddress, 16);
            this.NameAddressPointer = Convert.ToInt32(nameAddress, 16);
            this.MapAddressPointer = Convert.ToInt32(mapAddress, 16);
            this.JobAddressPointer = Convert.ToInt32(jobAddress, 16);
            this.OnlineAddressPointer = Convert.ToInt32(onlineAddress, 16);
        }
    }

    public sealed class ClientListSingleton
    {
        private static List<Client> Clients = new List<Client>();

        public static void AddClient(Client c)
        {
            Clients.Add(c);
        }

        public static void RemoveClient(Client c)
        {
            Clients.Remove(c);
        }

        public static List<Client> GetAll()
        {
            return Clients;
        }

        public static bool ExistsByProcessName(string processName)
        {
            return Clients.Exists(client => client.ProcessName == processName);
        }

        // Check if any client is logged in
        public static bool IsLoggedIn()
        {
            return Clients.Any(client => client.IsLoggedIn);
        }

        // Check if a specific client is logged in by process name
        public static bool IsLoggedIn(string processName)
        {
            Client client = Clients.FirstOrDefault(c => c.ProcessName == processName);
            return client?.IsLoggedIn ?? false;
        }

        // Get all logged in clients
        public static List<Client> GetLoggedInClients()
        {
            return Clients.Where(client => client.IsLoggedIn).ToList();
        }

        // Force refresh login status cache for all clients
        public static void RefreshAllLoginStatus()
        {
            foreach (Client client in Clients)
            {
                client.RefreshLoginStatus();
            }
        }
    }

    public sealed class ClientSingleton
    {
        private static Client client;
        private ClientSingleton(Client client)
        {
            ClientSingleton.client = client;
        }

        public static ClientSingleton Instance(Client client)
        {
            return new ClientSingleton(client);
        }

        public static Client GetClient()
        {
            return client;
        }
    }

    public class Client
    {
        public Process Process { get; }

        public string ProcessName { get; private set; }
        private Utils.ProcessMemoryReader PMR { get; set; }
        public int CurrentNameAddress { get; set; }
        public int CurrentHPBaseAddress { get; set; }
        public int CurrentMapAddress { get; set; }
        public int CurrentJobAddress { get; set; }
        public int CurrentOnlineAddress { get; set; }
        private int StatusBufferAddress { get; set; }

        private int _num = 0;

        // Caching for login status
        private bool? _cachedLoginStatus = null;
        private DateTime _lastLoginStatusCheck = DateTime.MinValue;
        private readonly TimeSpan _loginStatusCacheTimeout = TimeSpan.FromMilliseconds(500); // Cache for 500ms

        public bool IsLoggedIn
        {
            get
            {
                if (Process == null || Process.HasExited)
                {
                    DebugLogger.Debug($"IsLoggedIn: Process is null or has exited for {ProcessName}.");
                    _cachedLoginStatus = false;
                    return false;
                }

                // Check if we have a valid cached value
                if (_cachedLoginStatus.HasValue &&
                    DateTime.Now - _lastLoginStatusCheck < _loginStatusCacheTimeout)
                {
                    return _cachedLoginStatus.Value;
                }

                // Read the login status from memory
                bool loginStatus = ReadLoginStatus();

                // Update cache
                _cachedLoginStatus = loginStatus;
                _lastLoginStatusCheck = DateTime.Now;

                return loginStatus;
            }
        }

        // Method to force refresh the login status cache
        public void RefreshLoginStatus()
        {
            _cachedLoginStatus = null;
            _lastLoginStatusCheck = DateTime.MinValue;
        }

        private bool ReadLoginStatus()
        {
            bool isLoggedIn = false;
            try
            {
                // HR
                if (AppConfig.ServerMode == 1)
                {
                    uint loginValue = ReadMemory(CurrentOnlineAddress);
                    isLoggedIn = loginValue > 0;
                    return isLoggedIn;
                }
                // LR
                else if (AppConfig.ServerMode == 2) { return isLoggedIn; }
                // MR
                else
                {
                    uint loginValue = ReadMemory(CurrentOnlineAddress);
                    isLoggedIn = loginValue == 3571961;
                    return isLoggedIn;
                }

            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error reading login status for {ProcessName}: {ex.Message}");
                return false; // Assume not logged in if we can't read the value
            }
        }

        public Client(string processName, int currentHPBaseAddress, int currentNameAddress, int currentMapAddress, int currentJobAddress, int currentOnlineAddress)
        {
            this.CurrentNameAddress = currentNameAddress;
            this.CurrentHPBaseAddress = currentHPBaseAddress;
            this.CurrentMapAddress = currentMapAddress;
            this.CurrentJobAddress = currentJobAddress;
            this.CurrentOnlineAddress = currentOnlineAddress;
            this.ProcessName = processName;

            if (AppConfig.ServerMode == 1) // HR
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x470;
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for HR client.");
            }
            else if (AppConfig.ServerMode == 2) // LR
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x474; // Placeholder for LR, to be updated with correct offset
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for LR client.");
            }
            else // Default to MR (ServerMode == 0)
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x474;
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for MR client.");
            }

            //CurrentJobAddress = currentJobAddress;
        }

        public Client(ClientDTO dto)
        {
            this.ProcessName = dto.Name;
            this.CurrentHPBaseAddress = Convert.ToInt32(dto.HPAddress, 16);
            this.CurrentNameAddress = Convert.ToInt32(dto.NameAddress, 16);
            this.CurrentMapAddress = Convert.ToInt32(dto.MapAddress, 16);
            this.CurrentJobAddress = Convert.ToInt32(dto.JobAdress, 16);
            this.CurrentOnlineAddress = Convert.ToInt32(dto.OnlineAddress, 16);

            if (AppConfig.ServerMode == 1) // HR
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x470;
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for HR client.");
            }
            else if (AppConfig.ServerMode == 2) // LR
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x474; // Placeholder for LR, to be updated with correct offset
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for LR client.");
            }
            else // Default to MR (ServerMode == 0)
            {
                this.StatusBufferAddress = this.CurrentHPBaseAddress + 0x474;
                //DebugLogger.Debug($"StatusBufferAddress set to: 0x{this.StatusBufferAddress:X8} for MR client.");
            }
        }

        public Client(string processName)
        {
            PMR = new Utils.ProcessMemoryReader();
            string rawProcessName = processName.Split(new string[] { ".exe - " }, StringSplitOptions.None)[0];
            int choosenPID = int.Parse(processName.Split(new string[] { ".exe - " }, StringSplitOptions.None)[1]);

            foreach (Process process in Process.GetProcessesByName(rawProcessName))
            {
                if (choosenPID == process.Id)
                {
                    this.Process = process;
                    PMR.ReadProcess = process;
                    PMR.OpenProcess();

                    try
                    {
                        Client c = GetClientByProcess(rawProcessName) ?? throw new Exception();
                        this.CurrentHPBaseAddress = c.CurrentHPBaseAddress;
                        this.CurrentNameAddress = c.CurrentNameAddress;
                        this.CurrentMapAddress = c.CurrentMapAddress;
                        this.CurrentJobAddress = c.CurrentJobAddress;
                        this.CurrentOnlineAddress = c.CurrentOnlineAddress;
                        this.StatusBufferAddress = c.StatusBufferAddress;
                    }
                    catch
                    {
                        //MessageBox.Show("This client is not supported. Only Spammers and macro will works.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.CurrentHPBaseAddress = 0;
                        this.CurrentNameAddress = 0;
                        this.CurrentMapAddress = 0;
                        this.CurrentJobAddress = 0;
                        this.CurrentOnlineAddress = 0;
                        this.StatusBufferAddress = 0;
                    }
                }
            }
        }

        private string ReadMemoryAsString(int address)
        {
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)address, 40u, out _num);
            List<byte> buffer = new List<byte>();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0) break;
                buffer.Add(bytes[i]);
            }
            return Encoding.Default.GetString(buffer.ToArray());
        }

        private uint ReadMemory(int address)
        {
            return BitConverter.ToUInt32(PMR.ReadProcessMemory((IntPtr)address, 4u, out _num), 0);
        }

        public bool IsHpBelow(int percent)
        {
            return ReadCurrentHp() * 100 < percent * ReadMaxHp();
        }

        public bool IsSpBelow(int percent)
        {
            return ReadCurrentSp() * 100 < percent * ReadMaxSp();
        }

        public uint ReadCurrentHp()
        {
            return ReadMemory(this.CurrentHPBaseAddress);
        }

        public uint ReadCurrentSp()
        {
            return ReadMemory(this.CurrentHPBaseAddress + 8);
        }

        public uint ReadMaxHp()
        {
            return ReadMemory(this.CurrentHPBaseAddress + 4);
        }

        public string ReadCharacterName()
        {
            return ReadMemoryAsString(this.CurrentNameAddress);
        }

        public string ReadCurrentMap()
        {
            return ReadMemoryAsString(this.CurrentMapAddress);
        }

        public uint ReadMaxSp()
        {
            return ReadMemory(this.CurrentHPBaseAddress + 12);
        }

        public uint ReadCurrentJob()
        {
            return ReadMemory(this.CurrentJobAddress);
        }

        public uint ReadCurrentExp()
        {
            switch (AppConfig.ServerMode)
            {
                case 1: // HR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 2));
                case 2: // LR #FIXME
                    return ReadMemory(this.CurrentJobAddress + 0);
                default: // MR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + 4);
            }
        }

        public uint ReadCurrentExpToLevel()
        {
            switch (AppConfig.ServerMode)
            {
                case 1: // HR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 4));
                case 2: // LR #FIXME
                    return ReadMemory(this.CurrentJobAddress + 0);
                default: // MR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 3));
            }
        }

        public uint ReadCurrentLevel()
        {
            switch (AppConfig.ServerMode)
            {
                case 1: // HR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 10));
                case 2: // LR #FIXME
                    return ReadMemory(this.CurrentJobAddress + 0);
                default: // MR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 9));
            }
        }

        public uint ReadCurrentJobLevel()
        {
            switch (AppConfig.ServerMode)
            {
                case 1: // HR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 12));
                case 2: // LR #FIXME
                    return ReadMemory(this.CurrentJobAddress + 0);
                default: // MR - correct as of 2025-07-05
                    return ReadMemory(this.CurrentJobAddress + (4 * 11));
            }
        }

        public uint CurrentBuffStatusCode(int effectStatusIndex)
        {
            return ReadMemory(this.StatusBufferAddress + (effectStatusIndex * 4));
        }

        public Client GetClientByProcess(string processName)
        {
            foreach (Client c in ClientListSingleton.GetAll())
            {
                if (c.ProcessName == processName)
                {
                    uint hpBaseValue = ReadMemory(c.CurrentHPBaseAddress);
                    if (hpBaseValue > 0) return c;
                }
            }
            return null;
        }

        public static Client FromDTO(ClientDTO dto)
        {
            return ClientListSingleton.GetAll()
                .Where(c => c.ProcessName == dto.Name)
                .Where(c => c.CurrentHPBaseAddress == dto.HPAddressPointer)
                .Where(c => c.CurrentNameAddress == dto.NameAddressPointer)
                .Where(c => c.CurrentJobAddress == dto.JobAddressPointer)
                .Where(c => c.CurrentOnlineAddress == dto.OnlineAddressPointer)
                .FirstOrDefault();
        }

        public static bool IsClientWindowActive()
        {
            try
            {
                IntPtr activeWindowHandle = Win32Interop.GetForegroundWindow();

                uint activeProcessId;
                Win32Interop.GetWindowThreadProcessId(activeWindowHandle, out activeProcessId);

                Process activeProcess = Process.GetProcessById((int)activeProcessId);
                return activeProcess.ProcessName.Equals(ClientSingleton.GetClient().Process.ProcessName, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SendKeysToClientIfActive(byte bVk, byte bScan, int dwFlags, int dwExtraInfo)
        {
            if (IsClientWindowActive())
            {
                Win32Interop.keybd_event(bVk, bScan, dwFlags, dwExtraInfo);
            }
        }

        public void Kill()
        {
            try
            {
                if (this.Process != null && !this.Process.HasExited)
                {
                    this.Process.Kill();
                    DebugLogger.Info($"Successfully killed client process: {this.Process.ProcessName} (PID: {this.Process.Id})");
                }
                else
                {
                    DebugLogger.Info("Process was already exited or null.");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Info($"Failed to kill client process: {ex.Message}");
            }
        }
    }
}