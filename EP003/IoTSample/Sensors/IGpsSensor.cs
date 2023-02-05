using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTSample.Models;

namespace IoTSample.Sensors
{
    internal interface IGpsSensor
    {
        GpsDataMessage GpsLocation { get; }
    }
}
