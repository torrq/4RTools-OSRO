using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BruteGamingMacros.Core.Model
{
    public class ClientDTO
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HPAddress { get; set; }
        public string NameAddress { get; set; }
        public string MapAddress { get; set; }
        public string OnlineAddress { get; set; }
        public int HPAddressPointer { get; set; }
        public int NameAddressPointer { get; set; }
        public int MapAddressPointer { get; set; }
        public int OnlineAddressPointer { get; set; }

        public ClientDTO() { }

        public ClientDTO(string name, string description, string hpAddress, string nameAddress, string mapAddress, string onlineAddress)
        {
            this.Name = name;
            this.Description = description;
            this.HPAddress = hpAddress;
            this.NameAddress = nameAddress;
            this.MapAddress = mapAddress;
            this.OnlineAddress = onlineAddress;

            this.HPAddressPointer = Convert.ToInt32(hpAddress, 16);
            this.NameAddressPointer = Convert.ToInt32(nameAddress, 16);
            this.MapAddressPointer = Convert.ToInt32(mapAddress, 16);
            this.OnlineAddressPointer = Convert.ToInt32(onlineAddress, 16);
        }
    }

    /// <summary>
    /// THREAD-SAFE: Manages list of clients with lock protection
    /// </summary>
    public sealed class ClientListSingleton
    {
        private static List<Client> Clients = new List<Client>();
        private static readonly object clientsLock = new object();

        public static void AddClient(Client c)
        {
            lock (clientsLock)
            {
                Clients.Add(c);
            }
        }

        public static void RemoveClient(Client c)
        {
            lock (clientsLock)
            {
                Clients.Remove(c);
            }
        }

        public static List<Client> GetAll()
        {
            lock (clientsLock)
            {
                // Return a copy to prevent external modification
                return new List<Client>(Clients);
            }
        }

        public static bool ExistsByProcessName(string processName)
        {
            lock (clientsLock)
            {
                return Clients.Exists(client => client.ProcessName == processName);
            }
        }
    }

    /// <summary>
    /// THREAD-SAFE: Singleton client manager with lock protection
    /// </summary>
    public sealed class ClientSingleton
    {
        private static Client client;
        private static readonly object clientLock = new object();

        private ClientSingleton(Client client)
        {
            lock (clientLock)
            {
                ClientSingleton.client = client;
            }
        }

        public static ClientSingleton Instance(Client client)
        {
            return new ClientSingleton(client);
        }

        public static Client GetClient()
        {
            lock (clientLock)
            {
                return client;
            }
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
        public int CurrentOnlineAddress { get; set; }
        private int StatusBufferAddress { get; set; }
        private int _num = 0;

        public Client(string processName, int currentHPBaseAddress, int currentNameAddress, int currentMapAddress, int currentOnlineAddress)
        {
            this.CurrentNameAddress = currentNameAddress;
            this.CurrentHPBaseAddress = currentHPBaseAddress;
            this.CurrentMapAddress = currentMapAddress;
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
        }

        public Client(ClientDTO dto)
        {
            this.ProcessName = dto.Name;
            this.CurrentHPBaseAddress = Convert.ToInt32(dto.HPAddress, 16);
            this.CurrentNameAddress = Convert.ToInt32(dto.NameAddress, 16);
            this.CurrentMapAddress = Convert.ToInt32(dto.MapAddress, 16);
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
                        this.CurrentOnlineAddress = c.CurrentOnlineAddress;
                        this.StatusBufferAddress = c.StatusBufferAddress;
                    }
                    catch
                    {
                        MessageBox.Show("This client is not supported. Only Spammers and macro will works.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.CurrentHPBaseAddress = 0;
                        this.CurrentNameAddress = 0;
                        this.CurrentMapAddress = 0;
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

        /// <summary>
        /// BUG FIX: Properly check online status instead of always returning true
        /// </summary>
        public bool IsOnline()
        {
            try
            {
                // Validate that we have a valid CurrentOnlineAddress
                if (CurrentOnlineAddress == 0)
                {
                    // If address is not configured, assume online (fallback for unsupported clients)
                    return true;
                }

                byte[] bytes = PMR.ReadProcessMemory((IntPtr)CurrentOnlineAddress, 1u, out _num);
                if (_num == 1)
                {
                    return bytes[0] == 1; // 1 = online, 0 = offline
                }

                // If read failed, log warning but assume online to prevent disruption
                DebugLogger.Warning($"Failed to read online status at address 0x{CurrentOnlineAddress:X8}");
                return true;
            }
            catch (Exception ex)
            {
                // On error, log and assume online to prevent disruption
                DebugLogger.Error($"Error reading online status: {ex.Message}");
                return true;
            }
        }

        public void WriteMemory(int address, uint intToWrite)
        {
            PMR.WriteProcessMemory((IntPtr)address, BitConverter.GetBytes(intToWrite), out _num);
        }

        public void WriteMemory(int address, byte[] bytesToWrite)
        {
            PMR.WriteProcessMemory((IntPtr)address, bytesToWrite, out _num);
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
                .Where(c => c.CurrentOnlineAddress == dto.OnlineAddressPointer)
                .FirstOrDefault();
        }
    }
}