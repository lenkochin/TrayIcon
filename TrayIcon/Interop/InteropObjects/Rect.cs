using System.Runtime.InteropServices;

namespace LenChon.Win32.TrayIcon.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
