using System.Windows;

namespace CSharpApp.Views
{
    public partial class AlertPopup : Window
    {
        public AlertPopup()
        {
            InitializeComponent();
        }

        public AlertPopup(string message, string details = "")
        {
            InitializeComponent();
            AlertMessage.Text = message;
            AlertDetails.Text = details;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void IgnoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}