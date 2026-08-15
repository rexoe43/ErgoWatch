using System.Windows;

namespace CSharpApp
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