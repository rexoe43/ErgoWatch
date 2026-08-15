using System.Windows;

namespace CSharApp
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            var app = new App();
            app.Run();
        }
    }
}