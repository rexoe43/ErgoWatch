using System.Windows;
using Forms = System.Windows.Forms;

namespace CSharpApp
{
    public partial class App : System.Windows.Application
    {
        private Forms.NotifyIcon? _trayIcon;
        private Window? _mainWindow;
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateTrayIcon();
            _mainWindow = new Views.MainWindow();
            _mainWindow.Show();
        }

        private void CreateTrayIcon()
        {
            _trayIcon = new Forms.NotifyIcon();
            _trayIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            _trayIcon.Text = "Ergo Watch - Ergonomic Monitoring Application";
            _trayIcon.Visible = true;
            var contextMenu = new Forms.ContextMenuStrip();
            var showItem = new Forms.ToolStripMenuItem("Show");
            showItem.Click += (s, e) =>
            {
                if (_mainWindow != null)
                {
                    _mainWindow.Show();
                    _mainWindow.WindowState = WindowState.Normal;
                    _mainWindow.Activate();
                }
            };
            contextMenu.Items.Add(showItem);
            var hideItem = new Forms.ToolStripMenuItem("Hide");
            hideItem.Click += (s, ev) =>
            {
                if (_mainWindow != null)
                {
                    _mainWindow.Hide();
                }
            };
            contextMenu.Items.Add(hideItem);
            contextMenu.Items.Add(new Forms.ToolStripSeparator());
            var exitItem = new Forms.ToolStripMenuItem("Exit");
            exitItem.Click += (s, ev) =>
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            };
            contextMenu.Items.Add(exitItem);
            _trayIcon.ContextMenuStrip = contextMenu;
            _trayIcon.DoubleClick += (s, ev) =>
            {
                if (_mainWindow != null)
                {
                    if (_mainWindow.IsVisible)
                    {
                        _mainWindow.Hide();
                    }
                    else
                    {
                        _mainWindow.Show();
                        _mainWindow.WindowState = WindowState.Normal;
                        _mainWindow.Activate();
                    }
                }
            };

            _trayIcon.ShowBalloonTip(
                3000,
                "ErgoWatch",
                "Ergonomic Monitor inicialized correctly",
                Forms.ToolTipIcon.Info
            );
        }

        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }
            base.OnExit(e);
            
        }
    }
}