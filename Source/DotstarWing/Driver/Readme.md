# Meadow.Foundation.FeatherWings.DotstarWing

**AdaFruit DotstarWing LED FeatherWing**

The **DotstarWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/).

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Usage

```csharp
DotstarWing dotStarWing;
MicroGraphics graphics;

public override Task Initialize()
{
    Console.WriteLine("Initialize...");

    var spiBus = Device.CreateSpiBus();
    dotStarWing = new DotstarWing(spiBus) 
    {
        Brightness = 0.1f
    };
    graphics = new MicroGraphics(dotStarWing) 
    {
        CurrentFont = new Font4x6()
    };

    return Task.CompletedTask;
}

public override Task Run()
{
    graphics.Clear();

    graphics.DrawRectangle(0, 0, 8, 4, Color.LawnGreen, true);
    graphics.DrawRectangle(2, 2, 8, 4, Color.Cyan, true);
    graphics.DrawText(0, 0, "F7", Color.White);

    graphics.Show();

    return Task.CompletedTask;
}

```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation.Featherwings](https://github.com/WildernessLabs/Meadow.Foundation.Featherwings) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
