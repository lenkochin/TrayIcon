using System.Windows;
using System.Windows.Input;

namespace LenChon.Win32.TrayIcon.Events
{
    public delegate void TrayMouseEventHandler(object sender, TrayMouseEventArgs e);

    public class TrayMouseEventArgs : RoutedEventArgs
    {
        public MouseButton Button { get; internal set; } = MouseButton.Left;

        internal TrayMouseEventArgs(MouseButton clickedButton)
        {
            Button = clickedButton;
        }

        public TrayMouseEventArgs() { }
    }
}