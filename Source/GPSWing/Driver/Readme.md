# Meadow.Foundation.FeatherWings.GPSWing

**AdaFruit GPSWing GPS FeatherWing**

The **GPSWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/).

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Usage

```csharp
GPSWing gps;

public override Task Initialize()
{
    Console.WriteLine("Initialize...");

    var serial = Device.CreateSerialMessagePort(
        Device.PlatformOS.GetSerialPortName("Com4"),
        suffixDelimiter: Encoding.ASCII.GetBytes("\r\n"),
        preserveDelimiter: true,
        baudRate: 9600);

    gps = new GPSWing(serial);

    gps.GgaReceived += (object sender, GnssPositionInfo location) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine(location);
        Console.WriteLine("*********************************************");
    };

    // GLL
    gps.GllReceived += (object sender, GnssPositionInfo location) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine(location);
        Console.WriteLine("*********************************************");
    };

    // GSA
    gps.GsaReceived += (object sender, ActiveSatellites activeSatellites) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine(activeSatellites);
        Console.WriteLine("*********************************************");
    };

    // RMC (recommended minimum)
    gps.RmcReceived += (object sender, GnssPositionInfo positionCourseAndTime) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine(positionCourseAndTime);
        Console.WriteLine("*********************************************");

    };

    // VTG (course made good)
    gps.VtgReceived += (object sender, CourseOverGround courseAndVelocity) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine($"{courseAndVelocity}");
        Console.WriteLine("*********************************************");
    };

    // GSV (satellites in view)
    gps.GsvReceived += (object sender, SatellitesInView satellites) => 
    {
        Console.WriteLine("*********************************************");
        Console.WriteLine($"{satellites}");
        Console.WriteLine("*********************************************");
    };

    return Task.CompletedTask;
}

public override Task Run()
{
    gps.StartUpdating();

    return Task.CompletedTask;
}

```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation.Featherwings](https://github.com/WildernessLabs/Meadow.Foundation.Featherwings) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
