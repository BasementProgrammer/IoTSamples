// See https://aka.ms/new-console-template for more information
using System.Device.Gpio;
using RpiDotNet;

Console.WriteLine("Hello, World!");

int _LightPin = 18;
int _switchPin = 17;
int _environmentPin = 16;

using GpioController _controller = new GpioController (PinNumberingScheme.Logical);
WarningLight _warningLight = new WarningLight (_controller, _LightPin);
EnvironmentSensor _environment = new EnvironmentSensor (_controller, _environmentPin);

_controller.OpenPin (_switchPin, PinMode.InputPullUp);
_controller.RegisterCallbackForPinValueChangedEvent(
    _switchPin,
    PinEventTypes.Falling | PinEventTypes.Rising,
    OnPinEvent);


while (true)
{
    //_warningLight.BlinkLight ();
    var tempHumidity = _environment.GetConditions ();
    if (tempHumidity != null)
    {
        Console.WriteLine ("Conditions = " + tempHumidity.Temperature + "c - " + tempHumidity.Humidity + "%");
    }
    System.Threading.Thread.Sleep (1000);
}

void OnPinEvent(object sender, PinValueChangedEventArgs args)
{     
    if (args.ChangeType == PinEventTypes.Falling)
    {
        // Door is open. There is no need to send sensor data.
        Console.WriteLine ("Trigger");
        _warningLight.SetWarning (true);
    }
    else
    {
        // Door is closed, we should send sensor data.
        Console.WriteLine ("Clear");
        _warningLight.SetWarning(false);
    }
}