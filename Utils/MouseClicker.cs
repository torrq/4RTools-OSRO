using _4RTools.Utils;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class MouseClicker
{
    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public static void ClickMouse()
    {
        var pos = Cursor.Position;
        mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)pos.X, (uint)pos.Y, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, (uint)pos.X, (uint)pos.Y, 0, 0);

        DebugLogger.Debug("Mouse clicked at position: " + pos);
    }
}
