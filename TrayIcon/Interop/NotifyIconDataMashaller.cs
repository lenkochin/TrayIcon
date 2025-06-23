using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using LenChon.Win32.TrayIcon.Interop.InteropObjects;

namespace LenChon.Win32.TrayIcon.Interop
{
    [CustomMarshaller(typeof(NotifyIconData), MarshalMode.Default, typeof(NotifyIconDataMashaller))]
    internal unsafe static class NotifyIconDataMashaller
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct NotifyIconDataUnmanaged
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uID;
            public int uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            public fixed byte szTip[128];
            public int dwState;
            public int dwStateMask;
            public fixed byte szInfo[256];
            public int uTimeoutOrVersion;
            public fixed byte szInfoTitle[64];
            public int dwInfoFlags;
            public Guid guidItem;
            public IntPtr hBalloonIcon;
        }

        public static NotifyIconDataUnmanaged ConvertToUnmanaged(NotifyIconData managed)
        {
            NotifyIconDataUnmanaged unmanaged = new()
            {
                cbSize = Marshal.SizeOf<NotifyIconDataUnmanaged>(),
                hWnd = managed.WindowHandle,
                uID = managed.Id,
                uFlags = managed.Flags,
                uCallbackMessage = managed.CallbackMessage,
                hIcon = managed.IconHandle,
                dwState = managed.State,
                dwStateMask = managed.StateMask,
                uTimeoutOrVersion = managed.TimeoutOrVersion,
                dwInfoFlags = managed.InfoFlags,
                guidItem = managed.GuidItem,
                hBalloonIcon = managed.BalloonIcon
            };

            CopyString(managed.Tip, unmanaged.szTip, 128, managed.Encoding);
            CopyString(managed.Info, unmanaged.szInfo, 256, managed.Encoding);
            CopyString(managed.InfoTitle, unmanaged.szInfoTitle, 64, managed.Encoding);

            return unmanaged;
        }

        private static void CopyString(string? source, byte* dest, int size, Encoding? encoding)
        {
            if (source?.Length is not > 0)
            {
                return;
            }

            var bytes = (encoding ?? Encoding.Default).GetBytes(source);
            var src = new ReadOnlySpan<byte>(bytes);
            var destSpan = new Span<byte>(dest, size);

            src.CopyTo(destSpan);
        }
    }
}