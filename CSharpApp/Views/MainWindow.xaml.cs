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
                LastAlertText.Text = $"Última alerta: {message}";
            });
        }

        public void UpdateTimer(int seconds)
        {
            Dispatcher.Invoke(() =>
            {
                TimerText.Text = $"⏱️ Temporizador: {seconds}/900 segundos";
            });
        }
    }
}