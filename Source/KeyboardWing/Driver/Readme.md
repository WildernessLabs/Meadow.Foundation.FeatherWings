# Meadow.Foundation.FeatherWings.KeyboardWing

**BBQ 10 Keyboard FeatherWing**

The **KeyboardWing** library is included in the **Meadow.Foundation.FeatherWings.KeyboardWing** nuget package and is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform.

This driver is part of the [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/) peripherals library, an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT applications.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Installation

You can install the library from within Visual studio using the the NuGet Package Manager or from the command line using the .NET CLI:

`dotnet add package Meadow.Foundation.FeatherWings.KeyboardWing`
## Usage

```csharp
KeyboardWing keyboardWing;
MicroGraphics graphics;

string lastKeyPress;

public override Task Initialize()
{
    Console.WriteLine("Initializing ...");

    var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
    var spiBus = Device.CreateSpiBus(new Meadow.Units.Frequency(48000, Meadow.Units.Frequency.UnitType.Kilohertz));

    keyboardWing = new KeyboardWing(
        spiBus: spiBus,
        i2cBus: i2cBus,
        keyboardPin: Device.Pins.D10,
        displayChipSelectPin: Device.Pins.D11,
        displayDcPin: Device.Pins.D12,
        lightSensorPin: Device.Pins.A05);

    keyboardWing.TouchScreen.Rotation = RotationType._90Degrees;

    graphics = new MicroGraphics(keyboardWing.Display)
    {
        Rotation = RotationType._90Degrees,
        CurrentFont = new Font12x16()
    };

    keyboardWing.Keyboard.OnKeyEvent += Keyboard_OnKeyEvent;

    return Task.CompletedTask;
}

public override Task Run()
{
    graphics.Clear(true);

    keyboardWing.LightSensor.StartUpdating(new TimeSpan(0, 0, 30));

    return Task.CompletedTask;
}

private void Keyboard_OnKeyEvent(object sender, Meadow.Foundation.Sensors.Hid.BBQ10Keyboard.KeyEvent e)
{
    if (e.KeyState == BBQ10Keyboard.KeyState.StatePress)
    {
        Console.WriteLine($"OnKeyEvent ASCII value: {(byte)e.AsciiValue}");

        lastKeyPress = (byte)e.AsciiValue switch
        {
            (byte)ButtonType._5WayUp => "5-way up",
            (byte)ButtonType._5WayDown => "5-way down",
            (byte)ButtonType._5WayLeft => "5-way left",
            (byte)ButtonType._5WayRight => "5-way right",
            (byte)ButtonType._5WayCenter => "5-way center",
            (byte)ButtonType.Button1 => "Button 1",
            (byte)ButtonType.Button2 => "Button 2",
            (byte)ButtonType.Button3 => "Button 3",
            (byte)ButtonType.Button4 => "Button 4",
            _ => e.AsciiValue.ToString()
        };
    }

    UpdateDisplay();
}

void UpdateDisplay()
{
    graphics.Clear();

    graphics.DrawText(0, 0, $"Last pressed: {lastKeyPress}");
    graphics.DrawText(0, 16, $"Luminance: {keyboardWing.LightSensor.Illuminance.Value.Lux}");

    graphics.Show();
}

```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation.Featherwings](https://github.com/WildernessLabs/Meadow.Foundation.Featherwings) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
## About Meadow

Meadow is a complete, IoT platform with defense-grade security that runs full .NET applications on embeddable microcontrollers and Linux single-board computers including Raspberry Pi and NVIDIA Jetson.

### Build

Use the full .NET platform and tooling such as Visual Studio and plug-and-play hardware drivers to painlessly build IoT solutions.

### Connect

Utilize native support for WiFi, Ethernet, and Cellular connectivity to send sensor data to the Cloud and remotely control your peripherals.

### Deploy

Instantly deploy and manage your fleet in the cloud for OtA, health-monitoring, logs, command + control, and enterprise backend integrations.


