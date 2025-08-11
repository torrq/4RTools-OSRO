using System;
using System.Drawing;
using System.Threading;

namespace _ORTools.Utils
{
    public static class MouseHelper
    {
        #region Public Click Methods

        /// <summary>
        /// Performs a left mouse click at the current cursor position within the specified window
        /// Matches the original TryClickAtCurrentPosition implementation
        /// </summary>
        /// <param name="hWnd">Handle to the target window</param>
        /// <returns>True if the click was successful, false otherwise</returns>
        public static bool TryClickAtCurrentPosition(IntPtr hWnd)
        {
            try
            {
                if (!IsValidWindow(hWnd))
                {
                    DebugLogger.Warning("MouseHelper: Invalid window handle provided for click operation");
                    return false;
                }

                Win32Interop.SendMessage(hWnd, Win32Interop.WM_LBUTTONDOWN, (IntPtr)1, IntPtr.Zero);
                Thread.Sleep(25);
                Win32Interop.SendMessage(hWnd, Win32Interop.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"MouseHelper: Exception in TryClickAtCurrentPosition - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Performs a left mouse click at the center of the specified window
        /// Matches the original TryClickAtCenter implementation
        /// </summary>
        /// <param name="hWnd">Handle to the target window</param>
        /// <returns>True if the click was successful, false otherwise</returns>
        public static bool TryClickAtWindowCenter(IntPtr hWnd)
        {
            try
            {
                if (!IsValidWindow(hWnd))
                {
                    DebugLogger.Warning("MouseHelper: Invalid window handle provided for center click operation");
                    return false;
                }

                if (!Win32Interop.GetClientRect(hWnd, out Win32Interop.RECT clientRect))
                {
                    DebugLogger.Error("MouseHelper: Failed to get window client rectangle");
                    return false;
                }

                // Calculate the center of the client area
                int centerX = clientRect.Right / 2;
                int centerY = clientRect.Bottom / 2;

                // Convert client coordinates to screen coordinates
                Win32Interop.POINT centerPoint = new Win32Interop.POINT { X = centerX, Y = centerY };
                Win32Interop.ClientToScreen(hWnd, ref centerPoint);

                // Save current cursor position
                if (!Win32Interop.GetCursorPos(out Win32Interop.POINT originalPos))
                {
                    DebugLogger.Error("MouseHelper: Failed to get original cursor position");
                    return false;
                }

                // Move cursor to center, click, then restore position
                Win32Interop.SetCursorPos(centerPoint.X, centerPoint.Y);
                Thread.Sleep(25); // Keep the original timing that was working
                Win32Interop.mouse_event(Win32Interop.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(50); // Slightly longer delay between down and up
                Win32Interop.mouse_event(Win32Interop.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                // Restore original cursor position
                Win32Interop.SetCursorPos(originalPos.X, originalPos.Y);

                return true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"MouseHelper: Exception in TryClickAtWindowCenter - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Validates that the window handle is valid and visible
        /// </summary>
        /// <param name="hWnd">Window handle to validate</param>
        /// <returns>True if window is valid and visible</returns>
        private static bool IsValidWindow(IntPtr hWnd)
        {
            return hWnd != IntPtr.Zero && Win32Interop.IsWindow(hWnd) && Win32Interop.IsWindowVisible(hWnd);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets the center point of the specified window's client area
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <returns>Center point of the window, or Point.Empty if failed</returns>
        public static Point GetWindowCenter(IntPtr hWnd)
        {
            if (IsValidWindow(hWnd) && Win32Interop.GetClientRect(hWnd, out Win32Interop.RECT clientRect))
            {
                return new Point(clientRect.Right / 2, clientRect.Bottom / 2);
            }
            return Point.Empty;
        }

        /// <summary>
        /// Gets the current cursor position in screen coordinates
        /// </summary>
        /// <returns>Current cursor position, or Point.Empty if failed</returns>
        public static Point GetCursorPosition()
        {
            if (Win32Interop.GetCursorPos(out Win32Interop.POINT cursorPos))
            {
                return new Point(cursorPos.X, cursorPos.Y);
            }
            return Point.Empty;
        }

        /// <summary>
        /// Converts screen coordinates to client coordinates for the specified window
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="screenPoint">Point in screen coordinates</param>
        /// <returns>Point in client coordinates, or Point.Empty if failed</returns>
        public static Point ScreenToClientPoint(IntPtr hWnd, Point screenPoint)
        {
            if (IsValidWindow(hWnd) && Win32Interop.ScreenToClient(hWnd, ref screenPoint))
            {
                return screenPoint;
            }
            return Point.Empty;
        }

        #endregion
    }
}