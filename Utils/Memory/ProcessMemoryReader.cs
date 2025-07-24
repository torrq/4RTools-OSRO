using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _ORTools.Utils
{
    /// <summary>
    /// Reads memory from a target process with optimized checks and buffer reuse.
    /// Enhanced with better error handling for process crashes and memory protection changes.
    /// </summary>
    public class ProcessMemoryReader : IDisposable
    {
        private Process _process;
        private IntPtr _processHandle;
        private bool _disposed = false;

        // Cached exit-state to avoid repeated HasExited calls
        private bool? _cachedHasExited;
        private DateTime _lastExitCheck;
        private readonly TimeSpan _exitCheckInterval = TimeSpan.FromSeconds(1);

        // Handle validation cache
        private bool? _cachedHandleValid;
        private DateTime _lastHandleCheck;
        private readonly TimeSpan _handleCheckInterval = TimeSpan.FromMilliseconds(500);

        // Error tracking for automatic recovery
        private int _consecutiveErrors = 0;
        private const int MAX_CONSECUTIVE_ERRORS = 3;
        private DateTime _lastErrorTime = DateTime.MinValue;

        /// <summary>
        /// Gets whether the process handle is currently valid and accessible.
        /// </summary>
        public bool IsProcessValid
        {
            get
            {
                try
                {
                    if (_disposed || _processHandle == IntPtr.Zero)
                        return false;

                    // Use cached result if recent
                    if (_cachedHandleValid.HasValue &&
                        DateTime.UtcNow - _lastHandleCheck < _handleCheckInterval)
                    {
                        return _cachedHandleValid.Value;
                    }

                    // Check if process has exited first (cheaper check)
                    if (HasProcessExited())
                    {
                        _cachedHandleValid = false;
                        return false;
                    }

                    // Try a simple operation to validate handle
                    uint processId = MemoryApi.GetProcessId(_processHandle);
                    if (processId == 0) { DebugLogger.Debug("IsProcessValid: processId == 0"); return false; }

                    _cachedHandleValid = true;
                    _lastHandleCheck = DateTime.UtcNow;
                    return true;
                }
                catch
                {
                    _cachedHandleValid = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Opens the target process with read/query access and resets the exit cache.
        /// </summary>
        public bool OpenProcess(int processId)
        {
            try
            {
                if (_processHandle != IntPtr.Zero)
                {
                    // Close existing handle first
                    Dispose();
                }

                _process = Process.GetProcessById(processId);
                _processHandle = MemoryApi.OpenProcess(
                    MemoryApi.ProcessAccessFlags.PROCESS_VM_READ |
                    MemoryApi.ProcessAccessFlags.PROCESS_QUERY_INFORMATION,
                    false, processId);

                if (_processHandle == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    DebugLogger.Debug($"Failed to open process {processId}, error: {error}");
                    return false;
                }

                ResetCaches();
                _consecutiveErrors = 0;
                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Exception opening process {processId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Clears all cached states so the next checks will query the OS.
        /// </summary>
        private void ResetCaches()
        {
            _cachedHasExited = null;
            _lastExitCheck = DateTime.MinValue;
            _cachedHandleValid = null;
            _lastHandleCheck = DateTime.MinValue;
        }

        /// <summary>
        /// Checks if the process has exited, using cached result when appropriate.
        /// </summary>
        private bool HasProcessExited()
        {
            if (_process == null)
                return true;

            // Only refresh HasExited once per interval
            if (!_cachedHasExited.HasValue ||
                DateTime.UtcNow - _lastExitCheck > _exitCheckInterval)
            {
                try
                {
                    _lastExitCheck = DateTime.UtcNow;
                    _cachedHasExited = _process.HasExited;
                }
                catch
                {
                    // If we can't check, assume it's exited
                    _cachedHasExited = true;
                }
            }

            return _cachedHasExited.Value;
        }

        /// <summary>
        /// Ensures the process handle is valid and that the process hasn't exited.
        /// </summary>
        private void EnsureProcessOpen()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ProcessMemoryReader));

            if (_processHandle == IntPtr.Zero)
                throw new InvalidOperationException("Process handle is not open. Call OpenProcess() first.");

            if (!IsProcessValid)
            {
                // Invalidate caches and throw
                ResetCaches();
                throw new InvalidOperationException("Target process is no longer accessible or has exited.");
            }
        }

        /// <summary>
        /// Reads memory into a provided buffer with enhanced error handling.
        /// Returns true if successful, false if the read should be retried later.
        /// </summary>
        public bool TryReadProcessMemory(IntPtr baseAddress, uint bytesToRead, byte[] buffer, out int bytesRead)
        {
            bytesRead = 0;

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer.Length < bytesToRead)
            {
                throw new ArgumentException("Buffer too small.", nameof(buffer));
            }

            if (_disposed)
            {
                return false;
            }

            // Quick validation without expensive checks
            if (_processHandle == IntPtr.Zero)
            {
                return false;
            }

            try
            {
                bool success = MemoryApi.ReadProcessMemory(
                    _processHandle,
                    baseAddress,
                    buffer,
                    (int)bytesToRead,
                    out bytesRead);

                if (success)
                {
                    _consecutiveErrors = 0;
                    return true;
                }

                // Handle specific error codes
                int error = Marshal.GetLastWin32Error();
                return HandleReadError(error, baseAddress);
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Exception in TryReadProcessMemory: {ex.Message}");
                _consecutiveErrors++;
                return false;
            }
        }

        /// <summary>
        /// Handles read errors and determines if the operation should be retried.
        /// </summary>
        private bool HandleReadError(int errorCode, IntPtr address)
        {
            _consecutiveErrors++;
            _lastErrorTime = DateTime.UtcNow;
            var processId = _process?.Id.ToString() ?? "N/A";

            switch (errorCode)
            {
                case 299: // ERROR_PARTIAL_COPY
                    DebugLogger.Debug($"ERROR_PARTIAL_COPY (299) at 0x{address:X} for PID {processId}. Errors: {_consecutiveErrors}");

                    // After several consecutive errors, invalidate caches
                    if (_consecutiveErrors >= MAX_CONSECUTIVE_ERRORS)
                    {
                        DebugLogger.Debug("Too many consecutive errors, invalidating process caches.");
                        ResetCaches();
                        _cachedHandleValid = false;
                    }
                    return false;

                case 5: // ERROR_ACCESS_DENIED
                    DebugLogger.Debug($"Access denied (5) at 0x{address:X} for PID {processId}.");
                    ResetCaches();
                    return false;

                case 6: // ERROR_INVALID_HANDLE
                    DebugLogger.Debug($"Invalid handle (6) for PID {processId}, process may have exited.");
                    ResetCaches();
                    _cachedHandleValid = false;
                    _cachedHasExited = true;
                    return false;

                default:
                    DebugLogger.Debug($"ReadProcessMemory failed with error {errorCode} at 0x{address:X} for PID {processId}");
                    return false;
            }
        }

        /// <summary>
        /// Reads memory into a provided buffer (throwing on failure).
        /// Use TryReadProcessMemory for non-throwing version.
        /// </summary>
        public void ReadProcessMemory(IntPtr baseAddress, uint bytesToRead, byte[] buffer, out int bytesRead)
        {
            EnsureProcessOpen();

            if (!TryReadProcessMemory(baseAddress, bytesToRead, buffer, out bytesRead))
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"ReadProcessMemory failed with error code {error}.");
            }
        }

        /// <summary>
        /// Convenience method: allocates a buffer, reads memory, and returns the bytes actually read.
        /// Returns null if the read fails and throwOnError is false.
        /// </summary>
        public byte[] ReadProcessMemory(IntPtr baseAddress, uint bytesToRead, bool throwOnError = true)
        {
            try
            {
                var buffer = new byte[bytesToRead];

                if (throwOnError)
                {
                    ReadProcessMemory(baseAddress, bytesToRead, buffer, out var read);
                    if (read != buffer.Length)
                        Array.Resize(ref buffer, read);
                    return buffer;
                }
                else
                {
                    if (TryReadProcessMemory(baseAddress, bytesToRead, buffer, out var read))
                    {
                        if (read != buffer.Length)
                            Array.Resize(ref buffer, read);
                        return buffer;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (throwOnError)
                    throw;

                DebugLogger.Debug($"ReadProcessMemory failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Gets the number of consecutive read errors since the last successful read.
        /// </summary>
        public int ConsecutiveErrorCount => _consecutiveErrors;

        /// <summary>
        /// Gets the time of the last read error.
        /// </summary>
        public DateTime LastErrorTime => _lastErrorTime;

        /// <summary>
        /// Manually resets the error counter (useful after process recovery).
        /// </summary>
        public void ResetErrorCount()
        {
            _consecutiveErrors = 0;
            ResetCaches();
        }

        /// <summary>
        /// Closes the process handle and disposes resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_processHandle != IntPtr.Zero)
                {
                    MemoryApi.CloseHandle(_processHandle);
                    _processHandle = IntPtr.Zero;
                }

                _process?.Dispose();
                _process = null;

                _disposed = true;
            }
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

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetProcessId(IntPtr hProcess);
    }
}