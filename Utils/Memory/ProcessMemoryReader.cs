using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _ORTools.Utils
{
    /// <summary>
    /// Reads memory from a target process with optimized checks and buffer reuse.
    /// </summary>
    public class ProcessMemoryReader : IDisposable
    {
        private Process _process;
        private IntPtr _processHandle;

        // Cached exit-state to avoid repeated HasExited calls
        private bool? _cachedHasExited;
        private DateTime _lastExitCheck;
        private readonly TimeSpan _exitCheckInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Opens the target process with read/query access and resets the exit cache.
        /// </summary>
        public void OpenProcess(int processId)
        {
            if (_processHandle != IntPtr.Zero)
                throw new InvalidOperationException("Process is already open.");

            _process = Process.GetProcessById(processId);
            _processHandle = MemoryApi.OpenProcess(
                MemoryApi.ProcessAccessFlags.PROCESS_VM_READ |
                MemoryApi.ProcessAccessFlags.PROCESS_QUERY_INFORMATION,
                false, processId);

            ResetExitCache();
        }

        /// <summary>
        /// Clears the cached exit state so the next check will query the OS.
        /// </summary>
        private void ResetExitCache()
        {
            _cachedHasExited = null;
            _lastExitCheck = DateTime.MinValue;
        }

        /// <summary>
        /// Ensures the process handle is valid and that the process hasn't exited,
        /// refreshing the cached state at most once per interval.
        /// </summary>
        private void EnsureProcessOpen()
        {
            if (_processHandle == IntPtr.Zero)
                throw new InvalidOperationException("Process handle is not open. Call OpenProcess() first.");

            // Only refresh HasExited once per interval
            if (!_cachedHasExited.HasValue ||
                DateTime.UtcNow - _lastExitCheck > _exitCheckInterval)
            {
                _lastExitCheck = DateTime.UtcNow;
                _cachedHasExited = _process.HasExited;
            }

            if (_cachedHasExited.Value)
                throw new InvalidOperationException("Target process has exited.");
        }

        /// <summary>
        /// Reads memory into a provided buffer (avoiding allocations).
        /// Caller must ensure buffer.Length >= bytesToRead.
        /// </summary>
        public void ReadProcessMemory(IntPtr baseAddress, uint bytesToRead, byte[] buffer, out int bytesRead)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length < bytesToRead)
                throw new ArgumentException("Buffer too small.", nameof(buffer));

            // Skip expensive checks in tight loops; assume caller ensures open
            bool success = MemoryApi.ReadProcessMemory(
                _processHandle,
                baseAddress,
                buffer,
                (int)bytesToRead,
                out bytesRead);

            if (!success)
            {
                int err = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"ReadProcessMemory failed with error code {err}.");
            }
        }

        /// <summary>
        /// Convenience method: allocates a buffer, reads memory, and returns the bytes actually read.
        /// </summary>
        public byte[] ReadProcessMemory(IntPtr baseAddress, uint bytesToRead)
        {
            var buffer = new byte[bytesToRead];
            ReadProcessMemory(baseAddress, bytesToRead, buffer, out var read);
            if (read != buffer.Length)
                Array.Resize(ref buffer, read);
            return buffer;
        }

        /// <summary>
        /// Closes the process handle and disposes resources.
        /// </summary>
        public void Dispose()
        {
            if (_processHandle != IntPtr.Zero)
            {
                MemoryApi.CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
            }
            _process?.Dispose();
        }
    }

    internal static class MemoryApi
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            PROCESS_VM_READ = 0x0010,
            PROCESS_QUERY_INFORMATION = 0x0400
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
