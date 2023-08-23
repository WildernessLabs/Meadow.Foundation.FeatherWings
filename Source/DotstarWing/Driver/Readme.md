# Meadow.Foundation.FeatherWings.DotstarWing

**AdaFruit DotstarWing LED FeatherWing**

The **DotstarWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/)

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/), to view all of Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/)

## Usage

```
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

