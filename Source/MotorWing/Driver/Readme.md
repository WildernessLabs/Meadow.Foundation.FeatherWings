# Meadow.Foundation.FeatherWings.MotorWing

**AdaFruit MotorWing motor controller FeatherWing**

The **MotorWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/).

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Usage

```csharp
MotorWing motorWing;

public override Task Initialize()
{
    Console.WriteLine("Initializing ...");

    var i2CBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
    motorWing = new MotorWing(i2CBus, new Frequency(100, Frequency.UnitType.Hertz), 0x61);
    motorWing.Initialize();

    return Task.CompletedTask;
}

public override async Task Run()
{
    //Get DC motor 1
    var dcMotor1 = motorWing.GetMotor(1);

    //Get DC motor 2
    var dcMotor2 = motorWing.GetMotor(2);

    //Get Stepper motor number 2
    var stepper = motorWing.GetStepper(2, 200);

    dcMotor1.Run(Commmand.FORWARD);
    dcMotor2.Run(Commmand.BACKWARD);

    while (true)
    {
        Console.WriteLine("Speed up");
        for (short i = 0; i <= 255; i++)
        {
            dcMotor1.SetSpeed(i);
            dcMotor2.SetSpeed(i);
            await Task.Delay(10);
        }

        stepper.Step(50);

        await Task.Delay(500);

        Console.WriteLine("Slow down");
        for (short i = 255; i >= 0; i--)
        {
            dcMotor1.SetSpeed(i);
            dcMotor2.SetSpeed(i);
            await Task.Delay(10);
        }

        stepper.Step(-50);

        await Task.Delay(500);
    }
}

```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation.Featherwings](https://github.com/WildernessLabs/Meadow.Foundation.Featherwings) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
