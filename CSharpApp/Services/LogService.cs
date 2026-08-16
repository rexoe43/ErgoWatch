using Serilog;
using System.IO;

namespace CSharpApp.Services
{
    public static class LogService
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized) return;

            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "app.log");
            var logDirectory = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory!);
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logPath,
                       rollingInterval: RollingInterval.Day,
                       retainedFileCountLimit: 7)
                .MinimumLevel.Information()
                .CreateLogger();

            _isInitialized = true;
            Log.Information("Ergowatch started");
        }

        public static void Shutdown()
        {
            if (_isInitialized)
            {
                Log.Information("Ergowatch Closed");
                Log.CloseAndFlush();
                _isInitialized = false;
            }
        }
    }
}