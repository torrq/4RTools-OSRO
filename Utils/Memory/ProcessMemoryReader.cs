using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _ORTools.Utils
{
    /// <summary>
    /// Enhanced ProcessMemoryReader with comprehensive error diagnostics
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
                    if (processId == 0)
                    {
                        int error = Marshal.GetLastWin32Error();
                        DebugLogger.Debug($"IsProcessValid: GetProcessId failed with error {error}");
                        return false;
                    }

                    _cachedHandleValid = true;
                    _lastHandleCheck = DateTime.UtcNow;
                    return true;
                }
                catch (Exception ex)
                {
                    DebugLogger.Debug($"IsProcessValid exception: {ex.Message}");
                    _cachedHandleValid = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Opens the target process with enhanced access rights and detailed error reporting
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

                // Log process information for diagnostics
                DebugLogger.Debug($"Opening process: PID {processId}, Name: {_process.ProcessName}, " +
                                $"MainWindowTitle: '{_process.MainWindowTitle}', " +
                                $"Responding: {_process.Responding}");

                // Try with enhanced access flags first
                _processHandle = MemoryApi.OpenProcess(
                    MemoryApi.ProcessAccessFlags.PROCESS_VM_READ |
                    MemoryApi.ProcessAccessFlags.PROCESS_QUERY_INFORMATION |
                    MemoryApi.ProcessAccessFlags.PROCESS_QUERY_LIMITED_INFORMATION,
                    false, processId);

                if (_processHandle == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    DebugLogger.Debug($"Failed to open process {processId} with enhanced flags, error: {error} ({GetErrorDescription(error)})");

                    // Try with minimal flags as fallback
                    _processHandle = MemoryApi.OpenProcess(
                        MemoryApi.ProcessAccessFlags.PROCESS_VM_READ,
                        false, processId);

                    if (_processHandle == IntPtr.Zero)
                    {
                        error = Marshal.GetLastWin32Error();
                        DebugLogger.Debug($"Failed to open process {processId} with minimal flags, error: {error} ({GetErrorDescription(error)})");
                        return false;
                    }
                    else
                    {
                        //DebugLogger.Debug($"Successfully opened process {processId} with minimal flags");
                    }
                }
                else
                {
                    //DebugLogger.Debug($"Successfully opened process {processId} with enhanced flags");
                }

                // Validate the handle immediately
                uint testPid = MemoryApi.GetProcessId(_processHandle);
                if (testPid != processId)
                {
                    DebugLogger.Debug($"Handle validation failed: Expected PID {processId}, got {testPid}");
                    Dispose();
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
        /// Validates a memory address before attempting to read from it
        /// </summary>
        public bool IsValidMemoryAddress(IntPtr address)
        {
            if (address == IntPtr.Zero)
            {
                DebugLogger.Debug("Invalid address: null pointer (0x0)");
                return false;
            }

            // Check if address is in valid user space range (Windows x64: 0x10000 to 0x7FFFFFFFFFFF)
            // For x86: 0x10000 to 0x7FFFFFFF
            long addr = address.ToInt64();

            if (IntPtr.Size == 8) // 64-bit
            {
                if (addr < 0x10000 || addr > 0x7FFFFFFFFFFF)
                {
                    DebugLogger.Debug($"Invalid address: 0x{addr:X} outside valid user space range");
                    return false;
                }
            }
            else // 32-bit
            {
                if (addr < 0x10000 || addr > 0x7FFFFFFF)
                {
                    DebugLogger.Debug($"Invalid address: 0x{addr:X} outside valid user space range");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads memory with comprehensive error handling and validation
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
                DebugLogger.Debug("TryReadProcessMemory: Object disposed");
                return false;
            }

            // Quick validation without expensive checks
            if (_processHandle == IntPtr.Zero)
            {
                DebugLogger.Debug("TryReadProcessMemory: Process handle is null");
                return false;
            }

            // Validate memory address
            if (!IsValidMemoryAddress(baseAddress))
            {
                return false;
            }

            // Log read attempt for diagnostics
            //DebugLogger.Debug($"Attempting to read {bytesToRead} bytes from 0x{baseAddress.ToInt64():X}");

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
                    //DebugLogger.Debug($"Successfully read {bytesRead} bytes from 0x{baseAddress.ToInt64():X}");
                    return true;
                }

                // Handle specific error codes
                int error = Marshal.GetLastWin32Error();
                return HandleReadError(error, baseAddress, bytesToRead);
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Exception in TryReadProcessMemory at 0x{baseAddress.ToInt64():X}: {ex.Message}");
                _consecutiveErrors++;
                return false;
            }
        }

        /// <summary>
        /// Enhanced error handling with detailed error descriptions
        /// </summary>
        private bool HandleReadError(int errorCode, IntPtr address, uint bytesToRead)
        {
            _consecutiveErrors++;
            _lastErrorTime = DateTime.UtcNow;
            var processId = _process?.Id.ToString() ?? "N/A";

            string errorDesc = GetErrorDescription(errorCode);

            DebugLogger.Debug($"ReadProcessMemory failed: Error {errorCode} ({errorDesc}) " +
                            $"at 0x{address.ToInt64():X}, trying to read {bytesToRead} bytes, " +
                            $"PID {processId}, consecutive errors: {_consecutiveErrors}");

            switch (errorCode)
            {
                case 183: // ERROR_ALREADY_EXISTS - This shouldn't happen with ReadProcessMemory, indicates handle issues
                    DebugLogger.Debug($"ERROR_ALREADY_EXISTS (183) - Unusual error for ReadProcessMemory, handle may be invalid");
                    ResetCaches();
                    _cachedHandleValid = false;
                    return false;

                case 299: // ERROR_PARTIAL_COPY
                    DebugLogger.Debug($"ERROR_PARTIAL_COPY (299) - Memory protection or access violation");
                    if (_consecutiveErrors >= MAX_CONSECUTIVE_ERRORS)
                    {
                        DebugLogger.Debug("Too many consecutive partial copy errors, invalidating process caches");
                        ResetCaches();
                        _cachedHandleValid = false;
                    }
                    return false;

                case 5: // ERROR_ACCESS_DENIED
                    DebugLogger.Debug($"Access denied (5) - Insufficient privileges or protected memory");
                    ResetCaches();
                    return false;

                case 6: // ERROR_INVALID_HANDLE
                    DebugLogger.Debug($"Invalid handle (6) - Process may have exited or handle corrupted");
                    ResetCaches();
                    _cachedHandleValid = false;
                    _cachedHasExited = true;
                    return false;

                case 87: // ERROR_INVALID_PARAMETER
                    DebugLogger.Debug($"Invalid parameter (87) - Check address and size parameters");
                    return false;

                default:
                    DebugLogger.Debug($"Unhandled error code {errorCode} ({errorDesc})");
                    return false;
            }
        }

        /// <summary>
        /// Gets human-readable description for Windows error codes
        /// </summary>
        private string GetErrorDescription(int errorCode)
        {
            switch (errorCode)
            {
                case 0: return "SUCCESS";
                case 5: return "ERROR_ACCESS_DENIED";
                case 6: return "ERROR_INVALID_HANDLE";
                case 87: return "ERROR_INVALID_PARAMETER";
                case 183: return "ERROR_ALREADY_EXISTS";
                case 299: return "ERROR_PARTIAL_COPY";
                case 1008: return "ERROR_SEEK";
                default: return $"UNKNOWN_ERROR_{errorCode}";
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
                catch (Exception ex)
                {
                    DebugLogger.Debug($"HasProcessExited exception: {ex.Message}");
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
        /// Reads memory into a provided buffer (throwing on failure).
        /// Use TryReadProcessMemory for non-throwing version.
        /// </summary>
        public void ReadProcessMemory(IntPtr baseAddress, uint bytesToRead, byte[] buffer, out int bytesRead)
        {
            EnsureProcessOpen();

            if (!TryReadProcessMemory(baseAddress, bytesToRead, buffer, out bytesRead))
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException($"ReadProcessMemory failed with error code {error} ({GetErrorDescription(error)}).");
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
            PROCESS_QUERY_INFORMATION = 0x0400,
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000  // Added for better compatibility
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