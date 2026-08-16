using System.Windows;
using System.ComponentModel;
namespace CSharpApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing!;
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = message;
            });
        }

        public void UpdateLastAlert(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LastAlertText.Text = $"Last Alert: {message}";
            });
        }

        // Versión con segundos
        public void UpdateTimer(int seconds)
        {
            Dispatcher.Invoke(() =>
            {
                int minutes = seconds / 60;
                int remainingSeconds = seconds % 60;
                TimerText.Text = $"Timer: {minutes:D2}:{remainingSeconds:D2}";
            });
        }
    }
}