using LenChon.Win32.TrayIcon.Events;
using LenChon.Win32.TrayIcon.Helpers;
using LenChon.Win32.TrayIcon.Interop;
using LenChon.Win32.TrayIcon.Interop.IntermidiateObjects;
using LenChon.Win32.TrayIcon.Interop.InteropObjects;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using I = LenChon.Win32.TrayIcon.Interop.InteropObjects;

namespace LenChon.Win32.TrayIcon
{
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(Popup))]
    public sealed class TrayNotifyIcon : ContentControl, IDisposable
    {
        private static readonly object _lockObj = new();
        private static WndProcWindow? _blankWin;
        private static bool _isCreated = false;
        private static bool _isInDesginMode = false;

        private static System.Drawing.Icon? _previousIcon, _currentIcon;
        private bool _iconAdded = false;
        private bool _leftDoubleClick = false;
        private bool _rightDoubleClick = false;

        private Popup? _contentHost;

        #region Dependency Properties

        public static readonly DependencyProperty DoubleClickDelayProperty =
                    DependencyProperty.Register(nameof(DoubleClickDelay), typeof(uint?), typeof(TrayNotifyIcon),
                        new PropertyMetadata(null));

        public static readonly DependencyProperty IconPathProperty =
                    DependencyProperty.Register(nameof(IconPath), typeof(string), typeof(TrayNotifyIcon),
                        new PropertyMetadata(null, OnIconPathChanged));

        public static readonly DependencyProperty TrayIconToolTipProperty =
                    DependencyProperty.Register(nameof(TrayIconToolTip), typeof(string), typeof(TrayNotifyIcon),
                        new PropertyMetadata(null, OnTrayToolTipChanged));

        public static readonly DependencyProperty TrayPopupActivationMethodProperty =
                    DependencyProperty.Register(nameof(TrayPopupActivationMethod), typeof(TrayPopupActivationMethods), typeof(TrayNotifyIcon),
                        new PropertyMetadata(TrayPopupActivationMethods.RightClick, OnTrayPopupActivationMethodChanged));

        public static readonly DependencyProperty TrayPopupClosingAnimationProperty =
                    DependencyProperty.Register(nameof(TrayPopupClosingAnimation), typeof(Storyboard), typeof(TrayNotifyIcon),
                        new PropertyMetadata(null, OnTrayPopupClosingAnimationChanged));

        public static readonly DependencyProperty TrayPopupOpeningAnimationProperty =
                    DependencyProperty.Register(nameof(TrayPopupOpeningAnimation), typeof(Storyboard), typeof(TrayNotifyIcon),
                        new PropertyMetadata(null, OnTrayPopupOpeningAnimationChanged));

        public uint? DoubleClickDelay
        {
            get => (uint?)GetValue(DoubleClickDelayProperty);
            set => SetValue(DoubleClickDelayProperty, value);
        }

        public string IconPath
        {
            get => (string)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }

        public string TrayIconToolTip
        {
            get => (string)GetValue(TrayIconToolTipProperty);
            set => SetValue(TrayIconToolTipProperty, value);
        }

        /// <summary>
        /// Indicates how to activate the popup control.
        /// </summary>
        public TrayPopupActivationMethods TrayPopupActivationMethod
        {
            get => (TrayPopupActivationMethods)GetValue(TrayPopupActivationMethodProperty);
            set => SetValue(TrayPopupActivationMethodProperty, value);
        }

        public Storyboard TrayPopupClosingAnimation
        {
            get => (Storyboard)GetValue(TrayPopupClosingAnimationProperty);
            set => SetValue(TrayPopupClosingAnimationProperty, value);
        }

        public Storyboard TrayPopupOpeningAnimation
        {
            get => (Storyboard)GetValue(TrayPopupOpeningAnimationProperty);
            set => SetValue(TrayPopupOpeningAnimationProperty, value);
        }

        #endregion Dependency Properties

        //-------------------------------------------------------------------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------      Route Event Field      -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //-------------------------------------------------------------------------------------

        #region RoutedEvent

        public static readonly RoutedEvent PreviewTrayPopupClosedEvent =
            EventManager.RegisterRoutedEvent(nameof(PreviewTrayPopupClosed), RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent PreviewTrayPopupOpenedEvent =
            EventManager.RegisterRoutedEvent(nameof(PreviewTrayPopupOpened), RoutingStrategy.Tunnel, typeof(TrayPopupOpenEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayMouseButtonClickEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayMouseButtonClick), RoutingStrategy.Bubble, typeof(TrayMouseClickEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayMouseButtonDownEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayMouseButtonDown), RoutingStrategy.Bubble, typeof(TrayMouseEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayMouseButtonUpEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayMouseButtonUp), RoutingStrategy.Bubble, typeof(TrayMouseEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayPopupActivationMethodChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayPopupActivationMethodChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayPopupClosedEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayPopupClosed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TrayNotifyIcon));

        public static readonly RoutedEvent TrayPopupOpenedEvent =
            EventManager.RegisterRoutedEvent(nameof(TrayPopupOpened), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TrayNotifyIcon));

        public event RoutedEventHandler PreviewTrayPopupClosed
        {
            add { AddHandler(PreviewTrayPopupClosedEvent, value); }
            remove { RemoveHandler(PreviewTrayPopupClosedEvent, value); }
        }

        public event TrayPopupOpenEventHandler PreviewTrayPopupOpened
        {
            add { AddHandler(PreviewTrayPopupOpenedEvent, value); }
            remove { RemoveHandler(PreviewTrayPopupOpenedEvent, value); }
        }

        public event TrayMouseClickEventHandler TrayMouseButtonClick
        {
            add { AddHandler(TrayMouseButtonClickEvent, value); }
            remove { RemoveHandler(TrayMouseButtonClickEvent, value); }
        }

        public event TrayMouseEventHandler TrayMouseButtonDown
        {
            add { AddHandler(TrayMouseButtonDownEvent, value); }
            remove { RemoveHandler(TrayMouseButtonDownEvent, value); }
        }

        public event TrayMouseEventHandler TrayMouseButtonUp
        {
            add { AddHandler(TrayMouseButtonUpEvent, value); }
            remove { RemoveHandler(TrayMouseButtonUpEvent, value); }
        }

        public event RoutedEventHandler TrayPopupActivationMethodChanged
        {
            add { AddHandler(TrayPopupActivationMethodChangedEvent, value); }
            remove { RemoveHandler(TrayPopupActivationMethodChangedEvent, value); }
        }

        public event RoutedEventHandler TrayPopupClosed
        {
            add { AddHandler(TrayPopupClosedEvent, value); }
            remove { RemoveHandler(TrayPopupClosedEvent, value); }
        }

        public event RoutedEventHandler TrayPopupOpened
        {
            add { AddHandler(TrayPopupOpenedEvent, value); }
            remove { RemoveHandler(TrayPopupOpenedEvent, value); }
        }

        #endregion RoutedEvent

        //-------------------------------------------------------------------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------     Public Methods Field    -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //-------------------------------------------------------------------------------------

        #region Public Methods

        static TrayNotifyIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TrayNotifyIcon), new FrameworkPropertyMetadata(typeof(TrayNotifyIcon)));
        }

        public TrayNotifyIcon()
        {
            TryCreate();
            InitializeStuff();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentHost = GetTemplateChild("PART_ContentHost") as Popup;

            ThrownIfContentHostNotExist();
        }

        public void Dispose()
        {
            Visibility = Visibility.Hidden;

            _blankWin?.Dispose();
            _currentIcon?.Dispose();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Construct a NOTIFYICONDATA that used by Shell_NotifyIcon.
        /// </summary>
        /// <param name="d"><see cref="TrayNotifyIcon"/></param>
        /// <returns></returns>
        private static NotifyIconData ConstructData(DependencyObject d)
        {
            var hIcon = StringToIconHandle(d);

            if (hIcon.Handle == IntPtr.Zero || _blankWin is null)
            {
                return default;
            }

            var data = new NotifyIconData()
            {
                IconHandle = hIcon.Handle,
                WindowHandle = _blankWin.Handle,
                Id = 0,
                Flags = ConstantPredefinedValues.NIF_MESSAGE | ConstantPredefinedValues.NIF_ICON | ConstantPredefinedValues.NIF_TIP,
                CallbackMessage = ConstantPredefinedValues.WM_TRAYMOUSEMESSAGE,
                Tip = (string)d.GetValue(TrayIconToolTipProperty),
                Encoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage)
            };

            return data;
        }

        private static System.Drawing.Icon StringToIconHandle(DependencyObject d)
                    => StringToIconHandle((string)d.GetValue(IconPathProperty));

        private static System.Drawing.Icon StringToIconHandle(string iconPath)
        {
            var uri = new Uri(iconPath, UriKind.RelativeOrAbsolute);
            Stream stream;

            if (!uri.IsAbsoluteUri)
            {
                using (stream = Application.GetResourceStream(uri).Stream)
                {
                    _previousIcon = _currentIcon;
                    System.Drawing.Icon icon = new(stream);
                    _currentIcon = icon;
                    return icon;
                }
            }
            else
            {
                using (stream = new FileStream(iconPath, FileMode.Open))
                {
                    _previousIcon = _currentIcon;
                    System.Drawing.Icon icon = new(stream);
                    _currentIcon = icon;
                    return icon;
                }
            }
        }

        /// <summary>
        /// Wrapper of Shell_NotifyIcon.
        /// </summary>
        /// <param name="action">Action of setting notify icon.</param>
        /// <param name="d">Icon provider.</param>
        private static void UpdateIcon(NotifyIconActions action, DependencyObject d)
        {
            if (_isInDesginMode || d is not TrayNotifyIcon trayIcon)
            {
                return;
            }

            lock (_lockObj)
            {
                switch (action)
                {
                    case NotifyIconActions.Add:
                        if (trayIcon._iconAdded)
                        {
                            return;
                        }

                        UnsafeNativeMethods.ShellNotifyIcon(ConstantPredefinedValues.NIM_ADD, ConstructData(d));
                        trayIcon._iconAdded = true;
                        break;

                    case NotifyIconActions.Modify:
                        UnsafeNativeMethods.ShellNotifyIcon(ConstantPredefinedValues.NIM_MODIFY, ConstructData(d));
                        break;

                    case NotifyIconActions.Delete:
                        if (!trayIcon._iconAdded)
                        {
                            return;
                        }

                        UnsafeNativeMethods.ShellNotifyIcon(ConstantPredefinedValues.NIM_DELETE, ConstructData(d));
                        trayIcon._iconAdded = false;
                        break;

                    default:
                        break;
                }
            }

            _previousIcon?.Dispose();
        }

        private void TryCreate()
        {
            _isInDesginMode = DesignerProperties.GetIsInDesignMode(this);

            if (_isInDesginMode)
            {
                return;
            }

            lock (_lockObj)
            {
                if (_isCreated)
                {
                    throw new InvalidOperationException("Cannot create more than one instance.");
                }

                _isCreated = true;
            }
        }

        private void RaisePopupOpenEvent(bool isPreview, double? dpiScale = null)
        {
            dpiScale ??= VisualTreeHelper.GetDpi(this).DpiScaleX;

            RaiseEvent(new TrayPopupOpenEventArgs(GetPopupDirection(), GetHostSize(dpiScale.Value), isPreview ? PreviewTrayPopupOpenedEvent : TrayPopupOpenedEvent));
        }

        private void RaisePopupCloseEvent(bool isPreview)
        {
            RaiseEvent(new RoutedEventArgs(isPreview ? PreviewTrayPopupOpenedEvent : TrayPopupOpenedEvent));
        }

        private void InitializeStuff()
        {
            //Override metadata for receiving the notification when the value has been changed.
            VisibilityProperty.OverrideMetadata(GetType(), new PropertyMetadata(OnVisibleChanged));

            _blankWin = new WndProcWindow();
            _blankWin.WndProc += WndProcCallback;

            //The icon will be "damaged" after system resolution changed.
            //A refresh can fix that.
            SystemEvents.DisplaySettingsChanged += async (s, e) =>
            {
                await Task.Delay(600);
                UpdateIcon(NotifyIconActions.Modify, this);
            };
        }

        /// <summary>
        /// Check if popup should be opened.
        /// </summary>
        /// <param name="e">EventArgs that contains necessary information. Received from TrayMouseButtonClick event.</param>
        /// <returns>Is popup activated.</returns>
        private bool ActivatePopupFilter(TrayMouseClickEventArgs e)
        {
            bool _isInvoked = false;

            switch (TrayPopupActivationMethod)
            {
                case TrayPopupActivationMethods.LeftClick:
                    if (e.Button == MouseButton.Left && !e.IsDoubleClick)
                    {
                        _isInvoked = true;
                        DoActivatePopup();
                    }
                    break;

                case TrayPopupActivationMethods.LeftDoubleClick:
                    if (e.Button == MouseButton.Left && e.IsDoubleClick)
                    {
                        _isInvoked = true;
                        DoActivatePopup();
                    }
                    break;

                case TrayPopupActivationMethods.RightClick:
                    if (e.Button == MouseButton.Right && !e.IsDoubleClick)
                    {
                        _isInvoked = true;
                        DoActivatePopup();
                    }
                    break;

                case TrayPopupActivationMethods.RightDoubleClick:
                    if (e.Button == MouseButton.Right && e.IsDoubleClick)
                    {
                        _isInvoked = true;
                        DoActivatePopup();
                    }
                    break;
            }

            return _isInvoked;
        }

        [MemberNotNull(nameof(_contentHost))]
        private void ThrownIfContentHostNotExist()
        {
            if (_contentHost is null)
            {
                throw new InvalidOperationException("A popup control named 'PART_ContentHost' is a must.");
            }
        }

        private Size GetHostSize(double dpiScale)
        {
            ThrownIfContentHostNotExist();

            if (_contentHost.ActualWidth > 0 && _contentHost.ActualHeight > 0)
            {
                return MakeResult(_contentHost.ActualWidth * dpiScale, _contentHost.ActualHeight * dpiScale);
            }

            if (_contentHost.DesiredSize.Width is 0 || _contentHost.DesiredSize.Height is 0)
            {
                _contentHost.Measure(SystemParameters.WorkArea.Size);
            }
            if (!(_contentHost.DesiredSize.Width is 0 || _contentHost.DesiredSize.Height is 0))
            {
                return MakeResult(_contentHost.DesiredSize.Width, _contentHost.DesiredSize.Height);
            }

            if (_contentHost.Child is FrameworkElement fe && fe.ActualWidth > 0 && fe.ActualHeight > 0)
            {
                return MakeResult(fe.ActualWidth, fe.ActualHeight);
            }

            if (_contentHost.Child?.DesiredSize is Size size)
            {
                if (size.Width is 0 || size.Height is 0)
                {
                    _contentHost.Child.Measure(SystemParameters.WorkArea.Size);
                }
                if (!(_contentHost.Child.DesiredSize.Width is 0 || _contentHost.Child.DesiredSize.Height is 0))
                {
                    return MakeResult(_contentHost.Child.DesiredSize.Width, _contentHost.Child.DesiredSize.Height);
                }
            }

            return Size.Empty;

            Size MakeResult(double _width, double _height) => new(_width * dpiScale, _height * dpiScale);
        }

        private Direction GetPopupDirection()
        {
            ThrownIfContentHostNotExist();

            var dpiScale = VisualTreeHelper.GetDpi(this).DpiScaleX;
            var point = MouseHelper.GetMousePositionInScreen();
            var screenRect = SystemParameters.WorkArea;
            var popupSize = GetHostSize(dpiScale);
            var taskbarEdge = SystemHelper.GetTaskBarEdge();

            return taskbarEdge switch
            {
                Edge.Top when popupSize.Width + point.X > screenRect.Right * dpiScale => Direction.LeftDown,
                Edge.Top => Direction.RightDown,
                Edge.Left when popupSize.Height + point.Y > screenRect.Bottom * dpiScale => Direction.RightUp,
                Edge.Left => Direction.RightDown,
                Edge.Right when popupSize.Height + point.Y > screenRect.Bottom * dpiScale => Direction.LeftUp,
                Edge.Right => Direction.LeftDown,
                Edge.Bottom when popupSize.Width + point.X > screenRect.Right * dpiScale => Direction.LeftUp,
                Edge.Bottom => Direction.RightUp,
                _ => Direction.Unknown
            };
        }

        /// <summary>
        /// A event wrapper to <see cref="DoDeactivatePopup"/>.
        /// </summary>
        private void DeactivatePopupByStoryboardWrapper(object? sender, EventArgs e)
        {
            ThrownIfContentHostNotExist();

            RaisePopupCloseEvent(false);
            _contentHost.IsOpen = false;
        }

        private void ActivatePopupByStoryboardWrapper(object? sender, EventArgs e) => RaisePopupOpenEvent(false);

        /// <summary>
        /// Settting parameter for popup and let it open.
        /// </summary>
        private void DoActivatePopup()
        {
            ThrownIfContentHostNotExist();

            I.Point mousePoint = MouseHelper.GetMousePositionInScreen();
            var dpi = VisualTreeHelper.GetDpi(this);

            _contentHost.HorizontalOffset = mousePoint.X / dpi.DpiScaleX;
            _contentHost.VerticalOffset = mousePoint.Y / dpi.DpiScaleX;

            RaisePopupOpenEvent(true, dpi.DpiScaleX);

            if (!_contentHost.IsOpen)
            {
                _contentHost.IsOpen = true;

                if (TrayPopupOpeningAnimation is not null)
                {
                    TrayPopupOpeningAnimation.Begin();
                }
                else
                {
                    RaisePopupOpenEvent(false, dpi.DpiScaleX);
                }
            }
        }

        private void DoDeactivatePopup()
        {
            ThrownIfContentHostNotExist();

            if (_contentHost.IsOpen == true)
            {
                RaiseEvent(new RoutedEventArgs(PreviewTrayPopupClosedEvent));

                if (TrayPopupClosingAnimation != null)
                {
                    TrayPopupClosingAnimation.Begin();
                }
                else
                {
                    _contentHost.IsOpen = false;
                }
            }
        }

        #endregion Private Methods

        #region Callback Methods

        private static void OnIconPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isInDesginMode)
            {
                return;
            }

            UpdateIcon(NotifyIconActions.Add, d);
        }

        private static void OnTrayPopupActivationMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isInDesginMode || d is not TrayNotifyIcon self)
            {
                return;
            }

            var arg = new RoutedEventArgs(TrayPopupActivationMethodChangedEvent);
            self.RaiseEvent(arg);
        }

        private static void OnTrayPopupOpeningAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isInDesginMode || e.NewValue is not Storyboard openingSb || d is not TrayNotifyIcon self)
            {
                return;
            }

            if (e.OldValue is Storyboard oldSb)
            {
                oldSb.Completed -= self.ActivatePopupByStoryboardWrapper;
            }

            openingSb.Completed += self.ActivatePopupByStoryboardWrapper;
        }

        private static void OnTrayPopupClosingAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isInDesginMode || e.NewValue is not Storyboard closingSb || d is not TrayNotifyIcon self)
            {
                return;
            }

            if (e.OldValue is Storyboard oldSb)
            {
                oldSb.Completed -= self.DeactivatePopupByStoryboardWrapper;
            }

            closingSb.Completed += self.DeactivatePopupByStoryboardWrapper;
        }

        private static void OnTrayToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue?.ToString()?.Length > 64)
            {
                throw new InvalidDataException("The length of TrayIconToolTip should be less than 64.");
            }

            UpdateIcon(NotifyIconActions.Modify, d);
        }

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var vis = (Visibility)e.NewValue;

            if (vis == Visibility.Collapsed || vis == Visibility.Hidden)
            {
                UpdateIcon(NotifyIconActions.Delete, d);
            }
            else if (vis == Visibility.Visible)
            {
                UpdateIcon(NotifyIconActions.Add, d);
            }
        }

        private IntPtr WndProcCallback(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_isInDesginMode)
            {
                return IntPtr.Zero;
            }

            IntPtr result = IntPtr.Zero;

            switch (Msg)
            {
                case ConstantPredefinedValues.WM_TRAYMOUSEMESSAGE:
                    switch (lParam.ToInt32())
                    {
                        case ConstantPredefinedValues.WM_LBUTTONDOWN:
                            MouseLeftButtonDownHandler(1);
                            break;

                        case ConstantPredefinedValues.WM_LBUTTONUP:
                            MouseLeftButtonUpHandler();
                            break;

                        case ConstantPredefinedValues.WM_LBUTTONDBLCLK:
                            //Known that WM_LBUTTONDBLCLK will be received after WM_LBUTTONUP.
                            //Sequence is WM_LBUTTONDOWN
                            //            WM_LBUTTONUP
                            //            WM_LBUTTONDBLCLK
                            //            WM_LBUTTONUP
                            MouseLeftButtonDownHandler(2);
                            break;

                        case ConstantPredefinedValues.WM_RBUTTONDOWN:
                            MouseRightButtonDownWrapper(1);
                            break;

                        case ConstantPredefinedValues.WM_RBUTTONUP:
                            MouseRightButtonUpWrapper();
                            break;

                        case ConstantPredefinedValues.WM_RBUTTONDBLCLK:
                            MouseRightButtonDownWrapper(2);
                            break;
                    }
                    break;

                case ConstantPredefinedValues.WM_KILLFOCUS:
                    DoDeactivatePopup();
                    break;

                case ConstantPredefinedValues.WM_TASKBARCREATED:
                    _iconAdded = false;
                    UpdateIcon(NotifyIconActions.Add, this);
                    break;

                default:
                    result = UnsafeNativeMethods.DefWindowProc(hWnd, Msg, wParam, lParam);
                    break;
            }

            return result;
        }

        #endregion Callback Methods

        //-------------------------------------------------------------------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------        Event Handlers       -----------------------------
        //---------------------------                             -----------------------------
        //---------------------------                             -----------------------------
        //-------------------------------------------------------------------------------------

        #region Event Handlers

        /// <summary>
        /// Doing some works to ensure a event can be fired correctly.
        /// </summary>
        /// <param name="clicks">Clicks count.</param>
        private void MouseLeftButtonDownHandler(int clicks)
        {
            if (clicks == 2)
            {
                _leftDoubleClick = true;
            }

            OnMouseLeftButtonDown(new TrayMouseEventArgs(MouseButton.Left) { RoutedEvent = TrayMouseButtonDownEvent });
        }

        private void MouseLeftButtonUpHandler()
        {
            OnMouseLeftButtonUp(new TrayMouseEventArgs(MouseButton.Left) { RoutedEvent = TrayMouseButtonUpEvent });
        }

        private void MouseRightButtonDownWrapper(int clicks)
        {
            if (clicks == 2)
            {
                _rightDoubleClick = true;
            }

            OnMouseRightButtonDown(new TrayMouseEventArgs(MouseButton.Right) { RoutedEvent = TrayMouseButtonDownEvent });
        }

        private void MouseRightButtonUpWrapper()
        {
            OnMouseRightButtonUp(new TrayMouseEventArgs(MouseButton.Right) { RoutedEvent = TrayMouseButtonDownEvent });
        }

        #endregion Event Handlers

        #region Event Handlers

        private void OnMouseLeftButtonDown(TrayMouseEventArgs e) => RaiseEvent(e);

        private void OnMouseLeftButtonUp(TrayMouseEventArgs e)
        {
            var ce = new TrayMouseClickEventArgs(false, MouseButton.Left)
            {
                RoutedEvent = TrayMouseButtonClickEvent,
                IsDoubleClick = _leftDoubleClick
            };

            RaiseTrayMouseButtonClick(ce);
            RaiseEvent(e);
            _leftDoubleClick = false;
        }

        private void OnMouseRightButtonDown(TrayMouseEventArgs e) => RaiseEvent(e);

        private void OnMouseRightButtonUp(TrayMouseEventArgs e)
        {
            var ce = new TrayMouseClickEventArgs(false, MouseButton.Right)
            {
                RoutedEvent = TrayMouseButtonClickEvent,
                IsDoubleClick = _rightDoubleClick
            };

            RaiseTrayMouseButtonClick(ce);
            RaiseEvent(e);
            _rightDoubleClick = false;
        }

        private void RaiseTrayMouseButtonClick(TrayMouseClickEventArgs e)
        {
            if (ActivatePopupFilter(e))
            {
                UnsafeNativeMethods.SetForegroundWindow(_blankWin!.Handle);
            }

            RaiseEvent(e);
        }

        #endregion Event Handlers
    }
}