using LenChon.Win32.TrayIcon.Interop.InteropObjects;
using System;
using System.Runtime.InteropServices;

namespace LenChon.Win32.TrayIcon.Interop
{
    internal partial class UnsafeNativeMethods
    {
        [LibraryImport(DllName.User32, EntryPoint = "DefWindowProcW")]
        internal static partial IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetForegroundWindow(IntPtr hWnd);

        [LibraryImport(DllName.Shell32, EntryPoint = "Shell_NotifyIcon", StringMarshalling = StringMarshalling.Utf8)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool ShellNotifyIcon(uint dwMessage, NotifyIconData data);

        [LibraryImport(DllName.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GetCursorPos(ref Point p);

        [LibraryImport(DllName.User32)]
        internal static partial IntPtr MonitorFromWindow(IntPtr hWnd, int dwFlags);

        [LibraryImport(DllName.Shcore)]
        internal static partial int GetDpiForMonitor(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY);

        [LibraryImport(DllName.Shell32)]
        internal static partial UIntPtr SHAppBarMessage(uint dwMessage, ref AppBarData pData);
    }
}