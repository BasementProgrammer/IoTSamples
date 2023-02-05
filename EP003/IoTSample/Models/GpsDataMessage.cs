namespace IoTSample.Models
{
    public class GpsDataMessage
    {
        public double Latitude { get; internal set; }
        public double Longitude { get; internal set; }

        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}";
        }
    }
}