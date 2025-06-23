namespace LenChon.Win32.TrayIcon.Interop
{
    internal static class ConstantPredefinedValues
    {
        //Window style
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_SIZEBOX = 0x00040000;
        public const int WS_BORDER = 0x00800000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_MAXIMIZEBOX = 0x10000;
        public const int WS_MINIMIZEBOX = 0x20000;
        public const int WS_SYSMENU = 0x00080000;

        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_COMPOSITED = 0x02000000;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_LAYOUTRTL = 0x00400000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_WINDOWEDGE = 0x00000100;

        public const int GWL_EXSTYLE = -20;
        public const int GWL_STYLE = -16;

        public const int MONITOR_DEFAULTTIONNEAREST = 2;
        public const int MONITOR_DEFAULTTIONPRIMARY = 1;
        public const int EDD_GET_DEVICE_INTERFACE_NAME = 0x1;
        public const int EDS_ROTATEDMODE = 0x4;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int SM_CMONITORS = 80;

        public const int SW_HIDE = 0;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOWNA = 8;

        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_USER = 1024;
        public const int WM_TRAYMOUSEMESSAGE = WM_USER + 1024;
        //public static readonly int WM_TASKBARCREATED = (int)NativeMethods.RegisterWindowMessage("TaskbarCreated");
        public const int WM_TASKBARCREATED = 49410;

        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;
        public const int SPI_SETDESKWALLPAPER = 0x0014;
        public const int SPI_GETDESKWALLPAPER = 0x0073;

        public const int HT_CAPTION = 2;

        public const int BIF_RETURNONLYFSDIRS = 0x1;
        public const int BIF_DONTGOBELOWDOMAIN = 0x2;
        public const int BIF_NEWDIALOGSTYLE = 0x40;
        public const int BIF_NONEWFOLDERBUTTON = 0x200;
        public const int BFFM_SELCHANGED = 2;
        public const int BFFM_ENABLEOK = 0x0465;

        public const int ABM_GETTASKBARPOS = 0x5;
        public const int ABE_LEFT = 0;
        public const int ABE_TOP = 1;
        public const int ABE_RIGHT = 2;
        public const int ABE_BOTTOM = 3;

        public const int NIM_ADD = 0x0;
        public const int NIM_MODIFY = 0x1;
        public const int NIM_DELETE = 0x2;
        public const int NIM_SETFOCUS = 0x3;
        public const int NIM_SETVERSION = 0x4;

        public const int NIF_MESSAGE = 0x1;
        public const int NIF_ICON = 0x2;
        public const int NIF_TIP = 0x4;
        public const int NIF_INFO = 0x10;

        public const int NIIF_NONE = 0x0;
        public const int NIIF_INFO = 0x1;
        public const int NIIF_WARNING = 0x2;
        public const int NIIF_ERROR = 0x3;
        public const int NIIF_USER = 0x4;
        public const int NIIF_NOSOUND = 0x10;
        public const int NIIF_LARGE_ICON = 0x20;

        public const int STATE_SYSTEM_INVISIBLE = 0x00008000;

        public const int VK_LBUTTON = 0x01;
        public const int VK_RBUTTON = 0x02;
        public const int VK_MBUTTON = 0x04;

        private const uint DPI_AWARENESS_CONTEXT_32 = uint.MaxValue;
        public const uint DPI_AWARENESS_CONTEXT_UNAWARE_32 = DPI_AWARENESS_CONTEXT_32 - 1;
        public const uint DPI_AWARENESS_CONTEXT_SYSTEM_AWARE_32 = DPI_AWARENESS_CONTEXT_32 - 2;
        public const uint DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_32 = DPI_AWARENESS_CONTEXT_32 - 3;
        public const uint DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2_32 = DPI_AWARENESS_CONTEXT_32 - 4;
        public const uint DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED_32 = DPI_AWARENESS_CONTEXT_32 - 5;
        // 64bit
        private const ulong DPI_AWARENESS_CONTEXT_64 = ulong.MaxValue;
        public const ulong DPI_AWARENESS_CONTEXT_UNAWARE_64 = DPI_AWARENESS_CONTEXT_64 - 1;
        public const ulong DPI_AWARENESS_CONTEXT_SYSTEM_AWARE_64 = DPI_AWARENESS_CONTEXT_64 - 2;
        public const ulong DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_64 = DPI_AWARENESS_CONTEXT_64 - 3;
        public const ulong DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2_64 = DPI_AWARENESS_CONTEXT_64 - 4;
        public const ulong DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED_64 = DPI_AWARENESS_CONTEXT_64 - 5;
    }
}
