using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTSample.Models;

namespace IoTSample.Sensors
{
    internal class MockGpsSensor : IGpsSensor
    {
        private Random _random = new Random();

        private GpsDataMessage _location;

        public MockGpsSensor ()
        {
            _location = new GpsDataMessage
            {
                Latitude = _random.NextDouble() * 180 - 90,
                Longitude = _random.NextDouble() * 360 - 180
            };
        }

        public GpsDataMessage GpsLocation
        {
            get
            {
                return _location;
            }
        }
    }

}
