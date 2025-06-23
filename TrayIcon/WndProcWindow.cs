using LenChon.Win32.TrayIcon.Interop;
using System;
using System.Windows.Interop;

namespace LenChon.Win32.TrayIcon
{
    internal class WndProcWindow : IWin32Window, IDisposable
    {
        private HwndSource _source;

        public event HwndSourceHook? WndProc;
        public IntPtr Handle { get; }

        public WndProcWindow()
        {
            _source = new(0, 0, 0, 0, 0, 0, 0, "blankWin", IntPtr.Zero);
            _source.AddHook(WndProcForward);

            Handle = _source.Handle;
        }

        private IntPtr WndProcForward(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return WndProc?.Invoke(hWnd, Msg, wParam, lParam, ref handled) ?? UnsafeNativeMethods.DefWindowProc(hWnd, Msg, wParam, lParam);
        }

        public void Dispose()
        {
            _source?.Dispose();
        }
    }
}