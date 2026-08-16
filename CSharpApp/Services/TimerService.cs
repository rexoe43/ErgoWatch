using System.Timers;
using Serilog;

namespace CSharpApp.Services
{
    public class TimerService
    {
        private System.Timers.Timer _timer;
        private int _seconds = 0;
        private int _maxSeconds = 900;
        private bool _isRunning = false;
        private bool _alertTriggered = false;

        public event EventHandler<int>? TimerUpdated;
        public event EventHandler? TimerThresholdReached;

        public TimerService()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerTick;
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _seconds = 0;
            _alertTriggered = false;
            _timer.Start();
            Log.Information("Timer started");
        }

        public void Reset()
        {
            _seconds = 0;
            _alertTriggered = false;
            TimerUpdated?.Invoke(this, _seconds);
            Log.Information("Timer reseted");
        }

        public void Stop()
        {
            _isRunning = false;
            _timer.Stop();
            Log.Information("Timer stopped");
        }

        private void OnTimerTick(object? sender, ElapsedEventArgs e)
        {
            _seconds++;

            TimerUpdated?.Invoke(this, _seconds);

            if (_seconds >= _maxSeconds && !_alertTriggered)
            {
                _alertTriggered = true;
                TimerThresholdReached?.Invoke(this, EventArgs.Empty);
                Log.Information("Limit time reached ({Seconds} segundos)", _seconds);

                Stop();
            }
        }

        public int GetSeconds() => _seconds;
        public int GetMaxSeconds() => _maxSeconds;
        public bool IsRunning() => _isRunning;

        public void SetMaxSeconds(int seconds)
        {
            _maxSeconds = seconds;
        }
    }
}