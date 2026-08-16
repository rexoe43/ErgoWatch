using System.Windows;
using System.ComponentModel;
using CSharpApp.Services;
namespace CSharpApp.Views
{
    public partial class MainWindow : Window
    {
        private TimerService? _timerService;
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing!;
            
            _timerService = new TimerService();
            _timerService.TimerUpdated += OnTimerUpdated;
            _timerService.TimerThresholdReached += OnTimerThresholdReached;
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void OnTimerUpdated(object? sender, int seconds)
        {
            UpdateTimer(seconds);
        }

        private void OnTimerThresholdReached(object? sender, EventArgs e)
        {
            UpdateStatus("¡15 minutes with a bad posture! Take a rest");

            var alertPopup = new AlertPopup(
                "¡Time to active pause",
                "You has been with a bad posture for 15 minutes. Please, stand up and walk."
            );
            alertPopup.ShowDialog();
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

        public void StartTimer()
        {
            _timerService?.Start();
        }
        
        public void ResetTimer()
        {
            _timerService?.Reset();
            UpdateStatus("System Status: Monitoring posture...");
        }

        public void StopTimer()
        {
            _timerService?.Stop();
        }
    }
}