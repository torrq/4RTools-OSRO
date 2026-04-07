using _ORTools.Forms;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private static DateTime _lastCleanup = DateTime.MinValue;
        private static readonly TimeSpan _cleanupInterval = TimeSpan.FromSeconds(30);

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
            // Note: Automatic cleanup disabled to prevent removing clients with temporary memory read issues
            // Use ForceCleanup() manually if needed
            return Clients;
        }

        /// <summary>
        /// Removes clients whose processes have actually exited (not just having memory read issues).
        /// </summary>
        public static void RemoveDeadClients()
        {
            var deadClients = new List<Client>();

            foreach (var client in Clients)
            {
                try
                {
                    // Only remove if process is truly null or has actually exited
                    if (client.Process == null)
                    {
                        deadClients.Add(client);
                        continue;
                    }

                    // Double check - only remove if process has actually exited
                    if (client.Process.HasExited)
                    {
                        deadClients.Add(client);
                    }
                }
                catch (Exception ex)
                {
                    // If we can't check the process state, assume it's dead
                    DebugLogger.Debug($"Exception checking process state for {client.ProcessName}: {ex.Message}");
                    deadClients.Add(client);
                }
            }

            foreach (var deadClient in deadClients)
            {
                DebugLogger.Debug($"Removing truly dead client: {deadClient.ProcessName}");
                Clients.Remove(deadClient);
                deadClient.Dispose();
            }
        }

        /// <summary>
        /// Forces cleanup of dead clients immediately.
        /// </summary>
        public static void ForceCleanup()
        {
            RemoveDeadClients();
            _lastCleanup = DateTime.Now;
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
        private static volatile Client client;
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

    public class Client : IDisposable
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

        // Caching for login status
        private bool? _cachedLoginStatus = null;
        private DateTime _lastLoginStatusCheck = DateTime.MinValue;
        private readonly TimeSpan _loginStatusCacheTimeout = TimeSpan.FromMilliseconds(500); // Cache for 500ms

        // Process validity caching
        private bool? _cachedProcessValid = null;
        private DateTime _lastProcessValidCheck = DateTime.MinValue;
        private readonly TimeSpan _processValidCacheTimeout = TimeSpan.FromMilliseconds(1000); // Cache for 1 second

        private bool _disposed = false;

        private IntPtr _cachedMainWindowHandle = IntPtr.Zero;
        
        /// <summary>
        /// Cached access to the MainWindowHandle. Avoids slow Process queries on every tick.
        /// </summary>
        public IntPtr MainWindowHandle
        {
            get
            {
                if (_cachedMainWindowHandle == IntPtr.Zero && Process != null)
                {
                    try { _cachedMainWindowHandle = Process.MainWindowHandle; } catch { }
                }
                return _cachedMainWindowHandle;
            }
        }

        /// <summary>
        /// Checks if the process is valid and accessible for memory operations.
        /// Only checks basic process state, not memory accessibility.
        /// </summary>
        public bool IsProcessRunning()
        {
            // Check if we have a valid cached value
            if (_cachedProcessValid.HasValue &&
                DateTime.Now - _lastProcessValidCheck < _processValidCacheTimeout)
            {
                return _cachedProcessValid.Value;
            }

            // Perform basic process validity check only
            bool isValid = false;
            try
            {
                if (Process != null && !Process.HasExited)
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"IsProcessValid check failed for {ProcessName}: {ex.Message}");
                isValid = false;
            }

            // Update cache
            _cachedProcessValid = isValid;
            _lastProcessValidCheck = DateTime.Now;

            return isValid;
        }

        /// <summary>
        /// Safely reads a 4-byte unsigned integer from memory with error handling.
        /// This is an alternative to ReadMemory() for cases where you want graceful failure.
        /// </summary>
        private uint ReadMemorySafe(int address)
        {
            try
            {
                if (!IsProcessRunning() || address == 0)
                    return 0;

                // Use non-throwing version
                byte[] bytes = PMR.ReadProcessMemory((IntPtr)address, 4u, throwOnError: false);
                if (bytes == null || bytes.Length < 4)
                {
                    return 0;
                }
                return BitConverter.ToUInt32(bytes, 0);
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"ReadMemorySafe failed for {ProcessName}: {ex.Message}");
                _cachedProcessValid = false;
                return 0;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                if (Process == null || Process.HasExited)
                {
                    DebugLogger.Debug($"IsLoggedIn: Process is null or has exited for {ProcessName}.");
                    _cachedLoginStatus = false;
                    return _cachedLoginStatus.Value;
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
            _cachedProcessValid = null;
            _lastProcessValidCheck = DateTime.MinValue;
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
                    // OPEN handle with the process ID
                    PMR.OpenProcess(process.Id);

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

        // ── Bulk read helpers ─────────────────────────────────────────────────────

        /// <summary>
        /// HP/SP values captured in a single 16-byte read.
        /// Offsets: CurrentHp+0, MaxHp+4, CurrentSp+8, MaxSp+12.
        /// </summary>
        public struct HpSpSnapshot
        {
            public uint CurrentHp;
            public uint MaxHp;
            public uint CurrentSp;
            public uint MaxSp;

            /// <summary>Returns true when HP is below <paramref name="percent"/>% of max.</summary>
            public bool IsHpBelow(int percent) =>
                CurrentHp * 100 < (uint)percent * MaxHp;

            /// <summary>Returns true when SP is below <paramref name="percent"/>% of max.</summary>
            public bool IsSpBelow(int percent) =>
                CurrentSp * 100 < (uint)percent * MaxSp;
        }

        /// <summary>
        /// Reads CurrentHp, MaxHp, CurrentSp, MaxSp in a single 16-byte
        /// ReadProcessMemory call instead of four separate 4-byte calls.
        /// Returns a zeroed snapshot on failure.
        /// Also pushes the result into HpSpCache for the character info panel.
        /// </summary>
        public HpSpSnapshot ReadHpSp()
        {
            if (CurrentHPBaseAddress == 0) return default;
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)CurrentHPBaseAddress, 16u, throwOnError: false);
            if (bytes == null || bytes.Length < 16) return default;
            var snap = new HpSpSnapshot
            {
                CurrentHp = BitConverter.ToUInt32(bytes, 0),
                MaxHp     = BitConverter.ToUInt32(bytes, 4),
                CurrentSp = BitConverter.ToUInt32(bytes, 8),
                MaxSp     = BitConverter.ToUInt32(bytes, 12),
            };
            HpSpCache.Push(snap);
            return snap;
        }

        /// <summary>
        /// Job/level/exp values captured in a single read spanning
        /// CurrentJobAddress+0 through CurrentJobAddress+(12*4)=+48 bytes (13 uint slots).
        /// Slot layout differs by ServerMode — see ReadCurrentLevel/Exp for offsets.
        /// </summary>
        public struct JobSnapshot
        {
            private readonly uint[] _slots;
            private readonly int _mode;

            public JobSnapshot(uint[] slots, int mode) { _slots = slots; _mode = mode; }

            public uint JobId        => _slots[0];
            public uint Exp          => _mode == 1 ? _slots[2]  : _slots[1];
            public uint ExpToLevel   => _mode == 1 ? _slots[4]  : _slots[3];
            public uint Level        => _mode == 1 ? _slots[10] : _slots[9];
            public uint JobLevel     => _mode == 1 ? _slots[12] : _slots[11];
        }

        /// <summary>
        /// Reads job/level/exp data in a single 52-byte ReadProcessMemory call
        /// instead of five individual 4-byte calls.
        /// Returns null on failure.
        /// </summary>
        public JobSnapshot? ReadJobBlock()
        {
            if (CurrentJobAddress == 0) return null;
            // Need slots 0-12 → 13 uints → 52 bytes
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)CurrentJobAddress, 52u, throwOnError: false);
            if (bytes == null || bytes.Length < 52) return null;
            var slots = new uint[13];
            for (int i = 0; i < 13; i++)
                slots[i] = BitConverter.ToUInt32(bytes, i * 4);
            return new JobSnapshot(slots, AppConfig.ServerMode);
        }


        /// (MAX_BUFF_LIST_INDEX_SIZE × 4 bytes) instead of one call per index.
        /// Returns null on failure; otherwise an array of length MAX_BUFF_LIST_INDEX_SIZE.
        /// </summary>
        // ── Status buffer ─────────────────────────────────────────────────────
        public uint[] ReadStatusBuffer()
        {
            var cached = StatusBufferCache.Latest;
            if (cached != null) return cached;

            if (StatusBufferAddress == 0) return null;
            int count = Constants.MAX_BUFF_LIST_INDEX_SIZE;
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)StatusBufferAddress, (uint)(count * 4), throwOnError: false);
            if (bytes == null || bytes.Length < count * 4) return null;
            var result = new uint[count];
            for (int i = 0; i < count; i++)
                result[i] = BitConverter.ToUInt32(bytes, i * 4);
            StatusBufferCache.Push(result);
            return result;
        }

        // ── Individual reads (kept for single-value use-cases) ────────────────
        private string ReadMemoryAsString(int address)
        {
            // use the convenience overload (no out param)
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)address, 40u);
            int len = Array.IndexOf(bytes, (byte)0);
            if (len < 0) len = bytes.Length;
            return Encoding.Default.GetString(bytes, 0, len);
        }

        private uint ReadMemory(int address)
        {
            // use the convenience overload
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)address, 4u);
            return BitConverter.ToUInt32(bytes, 0);
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

        // Shared 1-second map cache — all macro threads call this instead of ReadCurrentMap()
        // so at most one RPM read per second regardless of how many macros are running.
        private volatile string _cachedMap = string.Empty;
        private long _mapCacheTicks = 0;
        private const long MAP_CACHE_TICKS = 10_000_000; // 1s in ticks

        public string ReadCurrentMapCached()
        {
            long now = DateTime.UtcNow.Ticks;
            if (now - System.Threading.Interlocked.Read(ref _mapCacheTicks) > MAP_CACHE_TICKS)
            {
                _cachedMap = ReadCurrentMap() ?? string.Empty;
                System.Threading.Interlocked.Exchange(ref _mapCacheTicks, now);
            }
            return _cachedMap;
        }

        /// <summary>
        /// Returns true if a text input box (chat, search, etc.) is currently active.
        /// When true, macros should not send keys to avoid interrupting typing.
        /// Returns false on HR (address unknown) so behaviour is unchanged there.
        /// Logs a debug message when the state changes (0→1 or 1→0) to aid address verification.
        /// </summary>
        /// <summary>
        /// Reads current and max weight in a single 8-byte RPM call.
        /// MaxWeight is at WeightAddress-4 (E8BB24), current at WeightAddress (E8BB28).
        /// Returns (0, 0) on HR where addresses are unknown.
        /// </summary>
        public (uint current, uint max) ReadWeight()
        {
            int addr = AppConfig.MaxWeightAddress;
            if (addr == 0) return (0, 0);
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)addr, 8u, throwOnError: false);
            if (bytes == null || bytes.Length < 8) return (0, 0);
            uint max     = BitConverter.ToUInt32(bytes, 0);
            uint current = BitConverter.ToUInt32(bytes, 4);
            return (current, max);
        }

        /// <summary>
        /// Returns true when the player has a text input box focused (chat, search, etc.).
        /// Only byte 0 at TextInputActiveAddress is checked — b0=0x01 means active.
        /// Returns false on HR where the address is unknown.
        /// </summary>
        /// <summary>
        /// Returns true when the character is dead (HP == 1 in RO).
        /// Uses HpSpCache so no extra RPM call is needed.
        /// </summary>
        public bool IsDead()
        {
            if (!ConfigGlobal.GetConfig().PauseWhenDead) return false;
            var snap = HpSpCache.Latest;
            if (!snap.IsValid || snap.Snapshot.MaxHp == 0) return false;
            return snap.Snapshot.CurrentHp == 1;
        }

        public bool IsTextInputActive()
        {
            if (!ConfigGlobal.GetConfig().PauseWhenChatting) return false;
            int addr = AppConfig.TextInputActiveAddress;
            if (addr == 0) return false;
            byte[] bytes = PMR.ReadProcessMemory((IntPtr)addr, 4u, throwOnError: false);
            return bytes != null && bytes.Length >= 1 && bytes[0] != 0;
        }

        // Fixed coordinates addresses (MR only — HR unknown)

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

        /// <summary>
        /// Disposes the client and releases all resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    PMR?.Dispose();
                    Process?.Dispose();
                }
                catch (Exception ex)
                {
                    DebugLogger.Debug($"Error disposing client {ProcessName}: {ex.Message}");
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }

    /// <summary>
    /// Shared, lock-free cache for the most recent HP/SP snapshot.
    /// Written on every ReadHpSp() call (macro threads); read by CharacterInfo timer.
    /// </summary>
    /// <summary>
    /// Shared status buffer cache. Threads share one RPM read per TTL window.
    /// Call Invalidate() after casting to force a fresh read next cycle.
    /// </summary>
    public static class StatusBufferCache
    {
        private const long TTL_TICKS = 500_000; // 50ms

        private static volatile uint[] _buffer = null;
        private static long _timestamp = 0;

        public static void Push(uint[] buffer)
        {
            _buffer = buffer;
            System.Threading.Interlocked.Exchange(ref _timestamp, DateTime.UtcNow.Ticks);
        }

        public static uint[] Latest
        {
            get
            {
                long age = DateTime.UtcNow.Ticks - System.Threading.Interlocked.Read(ref _timestamp);
                return age < TTL_TICKS ? _buffer : null;
            }
        }

        public static void Invalidate()
        {
            System.Threading.Interlocked.Exchange(ref _timestamp, 0);
        }
    }

    public static class HpSpCache
    {
        private static volatile HpSpCacheEntry _entry = new HpSpCacheEntry();

        public static void Push(Client.HpSpSnapshot snap)
        {
            _entry = new HpSpCacheEntry { Snapshot = snap, Timestamp = DateTime.UtcNow };
        }

        /// <summary>Returns the cached snapshot and its age. Returns null if never populated.</summary>
        public static HpSpCacheEntry Latest => _entry;

        public class HpSpCacheEntry
        {
            public Client.HpSpSnapshot Snapshot;
            public DateTime Timestamp;
            public bool IsValid => Timestamp != default;
        }
    }
}