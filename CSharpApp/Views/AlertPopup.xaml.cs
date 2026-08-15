using System.Windows;

namespace CSharApp
{
    public partial class ALertPopup : Window
    {
        public AlertPopup(string message, string details = "")
        {
            InitializeComponent();
            AlertMessage.Text = message;
            AlertDetails.Text = details;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void IgnoreButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}