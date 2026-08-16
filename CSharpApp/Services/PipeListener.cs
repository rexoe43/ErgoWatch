using System.IO.Pipes;
using System.Text;
using Newtonsoft.Json;
using CSharpApp.Models;
using Serilog;
using System.Diagnostics;
using System.IO;

namespace CSharpApp.Services
{
    public class PipeListener
    {
        private const string PIPE_NAME = "ergonomics_pipe";
        private NamedPipeServerStream? _pipeServer;
        private Thread? _listenerThread;
        private bool _isRunning;

        public event EventHandler<AlertModel>? AlertReceived;

        public void Start()
        {
            if(_isRunning) return;

            _isRunning = true;
            _listenerThread = new Thread(ListenForConnections);
            _listenerThread.IsBackground = true;
            _listenerThread.Start();

            Log.Information("Named Pipe service started: {PipeName}", PIPE_NAME);
        }

        public void Stop()
        {
            _isRunning = false;
            _pipeServer?.Dispose();
            _listenerThread?.Join(1000);
            Log.Information("Named Pipe service stopped");
        }

        private void ListenForConnections()
        {
            while(_isRunning)
            {
                try
                {
                    _pipeServer = new NamedPipeServerStream(
                        PIPE_NAME,
                        PipeDirection.InOut,
                        1,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous
                    );

                    Log.Debug("Waiting for Python connection..");

                    _pipeServer.WaitForConnection();
                    Log.Information("Client connected to pipe");
                    ProcessClient(_pipeServer);
                    _pipeServer.Disconnect();
                    _pipeServer.Dispose();
                    _pipeServer = null;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in the service (Named Pipe)");
                    Thread.Sleep(1000);
                }
            }
        }

        private void ProcessClient(NamedPipeServerStream pipeServer)
        {
            try
            {
                using var reader = new StreamReader(pipeServer, Encoding.UTF8);
                string? message;

                while (_isRunning && pipeServer.IsConnected)
                {
                    message = reader.ReadLine();
                    if (string.IsNullOrEmpty(message))
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    Log.Debug("Message received: {Mesage}", message);
                    ProcessMessage(message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Procesing error");
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                var alert = JsonConvert.DeserializeObject<AlertModel>(message);

                if (alert != null)
                {
                    Log.Information("Alert received: {Type} - {Severity} - {Message}", alert.Type, alert.Severity, alert.Message);
                    OnAlertReceived(alert);
                }
                else
                {
                    Log.Warning("The message could not be deserialized: {Message}", message);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error with the processing message: {Message}", message);
            }
        }

        protected virtual void OnAlertReceived(AlertModel alert)
        {
            AlertReceived?.Invoke(this, alert);
        }
    }
}