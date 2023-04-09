using System.Device.Gpio;

namespace RpiDotNet
{
    public class WarningLight
    {
        private GpioController _controller;
        private bool _lightOn = false;
        private int _pinNumber;

        private bool _enableWarning = false;

        private System.Threading.Timer _warningTimer;

        public WarningLight (GpioController controller, int pinNumber)
        {
            _controller = controller;
            _controller.OpenPin (pinNumber, PinMode.Output);
            _pinNumber = pinNumber;

            // CReate a timer object to handle the warnings for tempereatire being out of range.
            _warningTimer = new Timer (new TimerCallback (DoWarningLight));
        }

        public void BlinkLight ()
        {
            _lightOn = !_lightOn;
            _controller.Write (_pinNumber, ((_lightOn) ? PinValue.High : PinValue.Low));
        }

        private void DoWarningLight (Object? stateInfo)
        {
            BlinkLight ();
        }

        public void SetWarning (bool enable)
        {
            _enableWarning = enable;
            if (_enableWarning)
            {
                // Turn on the warning light and flash it every second.
                _warningTimer.Change (0, 1000);
            }
            else
            {
                // Disable the timer.
                _warningTimer.Change (System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                if (_lightOn)
                {
                    BlinkLight ();
                }
            }
            
        }
    }
}