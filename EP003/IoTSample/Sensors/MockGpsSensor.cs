using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTSample.Sensors
{
    internal class MockGpsSensor : IGpsSensor
    {
        private Random _random = new Random();

        private GpsData _location;

        public MockGpsSensor ()
        {
            _location = new GpsData
            {
                Latitude = _random.NextDouble() * 180 - 90,
                Longitude = _random.NextDouble() * 360 - 180
            };
        }

        public GpsData GpsLocation
        {
            get
            {
                return _location;
            }
        }
    }

}
