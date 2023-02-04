using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTSample.Sensors
{
    internal interface IGpsSensor
    {
        GpsData GpsLocation { get; }
    }
}
