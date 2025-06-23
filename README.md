# TrayIcon
This is a tiny library that provides a tray icon for the WPF platform. It is mainly used for my personal "toys" and has not been widely tested. Therefore, DO NOT use it in production projects unless you have thoroughly tested it for all your use cases.

Only use it if you want to make your toys as well.

---

## How to use it

The `TrayNotifyIcon` is the key class. Since it's a `ContentControl`, you can simply place any custom visual you like inside the `TrayNotifyIcon` element.

```xaml
<Window
	x:Class="MoyuTokei.Views.MainWindow"
	...
	xmlns:tray="clr-namespace:LenChon.Win32.TrayIcon;assembly=TrayIcon">
	<Grid>
 		<tray:TrayNotifyIcon
			IconPath="app.ico"
			TrayIconToolTip="Nice day"
			TrayPopupActivationMethod="LeftDoubleClick">
				<!-- Your custom control here -->
		<tray:TrayNotifyIcon.TrayPopupOpeningAnimation>
				<!-- Optional: Apply a storyboard for the opening animation -->
		</tray:TrayNotifyIcon.TrayPopupOpeningAnimation>
			<tray:TrayNotifyIcon.TrayPopupClosingAnimation>
				<!-- Same as TrayPopupOpeningAnimation -->
			</tray:TrayNotifyIcon.TrayPopupClosingAnimation>
		</tray:TrayNotifyIcon>
	</Grid>
</Window>
```

