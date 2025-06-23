using System.Windows.Input;

namespace LenChon.Win32.TrayIcon.Events
{
    public delegate void TrayMouseClickEventHandler(object sender, TrayMouseClickEventArgs e);

    public sealed class TrayMouseClickEventArgs : TrayMouseEventArgs
    {
        public bool IsDoubleClick { get; internal set; } = false;

        internal TrayMouseClickEventArgs(bool isDoubleClick, MouseButton clickedButton)
            :base(clickedButton)
        {
            IsDoubleClick = isDoubleClick;
        }

        public TrayMouseClickEventArgs() { }
    }
}