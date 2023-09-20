# Meadow.Foundation.FeatherWings.CharlieWing

**AdaFruit CharlieWing Matrix LED FeatherWing**

The **CharlieWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/)

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/), to view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/)

## Usage

```csharp
CharlieWing charlieWing;
MicroGraphics graphics;

public override Task Initialize()
{
    Console.WriteLine("Initialize...");

    charlieWing = new CharlieWing(Device.CreateI2cBus());

    graphics = new MicroGraphics(charlieWing) 
    {
        CurrentFont = new Font4x8()
    };

    return Task.CompletedTask;
}

public override Task Run()
{
    graphics.Clear();

    graphics.DrawText(0, 0, "F7");

    graphics.Show();

    return Task.CompletedTask;
}

```
