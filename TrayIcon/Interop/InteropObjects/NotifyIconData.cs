using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace LenChon.Win32.TrayIcon.Interop.InteropObjects
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [NativeMarshalling(typeof(NotifyIconDataMashaller))]
    public struct NotifyIconData
    {
        public int StructureSize;
        public nint WindowHandle;
        public int Id;
        public int Flags;
        public int CallbackMessage;
        public nint IconHandle;
        public string? Tip;
        public int State;
        public int StateMask;
        public string? Info;
        public int TimeoutOrVersion;
        public string? InfoTitle;
        public int InfoFlags;
        public Guid GuidItem;
        public nint BalloonIcon;

        public Encoding? Encoding;
    }
}
