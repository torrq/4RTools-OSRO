namespace _ORTools.Utils
{
    internal static class Constants
    {
        // Window Messages
        public const int WM_HOTKEY_MSG_ID = 0x0312;
        public const int WM_KEYUP_MSG_ID = 0x0101;
        public const int WM_KEYDOWN_MSG_ID = 0x0100;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;

        // Hook Types
        public const int WH_KEYBOARD_LL = 13;

        // Mouse Event Flags
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        // Keyboard Event Flags
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;

        // Virtual Key Codes
        public const int VK_SHIFT = 0x10;
        public const int VK_LMENU = 0xA4;

        // Window Show States
        public const int SW_RESTORE = 9;

        // Application-specific constants
        public const int MINIMUM_HP_TO_RECOVER = 20;
        public const int MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK = 1;
        public const int MAX_BUFF_LIST_INDEX_SIZE = 100;
    }
}