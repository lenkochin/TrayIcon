using LenChon.Win32.TrayIcon.Interop.InteropObjects;
using LenChon.Win32.TrayIcon.Interop;

namespace LenChon.Win32.TrayIcon.Helpers
{
    internal static class MouseHelper
    {
        public static Point GetMousePositionInScreen()
        {
            Point p = new();

            UnsafeNativeMethods.GetCursorPos(ref p);

            return p;
        }
    }
}