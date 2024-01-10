# Meadow.Foundation.FeatherWings.OLED128x64Wing

**AdaFruit OLED 128x64 monochrome display FeatherWing**

The **OLED128x64** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/).

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Usage

```csharp
OLED128x64Wing oledWing;
MicroGraphics graphics;

public override Task Initialize()
{
    Resolver.Log.Info("Initializing...");
    var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);

    oledWing = new OLED128x64Wing(i2cBus, Device.Pins.D11, Device.Pins.D10, Device.Pins.D09);

    graphics = new MicroGraphics(oledWing.Display);
    // Use RotationType.Default for "native" screen orientation or RotationType._90Degrees for "wide" orientation.
    graphics.Rotation = RotationType._90Degrees;
    graphics.CurrentFont = new Font8x8();

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
