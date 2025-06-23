using LenChon.Win32.TrayIcon.Interop;
using LenChon.Win32.TrayIcon.Interop.IntermidiateObjects;
using LenChon.Win32.TrayIcon.Interop.InteropObjects;
using System;
using System.Runtime.InteropServices;

namespace LenChon.Win32.TrayIcon.Helpers
{
    internal static class SystemHelper
    {
        public static OperatingSystemVersion GetOSVersionEnum()
        {
            var osv = Environment.OSVersion;
            var osve = osv.Platform switch
            {
                // Platform check.
                PlatformID.Win32NT => osv.Version.Major switch
                {
                    // The '6' represents Windows NT 6.x.
                    6 => osv.Version.Minor switch
                    {
                        1 => OperatingSystemVersion.Windows7,
                        2 => OperatingSystemVersion.Windows8,
                        3 => OperatingSystemVersion.Windows8_1,
                        _ => OperatingSystemVersion.Unknown
                    },
                    // The '10' represents Windows NT 10, which covers Windows 10 and Windows 11.
                    10 => osv.Version.Build switch
                    {
                        10240 => OperatingSystemVersion.Windows10_1507,
                        10586 => OperatingSystemVersion.Windows10_1511,
                        14393 => OperatingSystemVersion.Windows10_1607,
                        15063 => OperatingSystemVersion.Windows10_1703,
                        16299 => OperatingSystemVersion.Windows10_1709,
                        17134 => OperatingSystemVersion.Windows10_1803,
                        17763 => OperatingSystemVersion.Windows10_1809,
                        18362 => OperatingSystemVersion.Windows10_1903,
                        18363 => OperatingSystemVersion.Windows10_1909,
                        19041 => OperatingSystemVersion.Windows10_2004,
                        19042 => OperatingSystemVersion.Windows10_20H2,
                        19043 => OperatingSystemVersion.Windows10_21H1,
                        19044 => OperatingSystemVersion.Windows10_21H2,
                        19045 => OperatingSystemVersion.Windows10_22H2,
                        22000 => OperatingSystemVersion.Windows11_21H2,
                        22621 => OperatingSystemVersion.Windows11_22H2,
                        22631 => OperatingSystemVersion.Windows11_23H2,
                        26100 => OperatingSystemVersion.Windows11_24H2,
                        _ => OperatingSystemVersion.FutureRelease,
                    },
                    _ => OperatingSystemVersion.Unknown
                },
                _ => OperatingSystemVersion.Unknown
            };

            return osve;
        }

        public static Edge GetTaskBarEdge()
        {
            var appBarData = new AppBarData()
            {
                StructureSize = (uint)Marshal.SizeOf<AppBarData>()
            };

            UnsafeNativeMethods.SHAppBarMessage(ConstantPredefinedValues.ABM_GETTASKBARPOS, ref appBarData);

            return appBarData.Edge switch
            {
                ConstantPredefinedValues.ABE_TOP => Edge.Top,
                ConstantPredefinedValues.ABE_RIGHT => Edge.Right,
                ConstantPredefinedValues.ABE_BOTTOM => Edge.Bottom,
                ConstantPredefinedValues.ABE_LEFT => Edge.Left,
                _ => Edge.Unknown
            };
        }
    }
}
