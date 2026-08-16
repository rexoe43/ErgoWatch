using Newtonsoft.Json;

namespace CSharpApp.Models
{
    public class AlertModel
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("severity")]
        public string Severity { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message {get; set; } = string.Empty;

        [JsonProperty("details")]
        public AlertDetails Details {get; set;} = new AlertDetails();
    }

    public class AlertDetails
    {
        [JsonProperty("neck_angle")]
        public double NeckAngle {get; set;}

        [JsonProperty("shoulder_angle")]
        public double ShoulderAngle {get; set;}

        [JsonProperty("blink_rate")]
        public double BlinkRate {get; set;}

        [JsonProperty("duration_seconds")]
        public int DurationSeconds {get; set;}

        [JsonProperty("message")]
        public string Message {get; set;} = string.Empty;
    }
}