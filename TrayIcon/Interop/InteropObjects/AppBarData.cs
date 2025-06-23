using System;
using System.Runtime.InteropServices;

namespace LenChon.Win32.TrayIcon.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AppBarData
    {
        public uint StructureSize;
        public IntPtr WindowHandle;
        public uint CallbackMessage;
        /// <summary>
        /// Value should be one of the following: ABM_BOTTOM/ABM_LEFT/ABM_RIGHT/ABM_TOP.
        /// </summary>
        public uint Edge;
        public Rect rect;
        public IntPtr LParam;
    }
}
