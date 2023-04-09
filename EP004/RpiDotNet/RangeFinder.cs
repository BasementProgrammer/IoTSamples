using System;
using System.Device.Gpio;
using Iot.Device.Hcsr04;

namespace RpiDotNet
{
    // Create the RangeFinder Class
    public class RangeFinder
    {
        private const int PIN_ECHO = 17;
        private const int PIN_TRIGGER = 4;

        Hcsr04 sensor;

        public RangeFinder()
        {
            // Set the GPIO Pins
            GpioController controller = new GpioController();
            controller.OpenPin(PIN_ECHO, PinMode.Input);
            controller.OpenPin(PIN_TRIGGER, PinMode.Output);
            sensor = new Hcsr04(PIN_TRIGGER, PIN_ECHO);

        }

        public double GetDistance()
        {
            if (sensor.TryGetDistance(out var distance))
            {
                return distance.Centimeters;
            }
            else
            {
                return -1;
            }
        }
    }
}