namespace LenChon.Win32.TrayIcon.Interop.IntermidiateObjects
{
    internal readonly struct Dpi(uint x, uint y)
    {
        public uint X => x;

        public uint Y => y;
    }
}