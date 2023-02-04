namespace IoTSample.Sensors
{
    public class GpsData
    {
        public double Latitude { get; internal set; }
        public double Longitude { get; internal set; }

        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}";
        }
    }
}