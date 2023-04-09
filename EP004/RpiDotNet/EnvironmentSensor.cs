using System;
using System.Device.Gpio;
using UnitsNet;

namespace RpiDotNet
{
    public class EnvironmentSensor
    {
        private Iot.Device.DHTxx.Dht11 _sensor;
        private GpioController _controller;
        private int _pinNumber;
        public EnvironmentSensor (GpioController controller, int pinNumber)
        {
            _controller = controller;
            _pinNumber = pinNumber;

            _sensor = new Iot.Device.DHTxx.Dht11 (_pinNumber, PinNumberingScheme.Logical, _controller, false);
        }

        public Conditions GetConditions ()
        {
            Conditions conditions = new Conditions ();
            Temperature temp = new Temperature ();
            RelativeHumidity humidity = new RelativeHumidity ();
            
            bool successfull = false;
            for (int i = 0; i < 20; i++)
            {
                successfull = _sensor.TryReadTemperature (out temp);
                if (successfull)
                {
                    conditions.Temperature = temp.DegreesCelsius;
                    break;
                }
                else
                {
                    Thread.Sleep (250);
                }
            }
            if (!successfull)
            {
                return null;
            }

            successfull = false;
            for (int i = 0; i < 20; i++)
            {
                successfull = _sensor.TryReadHumidity (out humidity);
                if (successfull)
                {
                    conditions.Humidity = humidity.Percent;
                    break;
                }
                else
                {
                    Thread.Sleep (250);
                }
            }
            
            return conditions;
        }

        public double GetTemperature ()
        {
            Temperature temp = new Temperature ();
            
            if (_sensor.TryReadTemperature (out temp))
            {
                return temp.DegreesCelsius;
            }
            else
            {
                
                return -1;
            }
        }

        public double GetHumitity ()
        {
            RelativeHumidity humidity = new RelativeHumidity ();

            if (_sensor.TryReadHumidity (out humidity))
            {
                return humidity.Percent;
            }
            else
            {
                return -1;
            }
        }
    }
}