# Meadow.Foundation.FeatherWings.ServoWing

**AdaFruit ServoWing servo controller FeatherWing**

The **ServoWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/)

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/), to view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/)

## Usage

```csharp
ServoWing servoWing;
Servo servo;

public override Task Initialize()
{
    Console.WriteLine("Initializing ...");

    servoWing = new ServoWing(Device.CreateI2cBus(I2cBusSpeed.FastPlus));

    servo = servoWing.GetServo(0, NamedServoConfigs.SG90);

    return Task.CompletedTask;
}

public override async Task Run()
{
    while (true)
    {
        Console.WriteLine("0");
        await servo.RotateTo(new Angle(0, AU.Degrees));
        await Task.Delay(1000);

        Console.WriteLine("45");
        await servo.RotateTo(new Angle(45, AU.Degrees));
        await Task.Delay(1000);

        Console.WriteLine("90");
        await servo.RotateTo(new Angle(90, AU.Degrees));
        await Task.Delay(1000);

        Console.WriteLine("135");
        await servo.RotateTo(new Angle(135, AU.Degrees));
        await Task.Delay(1000);
    }
}

```
