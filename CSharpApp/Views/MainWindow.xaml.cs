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
    }
}