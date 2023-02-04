using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IoTSample.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WaterLevelMessage
    {
        public WaterLevelMessage ()
        {
            SensorId = string.Empty;
            MessageId = -1;
            ReadingDate = DateTime.Now;
            GpsLocation = string.Empty;
            WaterLevel = -1;
        }
        
        // create a constructor for all properties
        public WaterLevelMessage (string sensorId, int messageId, DateTime readingDate, string gpsLocation, double waterLevel)
        {
            SensorId = sensorId;
            MessageId = messageId;
            ReadingDate = readingDate;
            GpsLocation = gpsLocation;
            WaterLevel = waterLevel;
        }



        [JsonPropertyName("sensorId")]
        public string SensorId { get; }
        [JsonPropertyName("messageId")]
        public int MessageId { get; }
        [JsonPropertyName("readingDate")]
        public DateTime ReadingDate { get; }
        [JsonPropertyName("gpsLocation")]
        public string GpsLocation { get; }
        [JsonPropertyName("waterLevel")]
        public double WaterLevel { get; }

    }
}
