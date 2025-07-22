using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _ORTools.Utils
{
    internal class ProcessMemoryReader : IDisposable
    {
        private static class MemoryApi
        {
            [Flags]
            public enum AllocType : uint
            {
                Commit = 0x1000,
                Reserve = 0x2000,
                Decommit = 0x4000,
                Reset = 0x80000,
                TopDown = 0x100000,
                WriteWatch = 0x200000,
                Physical = 0x400000,
                LargePages = 0x20000000
            }

            [Flags]
            public enum Protect : uint
            {
                NoAccess = 0x01,
                ReadOnly = 0x02,
                ReadWrite = 0x04,
                WriteCopy = 0x08,
                Execute = 0x10,
                ExecuteRead = 0x20,
                ExecuteReadWrite = 0x40,
                ExecuteWriteCopy = 0x80,
                Guard = 0x100,
                NoCache = 0x200,
                WriteCombine = 0x400
            }

            [Flags]
            public enum FreeType : uint
            {
                Decommit = 0x4000,
                Release = 0x8000
            }

            [Flags]
            public enum ProcessAccessRights : uint
            {
                Delete = 0x00010000,
                ReadControl = 0x00020000,
                WriteDac = 0x00040000,
                WriteOwner = 0x00080000,
                Synchronize = 0x00100000,
                StandardRightsRequired = 0x000F0000,
                StandardRightsRead = 0x00020000,
                StandardRightsWrite = 0x00020000,
                StandardRightsExecute = 0x00020000,
                StandardRightsAll = 0x001F0000,
                SpecificRightsAll = 0x0000FFFF,
                AccessSystemSecurity = 0x01000000,
                MaximumAllowed = 0x02000000,
                GenericRead = 0x80000000,
                GenericWrite = 0x40000000,
                GenericExecute = 0x20000000,
                GenericAll = 0x10000000,
                ProcessTerminate = 0x0001,
                ProcessCreateThread = 0x0002,
                ProcessSetSessionId = 0x0004,
                ProcessVmOperation = 0x0008,
                ProcessVmRead = 0x0010,
                ProcessVmWrite = 0x0020,
                ProcessDupHandle = 0x0040,
                ProcessCreateProcess = 0x0080,
                ProcessSetQuota = 0x0100,
                ProcessSetInformation = 0x0200,
                ProcessQueryInformation = 0x0400,
                ProcessQueryLimitedInformation = 0x1000,
                ProcessAllAccess = StandardRightsRequired | Synchronize | 0xFFFF
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr OpenProcess(ProcessAccessRights dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, AllocType flAllocationType, Protect flProtect);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();
        }

        private Process _readProcess;
        private IntPtr _processHandle = IntPtr.Zero;
        private bool _disposed = false;

        private void LogError(string message)
        {
            DebugLogger.Error($"[ProcessMemoryReader] {message}");
        }

        /// <summary>
        /// Gets or sets the target process for memory operations
        /// </summary>
        public Process ReadProcess
        {
            get => _readProcess;
            set
            {
                if (_readProcess != value)
                {
                    CloseHandle(); // Close existing handle if any
                    _readProcess = value;
                }
            }
        }

        /// <summary>
        /// Gets whether the process handle is currently open
        /// </summary>
        public bool IsProcessOpen => _processHandle != IntPtr.Zero;

        /// <summary>
        /// Opens a handle to the target process with required permissions
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no process is set or process access fails</exception>
        public void OpenProcess()
        {
            if (_readProcess == null)
            {
                LogError("No process set. Set ReadProcess property before calling OpenProcess.");
                return;
            }

            if (_readProcess.HasExited)
            {
                LogError("Target process has exited.");
                return;
            }

            // Close existing handle if open
            if (_processHandle != IntPtr.Zero)
            {
                CloseHandle();
            }

            const MemoryApi.ProcessAccessRights desiredAccess =
                MemoryApi.ProcessAccessRights.ProcessVmOperation |
                MemoryApi.ProcessAccessRights.ProcessVmRead |
                MemoryApi.ProcessAccessRights.ProcessVmWrite;

            _processHandle = MemoryApi.OpenProcess(desiredAccess, false, (uint)_readProcess.Id);

            if (_processHandle == IntPtr.Zero)
            {
                var error = MemoryApi.GetLastError();
                LogError($"Failed to open process. Error code: {error}");
            }
        }

        /// <summary>
        /// Closes the process handle if it's open
        /// </summary>
        public void CloseHandle()
        {
            if (_processHandle != IntPtr.Zero)
            {
                if (!MemoryApi.CloseHandle(_processHandle))
                {
                    var error = MemoryApi.GetLastError();
                    LogError($"Failed to close process handle. Error code: {error}");
                }
                _processHandle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Reads memory from the target process
        /// </summary>
        /// <param name="memoryAddress">The memory address to read from</param>
        /// <param name="bytesToRead">Number of bytes to read</param>
        /// <param name="bytesRead">Actual number of bytes read</param>
        /// <returns>Byte array containing the read data</returns>
        /// <exception cref="InvalidOperationException">Thrown when process is not open or read fails</exception>
        public byte[] ReadProcessMemory(IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
        {
            EnsureProcessOpen();

            var buffer = new byte[bytesToRead];
            bool success = MemoryApi.ReadProcessMemory(_processHandle, memoryAddress, buffer, (int)bytesToRead, out bytesRead);

            if (!success)
            {
                var error = MemoryApi.GetLastError();
                LogError($"Failed to read process memory at 0x{memoryAddress.ToInt64():X}. Error code: {error}");
            }

            // Return only the bytes that were actually read
            if (bytesRead < bytesToRead)
            {
                var actualBuffer = new byte[bytesRead];
                Array.Copy(buffer, actualBuffer, bytesRead);
                return actualBuffer;
            }

            return buffer;
        }

        /// <summary>
        /// Reads memory from the target process (overload for backward compatibility)
        /// </summary>
        public byte[] ReadProcessMemory(IntPtr memoryAddress, uint bytesToRead)
        {
            return ReadProcessMemory(memoryAddress, bytesToRead, out _);
        }

        /// <summary>
        /// Allocates memory in the target process
        /// </summary>
        /// <param name="addr">Returns the allocated memory address</param>
        /// <param name="size">Size of memory to allocate</param>
        /// <exception cref="InvalidOperationException">Thrown when allocation fails</exception>
        public void Alloc(out int addr, int size)
        {
            EnsureProcessOpen();

            IntPtr allocatedAddr = MemoryApi.VirtualAllocEx(
                _processHandle,
                IntPtr.Zero,
                size,
                MemoryApi.AllocType.Commit | MemoryApi.AllocType.Reserve,
                MemoryApi.Protect.ExecuteReadWrite);

            if (allocatedAddr == IntPtr.Zero)
            {
                var error = MemoryApi.GetLastError();
                LogError($"Failed to allocate {size} bytes in target process. Error code: {error}");
            }

            addr = allocatedAddr.ToInt32();
        }

        /// <summary>
        /// Allocates memory in the target process (alternative overload)
        /// </summary>
        /// <param name="size">Size of memory to allocate</param>
        /// <returns>The allocated memory address</returns>
        public IntPtr AllocateMemory(int size)
        {
            Alloc(out int addr, size);
            return new IntPtr(addr);
        }

        /// <summary>
        /// Deallocates memory in the target process
        /// </summary>
        /// <param name="addr">Address to deallocate</param>
        /// <returns>True if successful</returns>
        public bool Dealloc(int addr)
        {
            if (_processHandle == IntPtr.Zero)
                return false;

            return MemoryApi.VirtualFreeEx(_processHandle, new IntPtr(addr), 0, MemoryApi.FreeType.Release);
        }

        /// <summary>
        /// Deallocates memory in the target process (IntPtr overload)
        /// </summary>
        /// <param name="addr">Address to deallocate</param>
        /// <returns>True if successful</returns>
        public bool DeallocateMemory(IntPtr addr)
        {
            return Dealloc(addr.ToInt32());
        }

        /// <summary>
        /// Ensures the process handle is open, throws exception if not
        /// </summary>
        private void EnsureProcessOpen()
        {
            if (_processHandle == IntPtr.Zero)
            {
                LogError("Process handle is not open. Call OpenProcess() first.");
            }

            if (_readProcess?.HasExited == true)
            {
                LogError("Target process has exited.");
            }
        }

        /// <summary>
        /// Disposes of the ProcessMemoryReader and closes any open handles
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _readProcess = null;
                }

                // Dispose unmanaged resources
                if (_processHandle != IntPtr.Zero)
                {
                    MemoryApi.CloseHandle(_processHandle);
                    _processHandle = IntPtr.Zero;
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~ProcessMemoryReader()
        {
            Dispose(false);
        }
    }
}