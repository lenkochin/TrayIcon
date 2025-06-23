using System.Windows;

namespace LenChon.Win32.TrayIcon.Events
{
    public delegate void TrayPopupOpenEventHandler(object sender, TrayPopupOpenEventArgs e);

    public class TrayPopupOpenEventArgs : RoutedEventArgs
    {
        public Direction PopupDirection { get; }

        public Size ElementSize { get; }

        public TrayPopupOpenEventArgs(Direction popupDirection, Size elementSize)
        {
            PopupDirection = popupDirection;
            ElementSize = elementSize;
        }

        public TrayPopupOpenEventArgs(Direction popupDirection, Size elementSize, RoutedEvent @event)
            :base(@event)
        {
            PopupDirection = popupDirection;
            ElementSize = elementSize;
        }
    }
}