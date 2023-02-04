using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTSample.Sensors
{
    public class MockWaterLevelSensor : IWaterLevelSensor
    {
        // Maximum water level in mm
        public const double MaxWaterLevel = 30000;

        public const double MinWaterLevel = 0;

        public const double MaxWaterAdgustment = 10;

        private Random _random = new Random();

        private double _waterLevel;

        public MockWaterLevelSensor()
        {
            _waterLevel = _random.NextDouble() * 100;
        }

        private void RandomStepWaterLevel ()
        {
            // an adjustment value that is between -MaxWaterAdjustment and +MaxWaterAdjustment
            // 0.7 * (2 * 10) - 10 = 0.7 * 20 - 10 = 14 - 10 = 4
            var waterAdjustment = (_random.NextDouble() * (2 * MaxWaterAdgustment)) - MaxWaterAdgustment;
            _waterLevel += waterAdjustment;
            if (_waterLevel > MaxWaterLevel)
            {
                _waterLevel = MaxWaterLevel;
            }
            else if (_waterLevel < MinWaterLevel)
            {
                _waterLevel = MinWaterLevel;
            }
        }
        
        public double WaterLevel
        {
            get
            {
                RandomStepWaterLevel();
                return _waterLevel;
            }
        }
    }
}
