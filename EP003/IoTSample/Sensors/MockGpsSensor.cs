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
                /*
                    Generate a random locationI've removed the random Lat, Long generation and replaced it with a static location someplace in the Australian Outback.
                */
                //Latitude = _random.NextDouble() * 180 - 90,
                //Longitude = _random.NextDouble() * 360 - 180
                Latitude = -22.545210,
                Longitude = 128.500041
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
