# Meadow.Foundation.FeatherWings.OLED128x32Wing

**AdaFruit OLED 128x32 monochrome display FeatherWing**

The **OLED128x32** library is included in the **Meadow.Foundation.FeatherWings.OLED128x32Wing** nuget package and is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform.

This driver is part of the [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/) peripherals library, an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT applications.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Installation

You can install the library from within Visual studio using the the NuGet Package Manager or from the command line using the .NET CLI:

`dotnet add package Meadow.Foundation.FeatherWings.OLED128x32Wing`
## Usage

```csharp
OLED128x32Wing oledWing;
MicroGraphics graphics;

public override Task Initialize()
{
    Resolver.Log.Info("Initializing...");
    var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);

    oledWing = new OLED128x32Wing(i2cBus, Device.Pins.D11, Device.Pins.D10, Device.Pins.D09);

    graphics = new MicroGraphics(oledWing.Display);
    graphics.CurrentFont = new Font12x16();

    oledWing.ButtonA.Clicked += (sender, e) => UpdateDisplay("A Clicked");
    oledWing.ButtonB.Clicked += (sender, e) => UpdateDisplay("B Clicked");
    oledWing.ButtonC.Clicked += (sender, e) => UpdateDisplay("C Clicked");
    UpdateDisplay("Ready");

    return Task.CompletedTask;
}

void UpdateDisplay(string message)
{
    graphics.Clear();
    graphics.DrawText(x: 0, y: 8, message);
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


