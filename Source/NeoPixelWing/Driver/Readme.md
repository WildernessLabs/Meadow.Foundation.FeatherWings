# Meadow.Foundation.FeatherWings.NeoPixelWing

**AdaFruit NeoPixel FeatherWing**

The **NeoPixelWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/)

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/), to view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/)

## Usage

```csharp
NeoPixelWing neoWing;
MicroGraphics graphics;

public override Task Initialize()
{
    Console.WriteLine("Initializing ...");

    neoWing = new NeoPixelWing(Device.CreateSpiBus());

    graphics = new MicroGraphics(neoWing);

    return Task.CompletedTask;
}

public override Task Run()
{
    graphics.Clear();

    graphics.DrawRectangle(0, 0, 8, 4, false);

    graphics.Show();

    return Task.CompletedTask;
}

```
