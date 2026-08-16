using System.Windows;

namespace CSharpApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        public void UpdateTimer(string time)
        {
            Dispatcher.Invoke(() =>
            {
                TimerText.Text = $"Timer: {time}";
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