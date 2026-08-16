using System.IO;
using System.IO.Pipes;
using System.Text;
using Newtonsoft.Json;
using CSharpApp.Models;
using Serilog;

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
            Console.WriteLine($"Named Pipe service started: {PIPE_NAME}");
        }

        public void Stop()
        {
            _isRunning = false;
            _pipeServer?.Dispose();
            _listenerThread?.Join(1000);
            Log.Information("Named Pipe service stopped");
            Console.WriteLine("Named pipe service stopped");
        }

        private void ListenForConnections()
        {
            while(_isRunning)
            {
                try
                {
                    Console.WriteLine("Creating pipe and waiting for connection..");
                    _pipeServer = new NamedPipeServerStream(
                        PIPE_NAME,
                        PipeDirection.InOut,
                        1,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous
                    );
                    
                    Console.WriteLine("Waiting for Python connection...");
                    Log.Debug("Waiting for Python connection..");

                    _pipeServer.WaitForConnection();

                    Console.WriteLine("Client connected to pipe");
                    Log.Information("Client connected to pipe");

                    ProcessClient(_pipeServer);

                    _pipeServer.Disconnect();
                    _pipeServer.Dispose();
                    _pipeServer = null;

                    Console.WriteLine("Client disconnected, waiting for new connection");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in the service (Named Pipe)");
                    Console.WriteLine("Error: {ex.Message}");
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

                    Console.WriteLine($"Message received: {message}");
                    Log.Debug("Message received: {Mesage}", message);

                    ProcessMessage(message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Procesing error");
                Console.WriteLine($"Processing error: {ex.Message}");
            }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                var alert = JsonConvert.DeserializeObject<AlertModel>(message);

                if (alert != null)
                {
                    Console.WriteLine($"Alert received: {alert.Type} - {alert.Severity}");
                    Log.Information("Alert received: {Type} - {Severity} - {Message}", alert.Type, alert.Severity, alert.Message);
                    OnAlertReceived(alert);
                }
                else
                {
                    Console.WriteLine($"Could not deserialize: {message}");
                    Log.Warning("The message could not be deserialized: {Message}", message);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error with the processing message: {Message}", message);
                Console.WriteLine($"Error processing: {ex.Message}");
            }
        }

        protected virtual void OnAlertReceived(AlertModel alert)
        {
            AlertReceived?.Invoke(this, alert);
        }

        public void SendResponse(string response)
        {
            if (_pipeServer != null && _pipeServer.IsConnected)
            {
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(response + "\n");
                    _pipeServer.Write(bytes, 0, bytes.Length);
                    _pipeServer.Flush();
                }
                catch(Exception ex)
                {
                    Log.Error(ex, "Error sending the response");
                }
            }
        }
    }
}